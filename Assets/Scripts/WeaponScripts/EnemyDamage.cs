using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    float health,maxHealth = 5.0f;

    [Tooltip("Accessing the Health Bar")][SerializeField]FloatingHealthBar healthBar;
    
   

    void Awake()
    {
        health = maxHealth;
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        if(healthBar != null)
        {
            healthBar.UpdateHealthBar(health,maxHealth); 
        }
        else
        {
            Debug.Log("Health Bar not found");
        }
    }
    public void TakeDamage()
    {
        if(healthBar)
        {
            health-=1;
            Debug.Log("Health = " + health);
            healthBar.UpdateHealthBar(health,maxHealth);
            if(health <= 0)
            {
                Debug.Log("Enemy killed");
                this.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("Health Bar not found");
        }
       

    }
}
