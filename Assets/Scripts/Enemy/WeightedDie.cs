using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedDie : MonoBehaviour, IEnemyCombat
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

    public void ActionOne()
    {
        throw new System.NotImplementedException();
    }

    public void ActionThree()
    {
        throw new System.NotImplementedException();
    }

    public void ActionTwo()
    {
        throw new System.NotImplementedException();
    }
}
