using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fortify : MonoBehaviour, IDiceSide
{
    private ActionTargets actionTarget = ActionTargets.SELF;
    private ActionNames actionName = ActionNames.FORTIFY;

    [SerializeField] int defense = 8;

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
        get => "Gain " + defense + " Defense For This Turn ";
    }

    public string RolledDescription
    {
        get => "Gain " + defense + " Defense For This Turn ";
    }

    public void Action()
    {
        FindObjectOfType<ActionManager>().AddDefenseToSelf(defense);
    }
}
