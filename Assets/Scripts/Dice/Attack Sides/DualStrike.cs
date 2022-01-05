using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualStrike : MonoBehaviour
{
    private ActionTargets actionTarget = ActionTargets.SINGLE_TARGET_ENEMY;
    private ActionNames actionName = ActionNames.DUAL_STRIKE;

    [SerializeField] int damage = 4;
    [SerializeField] int attackTimes = 2;
    [SerializeField] float timeBetweenAttacks = 0.3f;

    public ActionTargets ActionTarget
    {
        get => actionTarget;
    }

    public ActionNames ActionName
    {
        get => actionName;
    }

    public void Action()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        ActionManager actionManager = FindObjectOfType<ActionManager>();
        CombatManager combatManager = FindObjectOfType<CombatManager>();
        for (int i = 0; i < attackTimes; i++)
        {
            if (combatManager.NoVictoryOrDefeat())
            {
                actionManager.DealDamageToCurrentTarget(damage);
                yield return new WaitForSeconds(timeBetweenAttacks);
            }

        }
    }
}
