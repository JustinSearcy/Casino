using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    [SerializeField] List<GameObject> currentDice = null;
    [SerializeField] List<GameObject> selectedDice = null;
    [SerializeField] List<Transform> selectPos = null;
    [SerializeField] List<Transform> rolledPos = null;
    [SerializeField] GameObject selectPosParent = null;
    [SerializeField] public GameObject selectedRolledDie = null;
    [SerializeField] Button rollButton = null;
    [SerializeField] int dicePickAmt = 4;
    [SerializeField] int diceSelectedAmt = 0;
    [SerializeField] float diceSelectTime = 0.2f;
    [SerializeField] float diceShiftTime = 0.1f;
    [SerializeField] float rolledMoveTime = 0.25f;
    [SerializeField] float waitToMoveTime = 0.3f;

    DiceLoader diceLoader;
    CombatManager combatManager;
    ActionManager actionManager;
    DiceSelectLine diceSelectLine;

    public bool diceLoaded = false;

    private int diceRolled = 0;
    private bool hasRolled = false;

    private void Start()
    {
        diceLoader = FindObjectOfType<DiceLoader>();
        combatManager = FindObjectOfType<CombatManager>();
        actionManager = FindObjectOfType<ActionManager>();
        diceSelectLine = FindObjectOfType<DiceSelectLine>();
        selectPosParent.SetActive(false);
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
        if (diceSelectedAmt < dicePickAmt && !hasRolled && diceLoaded)
        {
            selectedDice.Add(die);
            LeanTween.move(die, selectPos[diceSelectedAmt].position, diceSelectTime);
            die.GetComponent<Dice>().SetSelectIndex(diceSelectedAmt);
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
        rollButton.interactable = false;
        foreach (GameObject die in selectedDice)
        {
            die.GetComponent<Dice>().Roll();
        }
    }

    public void LoadNewDice()
    {
        diceLoader.LoadDice(currentDice);
    }

    public void UnloadDice()
    {
        diceLoaded = false;
        StartCoroutine(UnloadDiceCoroutine());
    }

    private IEnumerator UnloadDiceCoroutine()
    {
        foreach (GameObject die in selectedDice)
        {
            diceLoader.PlaceDieBackOnLoader(die);
        }
        yield return new WaitForSeconds(diceSelectTime + 0.1f);
        selectedDice.Clear();
        diceLoader.UnloadDice();
    }

    public void DiceLoaded()
    {
        diceLoaded = true;
        selectPosParent.SetActive(true);
    }

    public void DieRolled()
    {
        diceRolled++;
        if (diceRolled >= selectedDice.Count)
        {
            StartCoroutine(MoveRolledDice());
            diceRolled = 0;
        }
    }

    private IEnumerator MoveRolledDice()
    {
        yield return new WaitForSeconds(waitToMoveTime);
        selectPosParent.SetActive(false);
        for (int i = 0; i < selectedDice.Count; i++)
        {
            LeanTween.move(selectedDice[i], rolledPos[i], rolledMoveTime).setEaseOutQuad();
        }
        combatManager.ActionComplete(CombatManager.DICE_ROLLED);
    }

    public void SelectRolledDie(GameObject die)
    {
        //if (!actionInProgress)
        //{
            if (selectedRolledDie != null)
                selectedRolledDie.GetComponent<Dice>().DeselectDie();
            selectedRolledDie = die;
            diceSelectLine.setNewTarget(die.transform);
        //}
    }

    public void TryAction(string targetType, GameObject currentActionTarget)
    {
        if (selectedRolledDie != null)
        {
            IDiceSide side = selectedRolledDie.GetComponent<Dice>().currentSide.GetComponent<IDiceSide>();
            ActionTargets target = side.ActionTarget;

            if (targetType.Equals("Enemy"))
            {
                if (target == ActionTargets.SINGLE_TARGET_ENEMY)
                    Action(currentActionTarget, side);
            }
        }
    }

    public void Action(GameObject target, IDiceSide side)
    {
        diceLoader.PlaceDieBackOnLoader(selectedRolledDie);
        diceSelectLine.deactivateLine();

        selectedRolledDie.GetComponent<Dice>().DieActionComplete();
        selectedDice.Remove(selectedRolledDie);
        selectedRolledDie = null;

        combatManager.SetActionTarget(target);
        actionManager.ManageAction(side);
    }

    public void ActionFinished()
    {
        Debug.Log("ACTION FINISHED");
        if (selectedDice.Count == 0)
            combatManager.ActionComplete(CombatManager.ALL_PLAYER_ACTIONS_COMPLETE);
    }
}
