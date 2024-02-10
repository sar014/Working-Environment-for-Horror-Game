using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetected : MonoBehaviour
{
    Maze obj;
    private void OnTriggerEnter(Collider other)
    {
        obj.hasEntered = true;
        Debug.Log("Entered");
    }
}
