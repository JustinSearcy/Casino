using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSystem : MonoBehaviour
{
    [SerializeField] int startingChips = 100;
    [SerializeField] int currentChips = 100;
    [SerializeField] GameObject chipLossText = null;
    [SerializeField] GameObject chipText = null;
    [SerializeField] float finalTextYPos = 2f;
    [SerializeField] float textTime = 1f;

    private void Start()
    {
        currentChips = startingChips;
    }

    public int getChips() { return currentChips; } //Change to getter setter

    public void LoseChips(int amount)
    {
        chipLoss();
        currentChips -= amount;
        FindObjectOfType<ChipCombatUI>().UpdateAndLoseChips(currentChips);
        if(currentChips <= 0)
        {
            GameOver();
        }
    }

    private void chipLoss()
    {
        chipLossText.SetActive(true);
        LeanTween.moveLocalY(chipLossText, finalTextYPos, textTime);
        StartCoroutine(resetChipLoss());
    }

    IEnumerator resetChipLoss()
    {
        yield return new WaitForSeconds(textTime);
        chipLossText.transform.position = chipText.transform.position;
        chipLossText.SetActive(false);
    }

    private void GameOver()
    {
        FindObjectOfType<CombatManager>().PlayerLost();
    }
}
