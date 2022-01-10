using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour, IDiceSide
{
    private ActionTargets actionTarget = ActionTargets.SINGLE_TARGET_ENEMY;
    private ActionNames actionName = ActionNames.BASIC_ATTACK;

    [SerializeField] int damage = 5;
    

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
