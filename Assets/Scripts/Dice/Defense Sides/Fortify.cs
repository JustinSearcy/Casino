using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fortify : MonoBehaviour, IDiceSide
{
    private ActionTargets actionTarget = ActionTargets.SELF;
    private ActionNames actionName = ActionNames.FORTIFY;

    [SerializeField] int defense = 8;

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
        get => "Gain " + GetUpdatedColor() + " Defense For This Turn ";
    }

    public string RolledDescription
    {
        get => "Gain " + GetUpdatedRolledColor() + " Defense For This Turn ";
    }

    public string GetUpdatedColor()
    {
        manager = manager == null ? FindObjectOfType<DiceManager>() : manager;
        return manager.GetDescriptionColor(defense, GetUpdatedVal());
    }

    public string GetUpdatedRolledColor()
    {
        manager = manager == null ? FindObjectOfType<DiceManager>() : manager;
        return manager.GetDescriptionColor(defense, GetUpdatedRolledVal());
    }

    public int GetUpdatedVal()
    {
        buffs = buffs == null ? GameObject.FindGameObjectWithTag("Player").GetComponent<Buffs>() : buffs;
        float buff = buffs.defenseBuff;
        return (int)(defense * buff);
    }

    public int GetUpdatedRolledVal()
    {
        buffs = buffs == null ? GameObject.FindGameObjectWithTag("Player").GetComponent<Buffs>() : buffs;
        float buff = buffs.defenseBuff;
        return (int)(defense * buff);
    }

    public void Action()
    {
        FindObjectOfType<ActionManager>().AddDefenseToSelf(GetUpdatedRolledVal());
    }
}
