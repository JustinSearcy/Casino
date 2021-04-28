using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyCombat : MonoBehaviour, IEnemyCombat
{
    EnemyHealth health;
    ChipSystem chips;

    private void Start()
    {
        health = this.gameObject.GetComponent<EnemyHealth>();
        chips = FindObjectOfType<ChipSystem>();
    }

    public void DetermineAction()
    {
        if (chips.getChips() < 20) //Do the attack action if it can kill player
        {
            ActionOne();
        }
        else if (health.getHealthPercent() < 0.25) //Do some kind of defensive action
        {
            ActionTwo();
        }
        else //Default Action
        {
            ActionOne();
        }
    }

    public void ActionOne()//Attack Action
    {
        chips.LoseChips(20);
        Debug.Log("Enemy Attacks");
    }

    public void ActionTwo()//Defensive Action
    {
        health.Heal(20);
        Debug.Log("Enemy Heals");
    }

    public void ActionThree()//Something Else to Add Later, special ability?
    {
        throw new System.NotImplementedException();
    }
}
