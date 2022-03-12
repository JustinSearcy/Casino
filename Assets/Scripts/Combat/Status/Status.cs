using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  Status : MonoBehaviour
{
    [SerializeField] protected int initCounter = 3;

    public abstract void TakeEffect(GameObject target);

    public abstract GameObject GetIcon();

    public virtual int InitCounter()
    {
        return initCounter;
    }
}
