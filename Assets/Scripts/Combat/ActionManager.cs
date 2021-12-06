using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [SerializeField] bool actionStarted = true;

    CombatManager combatManager;
    DiceManager diceManager;

    void Start()
    {
        combatManager = FindObjectOfType<CombatManager>();
        diceManager = FindObjectOfType<DiceManager>();
    }

    public void ManageAction(IDiceSide side)
    {
        side.Action();
        actionStarted = true;
    }

    public void ActionFinished()
    {
        if (actionStarted)
        {
            actionStarted = false;
            diceManager.ActionFinished();
        }
    }

    public void DealDamageToCurrentTarget(int damage)
    {
        combatManager.currentActionTarget.GetComponent<EnemyHealth>().TakeDamage(damage);
    }

}
