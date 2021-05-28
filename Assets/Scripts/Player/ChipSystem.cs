using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChipSystem : MonoBehaviour
{
    [SerializeField] int startingChips = 100;
    [SerializeField] int currentChips = 100;
    [SerializeField] GameObject chipLossText = null;
    [SerializeField] Transform chipLossPos = null;
    [SerializeField] GameObject chipText = null;
    [SerializeField] float finalTextYPos = 2f;
    [SerializeField] float textTime = 1f;

    Shake shake;

    private void Start()
    {
        currentChips = startingChips;
        shake = FindObjectOfType<Shake>();
    }

    public int getChips() { return currentChips; } //Change to getter setter

    public void LoseChips(int amount)
    {
        chipLoss(amount);
        currentChips -= amount;
        FindObjectOfType<ChipCombatUI>().UpdateAndLoseChips(currentChips);
        if(currentChips <= 0)
        {
            GameOver();
        }
    }

    private void chipLoss(int amount)
    {
        shake.CamShake();
        GameObject newChipLossText = Instantiate(chipLossText, chipLossPos.position, Quaternion.identity);
        newChipLossText.GetComponent<TextMeshPro>().text = "-" + amount;
        LeanTween.moveLocalY(newChipLossText, finalTextYPos, textTime);
        Destroy(newChipLossText, textTime);
    }

    private void GameOver()
    {
        FindObjectOfType<CombatManager>().PlayerLost();
    }
}
