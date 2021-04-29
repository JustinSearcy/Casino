using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    [SerializeField] int baseStrength = 1;
    [SerializeField] int baseMagic = 1;
    [SerializeField] int basePhysDefense = 1;
    [SerializeField] int baseMagDefense = 1;
    [SerializeField] float baseCritChance = 0.1f;
    [SerializeField] int strength = 1;
    [SerializeField] int magic = 1;
    [SerializeField] int physDefense = 1;
    [SerializeField] int magDefense = 1;
    [SerializeField] float critChance = 0.1f;

    private void Start()
    {
        strength = baseStrength;
        magic = baseMagic;
        physDefense = basePhysDefense;
        magDefense = baseMagDefense;
        critChance = baseCritChance;
    }
}
