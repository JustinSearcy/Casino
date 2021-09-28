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
    [SerializeField] float outlineAlpha = 0.75f;
    [SerializeField] float rollForce = 2.5f;
    [SerializeField] float spinForce = 5f;

    DiceManager diceManager;
    GameObject outline;
    Rigidbody2D rb;
    SpriteRenderer sprite;

    private void Start()
    {
        diceManager = FindObjectOfType<DiceManager>();
        outline = gameObject.transform.GetChild(0).gameObject;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
    }

    public void Roll()
    {
        StartCoroutine(RollAnim());
    }

    public IEnumerator RollAnim()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        int oldSpriteIndex = 0;
        int rollAmt = Random.Range(minRolls, maxRolls);
        float slowIncrement = 1 / rollAmt;
        float rollTime = rollAmt * rollIntervalTime;
        MoveDie(rollTime);
        for (int i = 0; i < rollAmt; i++)
        {
            yield return new WaitForSeconds(rollIntervalTime);
            sprite.sprite = sides[GetNewSpriteIndex(oldSpriteIndex)].GetComponent<SpriteRenderer>().sprite;
            rb.velocity *= 1 - Mathf.Sqrt(1 - Mathf.Pow(slowIncrement - 1, 2)); //Mathf.Pow(1 - slowIncrement, 3);
        }

        rb.bodyType = RigidbodyType2D.Static;
    }

    private void MoveDie(float rollTime)
    {
        Vector2 randDir = new Vector2(Random.Range(-1f, 1f) * rollForce, Random.Range(-1f, 1f) * rollForce);
        rb.velocity = randDir;
        rb.rotation = Random.Range(0.5f, 1f) * spinForce;
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
        Color tmp = outline.GetComponent<SpriteRenderer>().color;
        if (!isSelected)
        {
            if (diceManager.SelectDie(gameObject))
            {
                isSelected = true;
                tmp.a = outlineAlpha;
                outline.GetComponent<SpriteRenderer>().color = tmp;
            }
        }
        else
        {
            diceManager.DeselectDie(gameObject);
            isSelected = false;
            tmp.a = 0;
            outline.GetComponent<SpriteRenderer>().color = tmp;
        }
    }

    public void DieRolled()
    {
        isSelected = false;
        Color tmp = outline.GetComponent<SpriteRenderer>().color;
        tmp.a = 0;
        outline.GetComponent<SpriteRenderer>().color = tmp;
    }
}
