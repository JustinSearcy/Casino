using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackJack : MonoBehaviour, IAction
{
    public void StartAction(GameObject target)
    {
        int damage = 50;
        target.GetComponent<EnemyHealth>().TakeDamage(damage);
        Debug.Log("Blackjack Attack");
    }
}
