using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;

public class EnemyHealth : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public int maxHealth = 100;
    [SerializeField] public int currentHealth = 100;
    [SerializeField] TextMeshPro enemyHealthText = null;

    [Header("Health Bar")]
    [SerializeField] GameObject healthBar = null;
    [SerializeField] float healthBarTime = 0.4f;

    [Header("Animations")]
    [SerializeField] GameObject enemyDeathParticles = null;
    [SerializeField] GameObject damagePopup = null;
    [SerializeField] float hitStunTime = 0.2f;
    [SerializeField] int rotationFinal = -1080;
    [SerializeField] float rotationTime = 1.3f;
    //[SerializeField] float deathAnimTime = 1f;

    Shake camShake;
    Animator anim;
    CombatManager combatManager;
    ActionManager actionManager;

    void Start()
    {
        combatManager = FindObjectOfType<CombatManager>();
        actionManager = FindObjectOfType<ActionManager>();
        anim = this.gameObject.GetComponent<Animator>();
        camShake = FindObjectOfType<Shake>();
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
        camShake.CamShake();
        anim.SetTrigger("Hit");
        GameObject text = Instantiate(damagePopup, this.gameObject.transform);
        text.GetComponent<DamagePopup>().UpdateText(damage);
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            UpdateHealthBar();
            StartCoroutine(Die());
        }
        else
        {
            StartCoroutine(HitStun());
        }
    }

    IEnumerator HitStun()
    {
        yield return new WaitForSeconds(hitStunTime);
        anim.SetTrigger("StopHit");
        actionManager.ActionFinished();
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(healthBarTime + 0.15f);
        LeanTween.rotateAround(this.gameObject, new Vector3(0, 1, 0), rotationFinal, rotationTime).setEaseOutExpo();
        yield return new WaitForSeconds(rotationTime);
        GameObject particles = Instantiate(enemyDeathParticles, this.gameObject.transform.position, Quaternion.identity);
        Destroy(particles, 2f);
        combatManager.EnemyDeath(this.gameObject);
        //DisableEnemy();
        //yield return new WaitForSeconds(2f); //Give time for combat manager to use this reference before destroying
        if (combatManager.combatState != CombatState.WIN)
            actionManager.ActionFinished();
        Destroy(this.gameObject);
    }

    private void DisableEnemy()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<ShadowCaster2D>().enabled = false;
    }

    private void UpdateHealthBar()
    {
        enemyHealthText.text = "" + currentHealth + "/" + maxHealth;
        LeanTween.scaleX(healthBar, GetHealthPercent(), healthBarTime).setEaseOutQuint();
    }
}
