using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public GameObject player;
    float speed =5.0f;
    float maxHealth = 5.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;

        direction.Normalize();

        transform.Translate(direction*speed * Time.deltaTime);
    }
}
