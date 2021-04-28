using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        currentChips -= amount;
        if(currentChips <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        FindObjectOfType<CombatManager>().PlayerLost();
    }
}
