using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth = 100;
    public GameObject deletePlayer;
    bool isDead;
    void Start()
    {
        
        currentHealth = maxHealth;
        Debug.Log("Start Health" + currentHealth);

    }

    // Method to reduce player's health
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Check if health is zero or less
        if (currentHealth <= 0)
        {
            isDead = true;

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            TakeDamage(10);
        }
    }

}
