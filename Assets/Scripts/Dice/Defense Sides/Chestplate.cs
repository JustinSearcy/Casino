using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chestplate : MonoBehaviour, IDiceSide
{
    private ActionTargets actionTarget = ActionTargets.SELF;
    private ActionNames actionName = ActionNames.CHESTPLATE;

    [SerializeField] int defense = 3;

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
        get => "Gain " + defense + " Defense For Each Unique Armor Piece Rolled This Turn";
    }

    public void Action()
    {
        int totalDefense = 0;
        FindObjectOfType<ActionManager>().AddDefenseToSelf(totalDefense);
    }
}
