using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth = 100;
    [SerializeField] TextMeshPro enemyHealthText = null;
    [SerializeField] float deathAnimTime = 1f;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    public float GetHealthPercent() { return ((float)currentHealth)/((float)maxHealth); }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthText();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Damage Dealt");
        currentHealth -= damage;
        UpdateHealthText();
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            UpdateHealthText();
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        //Add in death animation here
        yield return new WaitForSeconds(deathAnimTime);
        FindObjectOfType<CombatManager>().EnemyDeath(this.gameObject);
        Destroy(this.gameObject);
    }

    private void UpdateHealthText()
    {
        enemyHealthText.text = "" + currentHealth + "/" + maxHealth;
    }
}
