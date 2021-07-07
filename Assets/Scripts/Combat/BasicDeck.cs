using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDeck : MonoBehaviour
{
    [SerializeField] List<Card> fullDeck = null;
    [SerializeField] List<Card> currentDeck = null;
    public List<GameObject> currentCards = null;
    [SerializeField] GameObject card = null;
    [SerializeField] GameObject cardEnter = null;
    [SerializeField] float moveOffsetTime = 0.3f;
    [SerializeField] float moveTime = 0.5f;
    [SerializeField] float initialMoveTime = 0.5f;
    [SerializeField] float cardPosY = 0f;

    private void Start()
    {
        currentDeck = new List<Card>(fullDeck);
    }

    public void Shuffle()
    {
        currentDeck = new List<Card>(fullDeck);
    }

    public void DrawCard()
    {
        int cardIndex = UnityEngine.Random.Range(0, currentDeck.Count);
        Card drawnCard = currentDeck[cardIndex];
        currentDeck.RemoveAt(cardIndex);
        GameObject newCard = Instantiate(card, cardEnter.transform.position, Quaternion.identity);
        currentCards.Add(newCard);
        newCard.GetComponent<CardDisplay>().card = drawnCard;
        newCard.GetComponent<CardDisplay>().DisplayCard();
        StartCoroutine(MoveCards());
    }

    IEnumerator MoveCards()
    {
        for (int i = 0; i < currentCards.Count - 1; i++)
        {
            GameObject card = currentCards[i];
            Vector2 newPos = new Vector2(card.transform.position.x + 0.5f, card.transform.position.y);
            LeanTween.move(card, newPos, moveTime);
            yield return new WaitForSeconds(moveOffsetTime);
        }

        GameObject newCard = currentCards[currentCards.Count - 1];
        Vector3 initialPos = new Vector3(((currentCards.Count * -0.5f) + 0.5f), cardPosY, (currentCards.Count * -0.1f));
        LeanTween.move(newCard, initialPos, initialMoveTime);
    }
}
