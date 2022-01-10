using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDefense : MonoBehaviour, IDiceSide
{
    private ActionTargets actionTarget = ActionTargets.SELF;
    private ActionNames actionName = ActionNames.BASIC_DEFENSE;

    [SerializeField] int defense = 5;

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

    public void Action()
    {
        FindObjectOfType<ActionManager>().AddDefenseToSelf(defense);
    }
}
