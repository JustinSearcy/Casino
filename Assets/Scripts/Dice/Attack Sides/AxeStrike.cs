using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeAttack : MonoBehaviour, IDiceSide
{
    private ActionTargets actionTarget = ActionTargets.SINGLE_TARGET_ENEMY;
    private ActionNames actionName = ActionNames.AXE_STRIKE;

    [SerializeField] int damage = 8;

    Buffs buffs;
    DiceManager manager;

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
        get => "Deal " + GetUpdatedColor() + " Damage To A Single Enemy";
    }

    public string RolledDescription
    {
        get => "Deal " + GetUpdatedRolledColor() + " Damage To A Single Enemy";
    }

    public string GetUpdatedColor()
    {
        manager = manager == null ? FindObjectOfType<DiceManager>() : manager;
        return manager.GetDescriptionColor(damage, GetUpdatedVal());
    }

    public string GetUpdatedRolledColor()
    {
        manager = manager == null ? FindObjectOfType<DiceManager>() : manager;
        return manager.GetDescriptionColor(damage, GetUpdatedRolledVal());
    }

    public int GetUpdatedVal()
    {
        buffs = buffs == null ? GameObject.FindGameObjectWithTag("Player").GetComponent<Buffs>() : buffs;
        float buff = buffs.attackBuff;
        return (int)(damage * buff);
    }

    public int GetUpdatedRolledVal()
    {
        buffs = buffs == null ? GameObject.FindGameObjectWithTag("Player").GetComponent<Buffs>() : buffs;
        float buff = buffs.attackBuff;
        return (int)(damage * buff);
    }

    public void Action()
    {
        FindObjectOfType<ActionManager>().DealDamageToCurrentTarget(GetUpdatedRolledVal());
    }
}
