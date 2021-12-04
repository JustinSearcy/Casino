using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyCombat : MonoBehaviour, IEnemyCombat
{
    [SerializeField] float timeBeforeAttack = 0.5f;

    EnemyHealth health;
    ChipSystem chips;

    public string actionOneName = "Attack";
    public string actionTwoName = "Heal";
    public string actionThreeName = "Something";

    private int currentAction;

    private void Start()
    {
        health = this.gameObject.GetComponent<EnemyHealth>();
        chips = FindObjectOfType<ChipSystem>();
    }

    public void DetermineAction()
    {
        if (chips.getChips() < 20) //Do the attack action if it can kill player
        {
            currentAction = 1;
        }
        else if (health.GetHealthPercent() < 0.25) //Do some kind of defensive action
        {
            currentAction = 2;
        }
        else //Default Action
        {
            currentAction = 3;
        }
    }

    public void Action()
    {
        if (currentAction == 1)
            StartCoroutine(ActionOne());
        else if (currentAction == 2)
            StartCoroutine(ActionTwo());
        else
            StartCoroutine(ActionThree());
    }

    IEnumerator ActionOne()//Attack Action
    {
        yield return new WaitForSeconds(timeBeforeAttack);
        chips.LoseChips(20);
        Debug.Log("Enemy Attacks");
    }

    IEnumerator ActionTwo()//Defensive Action
    {
        yield return new WaitForSeconds(timeBeforeAttack);
        health.Heal(20);
        Debug.Log("Enemy Heals");
    }

    IEnumerator ActionThree()//Something Else to Add Later, special ability?
    {
        yield return new WaitForSeconds(timeBeforeAttack);
        throw new System.NotImplementedException();
    }
}
