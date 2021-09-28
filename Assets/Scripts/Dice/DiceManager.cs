using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    [SerializeField] List<GameObject> dice = null;
    [SerializeField] Button rollButton = null;
    [SerializeField] int dicePickAmt = 4;
    [SerializeField] int diceSelected = 0;

    [SerializeField] bool hasRolled = false;

    public bool SelectDie(GameObject die)
    {
        if (diceSelected < dicePickAmt && !hasRolled)
        {
            dice.Add(die);
            diceSelected++;
            if (diceSelected == dicePickAmt)
            {
                rollButton.interactable = true;
            }
            return true;
        }

        return false;
    }

    public void DeselectDie(GameObject die)
    {
        dice.Remove(die);
        diceSelected--;
        rollButton.interactable = false;
    }


    public void RollDice()
    {
        hasRolled = true;
        foreach (GameObject die in dice)
        {
            die.GetComponent<Dice>().Roll();
        }
    }
}
