using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [SerializeField] bool actionStarted = true;

    CombatManager combatManager;
    DiceManager diceManager;
    Health playerHealth;

    void Start()
    {
        combatManager = FindObjectOfType<CombatManager>();
        diceManager = FindObjectOfType<DiceManager>();
    }

    public void ManageAction(IDiceSide side)
    {
        if (playerHealth == null)
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        actionStarted = true;
        side.Action();
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
        combatManager.currentActionTarget.GetComponent<Health>().TakeDamage(damage);
    }

    public void AddDefenseToSelf(int defense)
    {
        playerHealth.AddDefense(defense);
    }
}
