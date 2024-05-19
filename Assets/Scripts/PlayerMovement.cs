using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    Maze mazeObj;
    public CharacterController controller;
    public Button hint;
    public AudioSource src;
    public AudioClip sfx;
    [SerializeField] float speed = 12f;
    [SerializeField] float gravity = -9.8f;

    Vector3 velocity = Vector3.zero;
    bool isWalking = false;
    bool playWalkingSound = true;
    bool walkOnGround = false;
    
    // Update is called once per frame
    void Update()
    {
        Debug.Log("Am i walking on Ground = "+walkOnGround);
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (x != 0 || z != 0)
        {
            if (!isWalking)
            {
                src.clip = sfx;
                src.loop = true;
                src.Play();
                isWalking = true;
            }

        }
        else
        {
            src.Stop();
            isWalking = false;
        }
        

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

        if(other.gameObject.name =="Ground")
        {
            walkOnGround = true;
        }
    }

}
