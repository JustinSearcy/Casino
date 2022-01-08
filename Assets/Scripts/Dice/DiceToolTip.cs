using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceToolTip : MonoBehaviour
{
    [SerializeField] GameObject diceTooltipItem = null;

    private RectTransform backgroundRectTransform;

    void Awake()
    {
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
    }

    public void UpdateTooltip(Dictionary<GameObject, int> uniqueSides)
    {
        foreach (GameObject key in uniqueSides.Keys)
        {
            GameObject item = Instantiate(diceTooltipItem, this.gameObject.transform);
            UpdateTooltipItem(item);
        }
    }

    private void UpdateTooltipItem(GameObject item)
    {

    }
}
