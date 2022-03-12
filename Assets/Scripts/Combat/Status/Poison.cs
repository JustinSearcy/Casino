using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : Status
{
    [SerializeField] public GameObject poisonIcon = null;
    [SerializeField] int poisonDamage = 5;

    public override void TakeEffect(GameObject target)
    {
        target.GetComponent<Health>().TakeDamage(poisonDamage);
    }

    public override GameObject GetIcon()
    {
        return poisonIcon;
    }
}
