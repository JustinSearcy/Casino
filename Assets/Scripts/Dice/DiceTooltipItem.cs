using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceToolTipItem : MonoBehaviour
{
    [SerializeField] GameObject diceSideSprite = null;
    [SerializeField] GameObject amountText = null;
    [SerializeField] GameObject descriptionText = null;

    Image image;
    TextMeshProUGUI amount;
    TextMeshProUGUI description;

    private void Awake()
    {
        image = diceSideSprite.GetComponent<Image>();
        amount = amountText.GetComponent<TextMeshProUGUI>();
        description = descriptionText.GetComponent<TextMeshProUGUI>();
        amount.text = "x1";
        description.text = "Dice Side Description";
    }

    public void UpdateItem(GameObject side, int count)
    {
        image.sprite = side.GetComponent<SpriteRenderer>().sprite;
        amount.text = "x" + count;
        description.text = side.GetComponent<IDiceSide>().Description;
    }
}
