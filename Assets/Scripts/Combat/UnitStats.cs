using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    [Header("BASE STATS")]
    [SerializeField] public int baseStrength = 1;
    [SerializeField] public int baseMagic = 1;
    [SerializeField] public int basePhysDefense = 1;
    [SerializeField] public int baseMagDefense = 1;
    [SerializeField] public float baseCritChance = 0.1f;

    [Header("CURRENT STATS")]
    [SerializeField] public int strength = 1;
    [SerializeField] public int magic = 1;
    [SerializeField] public int physDefense = 1;
    [SerializeField] public int magDefense = 1;
    [SerializeField] public float critChance = 0.1f;

    [Header("MISC")]
    [SerializeField] public int chipValue = 15;
    [SerializeField] public string unitName = "";

    private void Start()
    {
        strength = baseStrength;
        magic = baseMagic;
        physDefense = basePhysDefense;
        magDefense = baseMagDefense;
        critChance = baseCritChance;
    }

    public void modifyStrength(float multiplier)
    {
        strength = (int)(strength * multiplier);
    }

    public void modifyMagic(float multiplier)
    {
        magic = (int)(magic * multiplier);
    }

    public void modifyPhysDefense(float multiplier)
    {
        physDefense = (int)(physDefense * multiplier);
    }

    public void modifyMagDefense(float multiplier)
    {
        magDefense = (int)(magDefense * multiplier);
    }
}
