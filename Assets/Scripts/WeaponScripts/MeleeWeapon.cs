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

        
    }

    // Update is called once per frame
    void Update()
    {
        // Find the active weapon at the start
        UpdateActiveWeapon();
        
        if(Input.GetMouseButtonDown(0)&& activeWeapon != null)
        {
            Debug.Log("Swing Triggered");
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
            EnemyDamage damage = other.gameObject.GetComponent<EnemyDamage>();
            damage.TakeDamage();
        }
    }

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
