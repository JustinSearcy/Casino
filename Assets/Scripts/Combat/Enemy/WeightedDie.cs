using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedDie : MonoBehaviour, IEnemyCombat
{
    [SerializeField] float timeBeforeAttack = 0.5f;
    [SerializeField] float timeAfterAttack = 0.75f;
    [SerializeField] GameObject hotRollerParticles = null;
    [SerializeField] GameObject poisonParticles = null;
    [SerializeField] float poisonTime = 1f;
    [SerializeField] int poisonTurnLength = 3;
    [SerializeField] float hotRollerTime = 1.2f;
    [SerializeField] GameObject intent = null;

    Health health;
    Health playerHealth;
    CombatManager combatManager;
    EnemyIntent enemyIntent;
    Animator anim;

    private bool isHotRoller = false;
    private int currentAction;

    private void Start()
    {
        health = this.gameObject.GetComponent<Health>();
        playerHealth = playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        combatManager = FindObjectOfType<CombatManager>();
        enemyIntent = FindObjectOfType<EnemyIntent>();
        anim = gameObject.GetComponent<Animator>();
    }

    public void DetermineAction()
    {
        intent.SetActive(true);
        float num = Random.Range(0, 1f);
        if (num <= 0.5f) //50% chance to Perfect Roll
        {
            int damage = isHotRoller ? 12 : 6;
            enemyIntent.SetAttack(intent, damage);
            currentAction = 1;
        }
        else if (num <= 0.8) //30% chance to Snake Eyes
        {
            enemyIntent.SetSpecial(intent);
            currentAction = 2;
        }
        else //20% chance to Hot Roller
        { 
            enemyIntent.SetSpecial(intent);
            currentAction = 3;
        }
    }

    public void Action()
    {
        if (currentAction == 1)
            StartCoroutine(ActionOne());
        else if (currentAction == 2)
            StartCoroutine(ActionTwo());
        else
            StartCoroutine(ActionThree());
    }

    IEnumerator ActionOne()//Roll
    {
        yield return new WaitForSeconds(timeBeforeAttack);
        anim.SetTrigger("Roll");
    }

    IEnumerator ActionTwo()//SnakeEyes
    {
        yield return new WaitForSeconds(timeBeforeAttack);
        anim.SetTrigger("SnakeEyes");
    }

    IEnumerator ActionThree()//HotRoller
    {
        yield return new WaitForSeconds(timeBeforeAttack);
        StartCoroutine(HotRoller());
    }

    public void Roll()
    {
        FindObjectOfType<Shake>().CamShake();
        int damage = isHotRoller ? 12 : 6;
        playerHealth.TakeDamage(damage);
        StartCoroutine(NextCharacter());
    }

    public void SnakeEyes()
    {
        StartCoroutine(SnakeEyesAttack());
    }

    private IEnumerator SnakeEyesAttack()
    {
        GameObject particles = Instantiate(poisonParticles, playerHealth.gameObject.transform.position, Quaternion.identity);
        Destroy(particles, 2f);
        yield return new WaitForSeconds(poisonTime);
        playerHealth.gameObject.GetComponent<StatusEffects>().Poisoned(poisonTurnLength);
        StartCoroutine(NextCharacter());
    }

    private IEnumerator HotRoller()
    {
        //FindObjectOfType<CameraZoom>().ZoomTarget(this.gameObject.transform);
        hotRollerParticles.SetActive(true);
        isHotRoller = true;
        yield return new WaitForSeconds(hotRollerTime);
        //FindObjectOfType<CameraZoom>().ZoomCenter();
        StartCoroutine(NextCharacter());
    }

    IEnumerator NextCharacter()
    {
        yield return new WaitForSeconds(timeAfterAttack);
        combatManager.ActionComplete(CombatManager.ENEMY_ACTION_COMPLETE);
    }
}
