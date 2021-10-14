using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] GameObject[] sides = null;
    [SerializeField] bool isSelected = false;
    [SerializeField] float rollIntervalTime = 0.03f;
    [SerializeField] int maxRolls = 30;
    [SerializeField] int minRolls = 20;

    DiceManager diceManager;
    SpriteRenderer sprite;

    public int loadIndex;
    public int selectIndex;

    private void Start()
    {
        diceManager = FindObjectOfType<DiceManager>();
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
        sprite.sprite = sides[Random.Range(0, sides.Length)].GetComponent<SpriteRenderer>().sprite;
    }

    public void Roll()
    {
        StartCoroutine(RollAnim());
    }

    public IEnumerator RollAnim()
    {
        int oldSpriteIndex = 0;
        int rollAmt = Random.Range(minRolls, maxRolls);
        float slowIncrement = 1 / rollAmt;
        float rollTime = rollAmt * rollIntervalTime;
        for (int i = 0; i < rollAmt; i++)
        {
            yield return new WaitForSeconds(rollIntervalTime);
            sprite.sprite = sides[GetNewSpriteIndex(oldSpriteIndex)].GetComponent<SpriteRenderer>().sprite;
        }
    }

    private int GetNewSpriteIndex(int oldSpriteIndex)
    {
        int newSpriteIndex = oldSpriteIndex;
        while (newSpriteIndex == oldSpriteIndex)
        {
            newSpriteIndex = Random.Range(0, sides.Length);
        }
        return newSpriteIndex;
    }

    private void OnMouseDown()
    {
        if (!isSelected)
        {
            if (diceManager.SelectDie(gameObject))
            {
                isSelected = true;
            }
        }
        else
        {
            if (diceManager.DeselectDie(gameObject))
            {
                isSelected = false;
            }
        }
    }

    public void DieRolled() => isSelected = false;

    public void setLoadIndex(int index) => loadIndex = index;

    public void setSelectIndex(int index) => selectIndex = index;
}
