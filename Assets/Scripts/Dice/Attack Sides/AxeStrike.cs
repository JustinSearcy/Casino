using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeAttack : MonoBehaviour, IDiceSide
{
    private ActionTargets actionTarget = ActionTargets.SINGLE_TARGET_ENEMY;
    private ActionNames actionName = ActionNames.AXE_STRIKE;

    [SerializeField] int damage = 8;

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
        get => "Deal " + damage + " Damage To A Single Enemy";
    }

    public void Action()
    {
        FindObjectOfType<ActionManager>().DealDamageToCurrentTarget(damage);
    }
}
