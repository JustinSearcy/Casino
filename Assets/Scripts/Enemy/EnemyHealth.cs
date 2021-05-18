using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth = 100;
    //erializeField] TextMeshPro enemyHealthText = null;
    [SerializeField] float deathAnimTime = 1f;
    [SerializeField] GameObject healthBar = null;
    [SerializeField] float healthBarTime = 0.4f;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public float GetHealthPercent() { return ((float)currentHealth)/((float)maxHealth); }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Damage Dealt");
        currentHealth -= damage;
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            UpdateHealthBar();
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

    private void UpdateHealthBar()
    {
        //enemyHealthText.text = "" + currentHealth + "/" + maxHealth;
        LeanTween.scaleX(healthBar, GetHealthPercent(), healthBarTime).setEaseOutCirc();
    }
}
