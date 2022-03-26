using System;
using UnityEngine;
using UnityEngine.Audio;

public class PhysicsHand : MonoBehaviour
{
    [Header("General")]
    [SerializeField] AudioClip flapSound;

    [Header("PID")]
    [SerializeField] float frequency = 50f;
    [SerializeField] float damping = 1f;
    [SerializeField] float rotFrequency = 100f;
    [SerializeField] float rotDamping = 0.9f;
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] Transform target;
  
    [Space]
    [Header("Hooke's Law")]
    [SerializeField] float climbForce = 500f;
    [SerializeField] float climbDrag = 250f;
    [SerializeField] float triggerThreshold = 0f;
    [SerializeField] float sidewaysMultiplier = 0.5f;

    [Space]
    [Header("References")]
    [SerializeField] Transform hmdTransform;

    [Space]
    [Header("Glide Mode")]
    [SerializeField] bool aileronMode = false;
    [SerializeField] float xForceMultiplier = 1f;
    [SerializeField] float thesholdAngle = 30f;
    [SerializeField] float pitchSpeed = 3f;

    Rigidbody myRigidbody;
    Player player;
    Vector3 previousPosition;
    bool canFly;
    bool isFlapping = false;

    AudioSource audioSource;

    void Start()
    {
        player = FindObjectOfType<Player>();
        canFly = true;
        ResetPosition();
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.maxAngularVelocity = float.PositiveInfinity;
        previousPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    public void ResetPosition()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PIDMovement();
        PIDRotation();
        if (!player.GetGlideMode())
        {
            HookesLaw();
        }
        else
        {
            GlideMode();
        }
    }

    void HookesLaw()
    {
        Vector3 displacementFromResting = transform.position - target.position;
        Vector3 force = displacementFromResting * climbForce;
        float drag = GetDrag();



        if ((force.y > triggerThreshold) && canFly)
        {
            if (!playerRigidbody.useGravity)
            {
                playerRigidbody.useGravity = true;
                FindObjectOfType<World>().StartMovement();
                FindObjectOfType<UIUpdator>().DisableBeginMsg();
            }

            Vector3 upForce = Vector3.zero;
            ;
            float sideForce = (transform.position.x < hmdTransform.position.x)
                ? force.y * sidewaysMultiplier
                : -force.y * sidewaysMultiplier;
            upForce.Set(sideForce, force.y, 0f);
            playerRigidbody.AddForce(upForce, ForceMode.Acceleration);
            playerRigidbody.AddForce(drag * -playerRigidbody.velocity * climbDrag, ForceMode.Acceleration);
            if (!isFlapping) audioSource.Play();
            isFlapping = true;
        }
        else if (force.y < 0) isFlapping = false;
    }

    private float GetDrag()
    {
        Vector3 handVelocity = (target.localPosition - previousPosition) / Time.fixedDeltaTime;
        float drag = 1 / handVelocity.magnitude + 0.01f;
        drag = drag > 1 ? 1 : drag;
        drag = drag < 0.03f ? 0.03f : drag;
        previousPosition = transform.position;
        return drag;
    }

    void PIDMovement()
    {
        float kp = (6f * frequency) * (6f * frequency) * 0.25f;
        float kd = 4.5f * frequency * damping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kdg = (kd + kp * Time.fixedDeltaTime) * g;
        Vector3 force = (target.position - transform.position) * ksg + (playerRigidbody.velocity - myRigidbody.velocity) * kdg;
        myRigidbody.AddForce(force, ForceMode.Acceleration);
    }

    void PIDRotation()
    {
        float kp = (6f * rotFrequency) * (6f * rotFrequency) * 0.25f;
        float kd = 4.5f * rotFrequency * rotDamping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kdg = (kd + kp * Time.fixedDeltaTime) * g;
        Quaternion q = target.rotation * Quaternion.Inverse(transform.rotation);
        if (q.w < 0)
        {
            q.x = -q.x;
            q.y = -q.y;
            q.z = -q.z;
            q.w = -q.w;
        }
        q.ToAngleAxis(out float angle, out Vector3 axis);
        axis.Normalize();
        axis *= Mathf.Deg2Rad;
        Vector3 torque = ksg * axis * angle + -myRigidbody.angularVelocity * kdg;
        myRigidbody.AddTorque(torque, ForceMode.Acceleration);
    }

    public void DisableFlight()
    {
        canFly = false;
    }

    public void EnableFlight()
    {
        canFly = true;
    }

    void GlideMode()
    {

        //Debug.Log(target.name + "  : " + target.rotation.eulerAngles.x);
        if (canFly && InZBounds())
        {
            float angleUpDown = 0f;
            float xForce = 0f;
            float xRot = (target.rotation.eulerAngles.x <= 180f) ? target.rotation.eulerAngles.x : target.rotation.eulerAngles.x - 360f;
            if (transform.position.x < hmdTransform.position.x) // right hand
            {
                angleUpDown = Mathf.Clamp((target.rotation.eulerAngles.z - 270f), -45f, 45f) / 90f;
                xForce = (aileronMode) ? -angleUpDown : -Mathf.Clamp(xRot, -thesholdAngle, thesholdAngle) / thesholdAngle;
            }
            else
            {
                angleUpDown = Mathf.Clamp(-(target.rotation.eulerAngles.z - 90f), -45f, 45f) / 90f;
                xForce = (aileronMode) ? angleUpDown : Mathf.Clamp(xRot, -thesholdAngle, thesholdAngle) / thesholdAngle;
            }
            Vector3 wingForce = Vector3.zero;
            wingForce.Set(xForce * xForceMultiplier - playerRigidbody.velocity.x, (4.9f * (pitchSpeed * angleUpDown + 1) - playerRigidbody.velocity.y), 0f);
            playerRigidbody.AddForce(wingForce, ForceMode.Acceleration);
            //target.position += wingForce;
            //Debug.Log(target.name + "  : " + angleUpDown);
        }
    }

    bool InZBounds()
    {
           return (target.rotation.eulerAngles.x > (360f - thesholdAngle) || target.rotation.eulerAngles.x < thesholdAngle);

    }
}
