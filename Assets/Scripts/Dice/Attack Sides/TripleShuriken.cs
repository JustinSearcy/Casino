using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShuriken : MonoBehaviour, IDiceSide
{
    private ActionTargets actionTarget = ActionTargets.SINGLE_TARGET_ENEMY;
    private ActionNames actionName = ActionNames.TRIPLE_SHURIKEN;

    [SerializeField] int damage = 3;
    [SerializeField] int attackTimes = 3;
    [SerializeField] float timeBetweenAttacks = 0.2f;

    public ActionTargets ActionTarget
    {
        get => actionTarget;
    }

    public ActionNames ActionName
    {
        get => actionName;
    }

    public string Description
    {
        get => "Deal " + damage + " Damage To A Single Enemy " + attackTimes + " Times";
    }

    public void Action()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        ActionManager actionManager = FindObjectOfType<ActionManager>();
        CombatManager combatManager = FindObjectOfType<CombatManager>();
        for(int i = 0; i < attackTimes; i++)
        {
            if(combatManager.NoVictoryOrDefeat())
            {
                actionManager.DealDamageToCurrentTarget(damage);
                yield return new WaitForSeconds(timeBetweenAttacks);
            }
                
        }
    }
}
