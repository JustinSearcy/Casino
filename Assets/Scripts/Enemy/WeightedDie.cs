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

    EnemyHealth health;
    ChipSystem chips;
    CombatManager combatManager;

    public string actionOneName = "Attack";
    public string actionTwoName = "Heal";
    public string actionThreeName = "Something";

    private void Start()
    {
        health = this.gameObject.GetComponent<EnemyHealth>();
        chips = FindObjectOfType<ChipSystem>();
        combatManager = FindObjectOfType<CombatManager>();
    }

    public string DetermineAction()
    {
        float num = Random.Range(0, 1f);
        if (num <= 0.5f) //50% chance to Perfect Roll
        {
            StartCoroutine(ActionOne());
            return actionOneName;
        }
        else if (num <= 0.8) //30% chance to Snake Eyes
        {
            StartCoroutine(ActionTwo());
            return actionTwoName;
        }
        else //20% chance to Hot Roller
        {
            StartCoroutine(ActionThree());
            return actionThreeName;
        }
    }

    IEnumerator ActionOne()
    {
        yield return new WaitForSeconds(timeBeforeAttack);
        this.gameObject.GetComponent<Animator>().SetTrigger("Roll");
    }

    IEnumerator ActionTwo()
    {
        yield return new WaitForSeconds(timeBeforeAttack);
        this.gameObject.GetComponent<Animator>().SetTrigger("SnakeEyes");
    }

    IEnumerator ActionThree()
    {
        yield return new WaitForSeconds(timeBeforeAttack);
        HotRoller();
    }

    public void Roll()
    {
        FindObjectOfType<Shake>().CamShake();
        int playerDefense = chips.gameObject.GetComponent<UnitStats>().physDefense;
        int dieStrength = this.gameObject.GetComponent<UnitStats>().strength;
        int damage = 0;
        if(dieStrength <= playerDefense)
        {
            damage = 6;
        }
        else
        {
            damage = 6 * (dieStrength - playerDefense);
        }
        chips.LoseChips(damage);
        StartCoroutine(NextCharacter());
    }

    public void SnakeEyes()
    {
        StartCoroutine(SnakeEyesAttack());
    }

    IEnumerator SnakeEyesAttack()
    {
        GameObject particles = Instantiate(poisonParticles, chips.gameObject.transform.position, Quaternion.identity);
        Destroy(particles, 2f);
        yield return new WaitForSeconds(poisonTime);
        FindObjectOfType<CombatManager>().CombatTextMessage("You've been poisoned!");
        chips.gameObject.GetComponent<StatusEffects>().Poisoned(poisonTurnLength);
        StartCoroutine(NextCharacter());
    }

    public void HotRoller()
    {
        hotRollerParticles.SetActive(true);
        this.gameObject.GetComponent<UnitStats>().modifyStrength(2f);
        StartCoroutine(NextCharacter());
    }

    IEnumerator NextCharacter()
    {
        yield return new WaitForSeconds(timeAfterAttack);
        combatManager.AttackComplete();
    }
}
