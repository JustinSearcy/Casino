using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceGroups : MonoBehaviour
{
    public const string ARMORGROUP = "ARMOR_GROUP";
    List<ActionNames> armorGroup = new List<ActionNames>{ ActionNames.CHESTPLATE, ActionNames.GAUNTLET, ActionNames.GREAVES, ActionNames.HELMET };


    DiceManager diceManager;

    private void Start()
    {
        diceManager = FindObjectOfType<DiceManager>();
    }

    public int uniqueRolledInGroup(string group)
    {
        HashSet<ActionNames> diceSet = new HashSet<ActionNames>();
        List<GameObject> rolledDice = diceManager.selectedDiceCopy;
        List<ActionNames> diceGroup = getActionGroup(group);
        foreach (GameObject die in rolledDice)
        {
            ActionNames name = die.GetComponent<Dice>().currentSide.GetComponent<IDiceSide>().ActionName;
            if (diceGroup.Contains(name))
                diceSet.Add(name);
        }
        return diceSet.Count;
    }

    private List<ActionNames> getActionGroup(string groupName)
    {
        switch (groupName)
        {
            case ARMORGROUP:
                return armorGroup;
            default:
                Debug.Log("Oopsie, no group found");
                return null;
        }
    }
}
