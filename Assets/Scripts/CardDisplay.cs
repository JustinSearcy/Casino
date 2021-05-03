using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] GameObject cardFront = null;
    [SerializeField] GameObject suit = null;
    [SerializeField] TextMeshPro valueTop = null;
    [SerializeField] TextMeshPro valueBottom = null;

    public Card card;

    public void DisplayCard()
    {
        cardFront.GetComponent<SpriteRenderer>().sprite = card.cardFront;
        suit.GetComponent<SpriteRenderer>().sprite = card.suit;
        valueTop.text = card.display;
        valueBottom.text = card.display;
    }
}
