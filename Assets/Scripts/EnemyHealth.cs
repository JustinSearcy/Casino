using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;

    int currentHealth = 100;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void takeDamage(int damage)
    {
        Debug.Log("Damage Dealt");
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<CombatManager>().EnemyDeath(this.gameObject);
        Destroy(this.gameObject);
    }
}
