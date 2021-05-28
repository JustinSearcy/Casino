using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Blackjack : MonoBehaviour, IAction
{
    private string actionName = "Blackjack";

    [SerializeField] float timeBetweenDraws = 0.75f;
    [SerializeField] Button standButton = null;
    [SerializeField] Button hitButton = null;
    [SerializeField] TextMeshProUGUI scoreText = null;
    [SerializeField] int currentScore = 0;
    [SerializeField] float animTime = 2f;
    [SerializeField] float pauseTime = 1f;
    [SerializeField] GameObject canvas = null;
    [SerializeField] GameObject hitParticles = null;
    [SerializeField] GameObject damagePopup = null;
    [SerializeField] GameObject bust = null;
    [SerializeField] float blackjackMulti = 1.25f;

    GameObject currentTarget = null;

    BasicDeck deck;

    public string ActionName
    {
        get { return actionName; }
    }

    public void StartAction(GameObject target)//Give it some kind of intro transition later with tweening
    {
        currentTarget = target;
        DisableButtons();
        UpdateScoreText();
        deck = FindObjectOfType<BasicDeck>();
        StartCoroutine(FirstCardDraws());
    }

    IEnumerator FirstCardDraws()
    {
        deck.Shuffle();
        deck.DrawCard(); 
        yield return new WaitForSeconds(timeBetweenDraws);
        CalculateScore();
        deck.DrawCard();
        yield return new WaitForSeconds(timeBetweenDraws);
        CalculateScore();
    }

    private void CalculateScore()
    {
        currentScore += deck.currentCards[deck.currentCards.Count - 1].GetComponent<CardDisplay>().cardValue;
        UpdateScoreText();
        if(deck.currentCards.Count > 1)
        {
            if(currentScore == 21) //BLACKJACK
            {
                DisableButtons();
                CalculateDamage(currentScore);
            }
            else if(currentScore > 21)
            {
                for (int i = 0; i < deck.currentCards.Count; i++)
                {
                    if (deck.currentCards[i].GetComponent<CardDisplay>().cardValue == 11 && !deck.currentCards[i].GetComponent<CardDisplay>().hasChanged) //CHECK FOR ACE
                    {
                        deck.currentCards[i].GetComponent<CardDisplay>().hasChanged = true;
                        currentScore -= 10;
                        UpdateScoreText();
                        EnableButtons();
                        break;
                    }
                }
                if(currentScore > 21) //BUST (Maybe have it deal Recoil Damage)
                {
                    DisableButtons();
                    StartCoroutine(Bust());
                }
            }
            else //NOTHING
            {
                EnableButtons();
            }
        }
    }

    IEnumerator Bust()
    {
        yield return new WaitForSeconds(pauseTime);
        GameObject bustEffect = Instantiate(bust, new Vector2(0, 1f), Quaternion.identity);
        yield return new WaitForSeconds(3f);
        canvas.SetActive(false);
        RemoveCards();
        Destroy(bustEffect);
        deck.currentCards.Clear();
        FindObjectOfType<CombatManager>().TurnEnd();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "" + currentScore;
    }

    private void DisableButtons()
    {
        standButton.interactable = false;
        hitButton.interactable = false;
    }

    private void EnableButtons()
    {
        standButton.interactable = true;
        hitButton.interactable = true;
    }

    private void CalculateDamage(int score)
    {
        UnitStats playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<UnitStats>();
        UnitStats targetStats = currentTarget.GetComponent<UnitStats>();
        //float critMultiplier = CheckCrit(playerStats);
        score = (score == 21) ? (int)(score * blackjackMulti) : score; //Increase score if blackjack achieved
        int damage = (int)((playerStats.strength + score) - targetStats.physDefense);
        StartCoroutine(DealDamage(damage));
    }

    IEnumerator DealDamage(int damage)
    {
        yield return new WaitForSeconds(pauseTime);
        canvas.SetActive(false);
        RemoveCards();
        GameObject particles = Instantiate(hitParticles, currentTarget.transform);
        yield return new WaitForSeconds(animTime);
        FindObjectOfType<Shake>().CamShake();
        GameObject text = Instantiate(damagePopup, currentTarget.transform);
        text.GetComponent<DamagePopup>().UpdateText(damage);
        Destroy(particles, animTime);
        currentTarget.GetComponent<EnemyHealth>().TakeDamage(damage);
        deck.currentCards.Clear();
        FindObjectOfType<CombatManager>().TurnEnd();
        Destroy(this.gameObject, 2*animTime);
    }

    private void RemoveCards()
    {
        for (int i = 0; i < deck.currentCards.Count; i++)
        {
            Destroy(deck.currentCards[i].gameObject);
        }
    }

    //Maybe Add back in later
    //private float CheckCrit(UnitStats playerStats)
    //{
    //    float critIfGreater = UnityEngine.Random.Range(0f, 1f);
    //    Debug.Log(critIfGreater);
    //    Debug.Log(playerStats.critChance);
    //    if (playerStats.critChance > critIfGreater)
    //    {
    //        return 1.3f;
    //    }
    //    else
    //    {
    //        return 1f;
    //    }
    //}

    public void Stand()
    {
        CalculateDamage(currentScore);
    }

    public void Hit()
    {
        StartCoroutine(DrawNewCard());
    }

    IEnumerator DrawNewCard()
    {
        deck.DrawCard();
        yield return new WaitForSeconds(timeBetweenDraws);
        CalculateScore();
    }
}
