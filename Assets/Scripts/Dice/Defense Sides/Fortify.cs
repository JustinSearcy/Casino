using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fortify : MonoBehaviour
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

    public void Action()
    {
        FindObjectOfType<ActionManager>().AddDefenseToSelf(defense);
    }
}
