using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    public string ActionName { get; }

    public void StartAction(GameObject target);
}
