using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackJack : MonoBehaviour, IAction
{
    private string actionName = "BlackJack";

    public string ActionName
    {
        get { return actionName; }
    }

    public void StartAction(GameObject target)
    {
        UnitStats playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<UnitStats>();
        UnitStats targetStats = target.GetComponent<UnitStats>();
        int attackVal = 10;
        float critMultiplier = CheckCrit(playerStats);
        int damage = (int)((playerStats.strength - targetStats.physDefense) * attackVal * critMultiplier);
        target.GetComponent<EnemyHealth>().TakeDamage(damage);
        Debug.Log("Blackjack Attack");
    }

    private float CheckCrit(UnitStats playerStats)
    {
        float critIfGreater = UnityEngine.Random.Range(0f, 1f);
        Debug.Log(critIfGreater);
        Debug.Log(playerStats.critChance);
        if (playerStats.critChance > critIfGreater)
        {
            return 1.3f;
        }
        else
        {
            return 1f;
        }
    }
}
