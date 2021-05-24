using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedDie : MonoBehaviour, IEnemyCombat
{
    [SerializeField] GameObject hotRollerParticles = null;
    [SerializeField] GameObject poisonParticles = null;

    EnemyHealth health;
    ChipSystem chips;

    public string actionOneName = "Attack";
    public string actionTwoName = "Heal";
    public string actionThreeName = "Something";

    private void Start()
    {
        health = this.gameObject.GetComponent<EnemyHealth>();
        chips = FindObjectOfType<ChipSystem>();
    }

    public string DetermineAction()
    {
        float num = Random.Range(0, 1f);
        if (num <= 0.5f) //50% chance to Perfect Roll
        {
            ActionOne();
            return actionOneName;
        }
        else if (num <= 0.8) //30% chance to Snake Eyes
        {
            ActionTwo();
            return actionTwoName;
        }
        else //20% chance to Hot Roller
        {
            ActionThree();
            return actionThreeName;
        }
    }

    public void ActionOne()
    {
        this.gameObject.GetComponent<Animator>().SetTrigger("Roll");
    }

    public void ActionTwo()
    {
        this.gameObject.GetComponent<Animator>().SetTrigger("SnakeEyes");
    }

    public void ActionThree()
    {
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
    }

    public void SnakeEyes()
    {
        GameObject particles = Instantiate(poisonParticles, chips.gameObject.transform.position, Quaternion.identity);
        Destroy(particles, 2f);
        Debug.Log("Snake Eyes");
    }

    public void HotRoller()
    {
        hotRollerParticles.SetActive(true);
        this.gameObject.GetComponent<UnitStats>().modifyStrength(2f);
        Debug.Log("Hot Roller");
    }
}
