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
        ActionOne();
        return actionOneName;
        /*
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
        */
    }

    public void ActionOne() //Make a system for determining damage
    {
        this.gameObject.GetComponent<Animator>().SetTrigger("Roll");
    }

    public void ActionThree()
    {
        throw new System.NotImplementedException();
    }

    public void ActionTwo()
    {
        throw new System.NotImplementedException();
    }

    public void Roll()
    {
        FindObjectOfType<Shake>().CamShake();
        int playerDefense = chips.gameObject.GetComponent<UnitStats>().physDefense;
        int dieStrength = this.gameObject.GetComponent<UnitStats>().strength;
        int damage = 0;
        if(dieStrength <= playerDefense)
        {
            damage = dieStrength;
        }
        else
        {
            damage = dieStrength * ((dieStrength - playerDefense) + 1);
        }
        chips.LoseChips(damage);
    }
}
