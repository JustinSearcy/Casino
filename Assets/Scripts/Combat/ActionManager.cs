using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    CombatManager combatManager;

    void Start()
    {
        combatManager = FindObjectOfType<CombatManager>();
    }

    public void DealDamageToCurrentTarget(int damage)
    {
        combatManager.currentActionTarget.GetComponent<EnemyHealth>().TakeDamage(damage);
    }

}
