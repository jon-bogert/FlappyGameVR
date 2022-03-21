using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] float flickerRefresh = 0.1f;
    [SerializeField] float flickerAmount = 0.5f;
    
    Light light;
    float initIntensity;
    
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        initIntensity = light.intensity;
        
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            light.intensity = Random.Range(initIntensity - flickerAmount, initIntensity + flickerAmount);
            yield return new WaitForSeconds(flickerRefresh);
        }
    }
}
