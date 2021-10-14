using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    [SerializeField] List<GameObject> currentDice = null;
    [SerializeField] List<GameObject> selectedDice = null;
    [SerializeField] List<Transform> selectPos = null;
    [SerializeField] Button rollButton = null;
    [SerializeField] int dicePickAmt = 4;
    [SerializeField] int diceSelectedAmt = 0;
    [SerializeField] bool hasRolled = false;
    [SerializeField] float diceSelectTime = 0.2f;
    [SerializeField] float diceShiftTime = 0.1f;

    DiceLoader diceLoader;

    private void Start()
    {
        diceLoader = FindObjectOfType<DiceLoader>();
    }

    public void AddDie(GameObject die)
    {
        currentDice.Add(die);
    }

    public void RemoveDie(GameObject die)
    {
        currentDice.Remove(die);
    }

    public bool SelectDie(GameObject die)
    {
        if (diceSelectedAmt < dicePickAmt && !hasRolled)
        {
            selectedDice.Add(die);
            LeanTween.move(die, selectPos[diceSelectedAmt].position, diceSelectTime);
            die.GetComponent<Dice>().setSelectIndex(diceSelectedAmt);
            diceSelectedAmt++;
            if (diceSelectedAmt == dicePickAmt)
            {
                rollButton.interactable = true;
            }
            return true;
        }

        return false;
    }

    public bool DeselectDie(GameObject die)
    {
        if (!hasRolled)
        {
            selectedDice.Remove(die);
            diceSelectedAmt--;
            rollButton.interactable = false;
            diceLoader.PlaceDieBackOnLoader(die);
            ShiftSelectedDie(die.GetComponent<Dice>().selectIndex);
            return true;
        }
        return false;
    }

    private void ShiftSelectedDie(int removedIndex)
    {
        int diceToMove = diceSelectedAmt - removedIndex;
        int count = 0;
        while(diceToMove > 0)
        {
            GameObject dieToMove = selectedDice[removedIndex + count];
            dieToMove.GetComponent<Dice>().selectIndex--;
            LeanTween.move(dieToMove, selectPos[removedIndex + count].position, diceShiftTime);

            count++;
            diceToMove--;
        }
    }


    public void RollDice()
    {
        hasRolled = true;
        foreach (GameObject die in selectedDice)
        {
            die.GetComponent<Dice>().Roll();
        }
    }

    public void LoadNewDice()
    {
        Debug.Log("Dice Loaded");
        diceLoader.LoadDice(currentDice);
    }

    public void UnloadDice()
    {
        StartCoroutine(UnloadDiceCoroutine());
    }

    IEnumerator UnloadDiceCoroutine()
    {
        foreach (GameObject die in selectedDice)
        {
            diceLoader.PlaceDieBackOnLoader(die);
        }
        yield return new WaitForSeconds(diceSelectTime + 0.1f);
        diceLoader.UnloadDice();
    }
}
