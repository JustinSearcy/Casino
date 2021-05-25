using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public enum CombatState { START, PLAYERTURN, PLAYERATTACK, ENEMYTURN, WIN, LOSE }

public class CombatManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject[] enemyPrefabs = null;
    [SerializeField] GameObject playerPrefab = null;
    [SerializeField] GameObject targetingCircle = null;

    [Header("Transforms")]
    [SerializeField] Transform playerSpawn = null;
    [SerializeField] Transform playerPos = null;
    [SerializeField] Transform enemySpawn = null;
    [SerializeField] Transform[] enemyPos = null;

    [Header("Animation")]
    [SerializeField] float moveTime = 1.5f;
    [SerializeField] float waitToStartTime = 2f;
    [SerializeField] float enemyWaitTime = 2.5f;
    [SerializeField] float enemyOffsetTime = 0.5f;
    [SerializeField] float attackTime = 2f;
    [SerializeField] float bufferTime = 1.5f;
    [SerializeField] float turnChangeTime = 2f;
    [SerializeField] float statusTime = 1.5f;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI combatText = null;
    [SerializeField] GameObject playerMenu = null;
    [SerializeField] GameObject attackMenu = null;
    [SerializeField] GameObject itemMenu = null;
    //Other Menu

    [Header("Misc")]
    [SerializeField] CombatState combatState;
    [SerializeField] List<GameObject> currentEnemies = null;
    [SerializeField] GameObject currentTarget = null;
    [SerializeField] float floorYPos = -1.95f;

    PlayerActions actions;

    GameObject player;

    void Start()
    {
        combatState = CombatState.START;
        SetUpBattle();
    }

    private void SetUpBattle()
    {
        combatText.text = "Enemies Approach...";
        StartCoroutine(SpawnCharacters());
    }

    IEnumerator SpawnCharacters() //Instantiate player/enemies move them to start position, set intial target
    {
        yield return new WaitForSeconds(waitToStartTime);
        player = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
        FindObjectOfType<ChipCombatUI>().UpdateChipText(FindObjectOfType<ChipSystem>().getChips());
        LeanTween.moveX(player, playerPos.position.x, moveTime).setEaseOutBack();
        PlayerSetUp();
        yield return new WaitForSeconds(enemyWaitTime);
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            GameObject enemy = Instantiate(enemyPrefabs[i], enemySpawn.position, Quaternion.identity);
            float offset =  enemy.GetComponent<Collider2D>().bounds.size.y / 2f;
            Debug.Log(offset);
            float yPos = floorYPos + offset;
            enemy.transform.position = new Vector2(enemy.transform.position.x, yPos);
            LeanTween.moveX(enemy, enemyPos[i].position.x, moveTime).setEaseOutBack();
            currentEnemies.Add(enemy);
            yield return new WaitForSeconds(enemyOffsetTime);
        }
        yield return new WaitForSeconds(enemyOffsetTime);
        combatState = CombatState.PLAYERTURN;
        SetTarget(currentEnemies[0]);
        StartCoroutine(PlayerTurn());
    }

    private void PlayerSetUp()
    {
        actions = FindObjectOfType<PlayerActions>();
        for (int i = 0; i < 3; i++)
        {
            if(actions.currentActions[i] != null)
            {
                attackMenu.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = actions.currentActions[i].name;
            }
            //Disable button if no action available in slot?
        }
    }

    IEnumerator PlayerTurn()
    {
        yield return new WaitForSeconds(turnChangeTime);
        string effect = player.GetComponent<StatusEffects>().CheckStatusEffects();
        if(effect != "")
        {
            combatText.text = effect + " took effect";
        }
        yield return new WaitForSeconds(statusTime);
        combatText.text = "Your Turn";
        playerMenu.SetActive(true);
    }

    public void OnAttackButton(int attackButton)
    {
        if(combatState != CombatState.PLAYERTURN && currentTarget != null)
        {
            return;
        }
        if(actions.currentActions[attackButton] != null)
        {
            combatState = CombatState.PLAYERATTACK;
            StartCoroutine(PlayerAction(attackButton));
        }
    }

    IEnumerator PlayerAction(int action)
    {
        attackMenu.SetActive(false);
        itemMenu.SetActive(false);
        //disable other menu too
        GameObject currentAction = Instantiate(actions.currentActions[action]); //Delete action when done
        combatText.text = "You used " + currentAction.GetComponent<IAction>().ActionName;
        currentAction.GetComponent<IAction>().StartAction(currentTarget);
        yield return new WaitForSeconds(attackTime);
    }

    public void TurnEnd()
    {
        combatState = CombatState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(turnChangeTime);
        combatText.text = "Enemy Turn";

        for (int i = 0; i < currentEnemies.Count; i++)
        {
            yield return new WaitForSeconds(bufferTime);
            string effect = currentEnemies[i].GetComponent<StatusEffects>().CheckStatusEffects();
            if (effect != "")
            {
                combatText.text = effect + " took effect";
            }
            yield return new WaitForSeconds(statusTime);
            var enemyAttacks = currentEnemies[i].GetComponent<IEnemyCombat>();
            string action = enemyAttacks.DetermineAction();
            combatText.text = "Enemy uses " + action;
            yield return new WaitForSeconds(attackTime);
            combatText.text = "";
            if(combatState == CombatState.LOSE)
            {
                break;
            }
        }
        if(combatState == CombatState.ENEMYTURN)
        {
            combatState = CombatState.PLAYERTURN;
            StartCoroutine(PlayerTurn());
        }
    }

    public void EnemyDeath(GameObject enemy)
    {
        Debug.Log("Enemy Killed");
        currentEnemies.Remove(enemy);
        OverrideSetTarget(currentEnemies[0]);
        if (currentEnemies.Count == 0)
        {
            combatState = CombatState.WIN;
            PlayerWon();
        }
    }

    private void PlayerWon()
    {
        combatText.text = "You Won";
    }

    public void PlayerLost()
    {
        combatState = CombatState.LOSE;
        combatText.text = "You Lost";
    }

    public void SetTarget(GameObject newTarget)
    {
        if(combatState == CombatState.PLAYERTURN)
        {
            currentTarget = newTarget;
            targetingCircle.transform.position = new Vector2(newTarget.transform.position.x, -3);
        }
    }

    private void OverrideSetTarget(GameObject newTarget)
    {
        currentTarget = newTarget;
        targetingCircle.transform.position = new Vector2(newTarget.transform.position.x, -3);
    }
}
