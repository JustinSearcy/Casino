using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiceToolTip : MonoBehaviour
{
    [SerializeField] Vector2 tooltipItemSize = new Vector2(400f, 75f);
    [SerializeField] Vector3 offset = new Vector3(50, 50, 0);
    [SerializeField] Vector3 rolledOffset = new Vector3(0, -150f, 0);
    [SerializeField] List<GameObject> tooltipItems;
    [SerializeField] GameObject rolledTooltip = null;
    [SerializeField] RectTransform canvasRectTransform;

    private RectTransform backgroundRectTransform;
    private RectTransform rectTransform;

    private bool rolledToolTip = false;

    void Awake()
    {
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        rectTransform = transform.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(!rolledToolTip)
            rectTransform.anchoredPosition = Input.mousePosition + offset;
        else
            rectTransform.anchoredPosition = Input.mousePosition + offset + rolledOffset;
    }

    public void UpdateTooltip(Dictionary<GameObject, int> uniqueSides)
    {
        rolledToolTip = false;
        int uniqueSidesCount = uniqueSides.Count;
        int counter = 0;
        Vector2 size = new Vector2(400f, 75f * uniqueSidesCount);
        backgroundRectTransform.sizeDelta = size;
        rolledTooltip.SetActive(false);
        for (int i = 0; i < 6; i++)
        {
            tooltipItems[i].SetActive(i < uniqueSidesCount);
        }
        foreach (GameObject key in uniqueSides.Keys)
        {
            tooltipItems[counter].GetComponent<DiceToolTipItem>().UpdateItem(key, uniqueSides[key]);
            counter++;
        }
    }

    public void UpdateRolledTooltip(GameObject die)
    {
        rolledToolTip = true;
        backgroundRectTransform.sizeDelta = tooltipItemSize;
        rolledTooltip.SetActive(true);
        for(int i = 0; i < 6; i++)
        {
            tooltipItems[i].SetActive(false);
        }
        rolledTooltip.GetComponent<TextMeshProUGUI>().text = die.GetComponent<Dice>().currentSide.GetComponent<IDiceSide>().Description;
    }
}
