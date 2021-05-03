using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackjack : MonoBehaviour, IAction
{
    private string actionName = "Blackjack";

    [SerializeField] float timeBetweenDraws = 0.75f;

    BasicDeck deck;

    public string ActionName
    {
        get { return actionName; }
    }

    public void StartAction(GameObject target)
    {
        deck = FindObjectOfType<BasicDeck>();
        StartCoroutine(FirstCardDraws());
        //CalculateDamage(target);
    }

    IEnumerator FirstCardDraws()
    {
        deck.Shuffle();
        deck.DrawCard();
        yield return new WaitForSeconds(timeBetweenDraws);
        deck.DrawCard();
    }

    private void CalculateDamage(GameObject target)
    {
        UnitStats playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<UnitStats>();
        UnitStats targetStats = target.GetComponent<UnitStats>();
        int attackVal = 10;
        float critMultiplier = CheckCrit(playerStats);
        int damage = (int)((playerStats.strength - targetStats.physDefense) * attackVal * critMultiplier);
        target.GetComponent<EnemyHealth>().TakeDamage(damage);
        Debug.Log(actionName);
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
