using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] GameObject[] sides = null;
    [SerializeField] public GameObject currentSide = null;
    [SerializeField] bool isSelected = false;
    [SerializeField] float rollIntervalTime = 0.03f;
    [SerializeField] int maxRolls = 30;
    [SerializeField] int minRolls = 20;
    [SerializeField] private bool wasRolled = false;
    [SerializeField] float outlineAlpha = 0.575f;

    DiceManager diceManager;
    SpriteRenderer sprite;
    Material mat;

    public int loadIndex;
    public int selectIndex;

    private GameObject dieBackgroundSelect;

    private void Start()
    {
        diceManager = FindObjectOfType<DiceManager>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        mat = gameObject.GetComponent<Renderer>().material;
        mat.SetFloat("_OutlineAlpha", 0);
        currentSide = sides[0];
        sprite.sprite = currentSide.GetComponent<SpriteRenderer>().sprite;
    }

    public void Roll()
    {
        StartCoroutine(RollAnim());
    }

    public IEnumerator RollAnim()
    {
        int oldSideIndex = 0;
        int rollAmt = Random.Range(minRolls, maxRolls);
        float slowIncrement = 1 / rollAmt;
        float rollTime = rollAmt * rollIntervalTime;
        for (int i = 0; i < rollAmt; i++)
        {
            yield return new WaitForSeconds(rollIntervalTime);
            currentSide = sides[GetNewSideIndex(oldSideIndex)];
            sprite.sprite = currentSide.GetComponent<SpriteRenderer>().sprite;
        }
        diceManager.DieRolled();
        isSelected = false;
        wasRolled = true;
    }

    private int GetNewSideIndex(int oldSideIndex)
    {
        int newSideIndex = oldSideIndex;
        while (newSideIndex == oldSideIndex)
        {
            newSideIndex = Random.Range(0, sides.Length);
        }
        return newSideIndex;
    }

    private void OnMouseDown()
    {
        if (!isSelected)
        {
            if (wasRolled)
            {
                diceManager.SelectRolledDie(gameObject);
                isSelected = true;
                mat.SetFloat("_OutlineAlpha", outlineAlpha);
            }
            else if (diceManager.SelectDie(gameObject))
                isSelected = true;
        }
        else
            if (diceManager.DeselectDie(gameObject))
                DeselectDie();
    }

    public void SetLoadIndex(int index) => loadIndex = index;

    public void SetSelectIndex(int index) => selectIndex = index;

    public void DeselectDie()
    {
        isSelected = false;
        mat.SetFloat("_OutlineAlpha", 0);
    }

    public void DieActionComplete()
    {
        DeselectDie();
        wasRolled = false;
    }
}
