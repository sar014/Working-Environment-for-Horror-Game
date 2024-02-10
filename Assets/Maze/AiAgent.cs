using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    [SerializeField]
    private Vector3 desiredDestination;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<NavMeshAgent>().destination = desiredDestination;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
