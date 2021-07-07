using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyCombat : MonoBehaviour, IEnemyCombat
{
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
        if (chips.getChips() < 20) //Do the attack action if it can kill player
        {
            ActionOne();
            return actionOneName;
        }
        else if (health.GetHealthPercent() < 0.25) //Do some kind of defensive action
        {
            ActionTwo();
            return actionTwoName;
        }
        else //Default Action
        {
            ActionOne();
            return actionOneName;
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
