using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam =  Camera.main;
    }

    void LateUpdate()
    {
        transform.LookAt(cam.transform);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}
