using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyCombat : MonoBehaviour, IEnemyCombat
{
    [SerializeField] float timeBeforeAttack = 0.5f;

    Health health;
    Health playerHealth;

    public string actionOneName = "Attack";
    public string actionTwoName = "Heal";
    public string actionThreeName = "Something";

    private int currentAction;

    private void Start()
    {
        health = this.gameObject.GetComponent<Health>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    public void DetermineAction()
    {
        if (playerHealth.currentHealth < 20) //Do the attack action if it can kill player
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
        playerHealth.TakeDamage(20);
    }

    IEnumerator ActionTwo()//Defensive Action
    {
        yield return new WaitForSeconds(timeBeforeAttack);
        health.Heal(20);
    }

    IEnumerator ActionThree()//Something Else to Add Later, special ability?
    {
        yield return new WaitForSeconds(timeBeforeAttack);
        throw new System.NotImplementedException();
    }
}
