using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtSound : MonoBehaviour
{
    public AudioSource src;
    public AudioClip sfx;

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag =="Player")
        {
            src.Play();
        }
    }
}
