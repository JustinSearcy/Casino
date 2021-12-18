using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChipSystem : MonoBehaviour
{
    [SerializeField] int startingChips = 100;
    [SerializeField] int currentChips = 100;

    private void Start()
    {
        currentChips = startingChips;
    }

    public int getChips() { return currentChips; } //Change to getter setter

    public void LoseChips(int amount)
    {
        FindObjectOfType<CombatManager>().LoseChips(amount);
        currentChips -= amount;
    }
}
