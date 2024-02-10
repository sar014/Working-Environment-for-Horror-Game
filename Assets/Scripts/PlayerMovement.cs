using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    Maze mazeObj;
    public CharacterController controller;
    public Button hint;
    [SerializeField] float speed = 12f;
    [SerializeField] float gravity = -9.8f;

    Vector3 velocity = Vector3.zero;
    
    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        //Falling Down Code Starts from here:
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.name =="OpeningToMaze")
        {
            hint.gameObject.SetActive(true);
        }
    }

}
