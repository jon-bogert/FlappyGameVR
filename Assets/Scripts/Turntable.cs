using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turntable : MonoBehaviour
{
    [SerializeField] float rpm = 10;

    float degreesPerSecond;

    // Start is called before the first frame update
    void Start()
    {
        degreesPerSecond = rpm * 360 / 60f;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("time: " + Time.deltaTime);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + (degreesPerSecond * Time.deltaTime), transform.rotation.eulerAngles.z);
    }
}
