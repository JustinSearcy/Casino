using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    [SerializeField] public int baseStrength = 1;
    [SerializeField] public int baseMagic = 1;
    [SerializeField] public int basePhysDefense = 1;
    [SerializeField] public int baseMagDefense = 1;
    [SerializeField] public float baseCritChance = 0.1f;
    [SerializeField] public int strength = 1;
    [SerializeField] public int magic = 1;
    [SerializeField] public int physDefense = 1;
    [SerializeField] public int magDefense = 1;
    [SerializeField] public float critChance = 0.1f;

    private void Start()
    {
        strength = baseStrength;
        magic = baseMagic;
        physDefense = basePhysDefense;
        magDefense = baseMagDefense;
        critChance = baseCritChance;
    }
}
