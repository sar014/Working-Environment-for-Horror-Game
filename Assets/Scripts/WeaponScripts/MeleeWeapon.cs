using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public Animator animator;
    public Transform weaponHolder;
    // GameObject weapon;
    GameObject activeWeapon;
    // Start is called before the first frame update
    void Start()
    {
        // weapon = this.gameObject;

        GameObject fpsGameObject = GameObject.Find("FirstPersonPlayer");
        Transform cameraTransform = fpsGameObject.transform.Find("Main Camera");
        weaponHolder = cameraTransform.Find("WeaponHolder")?.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Find the active weapon at the start
        UpdateActiveWeapon();
        
        if(Input.GetKeyDown(KeyCode.Z)&& activeWeapon != null)
        {

            animator = activeWeapon.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Hit");
            }
        } 
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Debug.Log("HIT");
            EnemyDamage damage = other.gameObject.GetComponent<EnemyDamage>();
            damage.TakeDamage();
        }
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     Debug.Log("Collision detected with: " + collision.gameObject.name); // Debug log for collision detection
    //     if (collision.gameObject.CompareTag("Enemy"))
    //     {
    //         Debug.Log("HIT");
    //         EnemyDamage damage = collision.gameObject.GetComponent<EnemyDamage>();
    //         if (damage != null)
    //         {
    //             damage.TakeDamage();
    //         }
    //         else
    //         {
    //             Debug.LogWarning("EnemyDamage component not found on: " + collision.gameObject.name);
    //         }
    //     }
    // }

    // Function to update the active weapon
    void UpdateActiveWeapon()
    {
        foreach (Transform child in weaponHolder)
        {
            if (child.gameObject.activeSelf)
            {
                activeWeapon = child.gameObject;
                animator = activeWeapon.GetComponent<Animator>();
                break;
            }
        }
    }
}
