using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpact : MonoBehaviour
{

    //Used to detect when a game object collides with another game object.
        //Both colliders should not be marked as triggers (they are regular colliders).
        //At least one of the game objects involved must have a Rigidbody component.
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            EnemyDamage damage = other.gameObject.GetComponent<EnemyDamage>();
            CreateBulletImpactEffect(other);
            damage.TakeDamage();
        }
    }

    void CreateBulletImpactEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        //Using Singleton (global reference) because the bullet itself is getting instantiated
        GameObject hole = Instantiate(GlobalReferences.Instance.bulletImpact,
                                    contact.point,
                                    Quaternion.LookRotation(contact.normal));

        hole.transform.SetParent(objectWeHit.gameObject.transform);
    }


}
