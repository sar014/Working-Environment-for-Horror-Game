using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FlickeringLights : MonoBehaviour
{
    [SerializeField] Light LightObj;
    [SerializeField] AudioSource lightSound;

    [SerializeField] float minTime;
    [SerializeField] float maxTime;
    [SerializeField] float timer;
    [SerializeField] float flickeringDuration;
    Boolean isFlickering = false;
    
    void Start()
    {
        //timer = Random.Range(minTime,maxTime);
        timer=0;
    }

    
    void Update()
    {
        if(isFlickering)
        {
            LightOnOff();

            flickeringDuration-=Time.deltaTime;
            
            if(flickeringDuration<=0)
            {
                isFlickering = false;
                LightObj.enabled = false;
                lightSound.Stop();
            }
        }
    
    }

    void OnTriggerEnter(Collider other) 
    {
        isFlickering =true;
        Debug.Log("Working Well");
    }

    void LightOnOff()
    {
        if(timer>0)
        timer-=Time.deltaTime;

        if(timer<=0)
        {
            LightObj.enabled = !LightObj.enabled;
            timer = UnityEngine.Random.Range(minTime,maxTime);
            lightSound.Play();
        }
    }
}
