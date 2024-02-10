using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    [SerializeField] GameObject flashlight;

    [SerializeField] AudioSource turnOn;
    [SerializeField] AudioSource turnOff;

    private bool on;
    private bool off;
    
    void Start()
    {
        off = true;
        flashlight.SetActive(false);
        
    }

    void Update()
    {
        if(off && Input.GetButtonDown("f"))
        {
            flashlight.SetActive(true);
            turnOn.Play();
            off = false;
            on = true;
        }
        else if(on && Input.GetButtonDown("f"))
        {
            flashlight.SetActive(false);
            turnOff.Play();
            off = true;
            on = false;
        }
        
    }
}
