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
    [SerializeField] public CombatState combatState;
    [SerializeField] List<GameObject> currentEnemies = null;
    [SerializeField] GameObject currentTarget = null;
    [SerializeField] float floorYPos = -1.95f;
    [SerializeField] int chipsLost = 0;
    [SerializeField] int chipsWon = 0;

    PlayerActions actions;

    GameObject player;

    int currentEnemyIndex = 0;

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
            float yPos = floorYPos + offset;
            enemy.transform.position = new Vector2(enemy.transform.position.x, yPos);
            LeanTween.moveX(enemy, enemyPos[i].position.x, moveTime).setEaseOutBack();
            currentEnemies.Add(enemy);
            yield return new WaitForSeconds(enemyOffsetTime);
        }
        yield return new WaitForSeconds(enemyOffsetTime);
        combatState = CombatState.PLAYERTURN;
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
        if(currentTarget == null)
        {
            SetTarget(currentEnemies[0]);
        }
        bool effectActive = player.GetComponent<StatusEffects>().CheckStatusEffects();
        if(effectActive)
        {
            yield return new WaitForSeconds(statusTime);
        }
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
        if(combatState != CombatState.WIN && combatState != CombatState.LOSE)
        {
            StartCoroutine(ChangeTurn());
        }
    }

    IEnumerator ChangeTurn()
    {
        combatState = CombatState.ENEMYTURN;
        yield return new WaitForSeconds(turnChangeTime);
        combatText.text = "Enemy Turn";
        Debug.Log(currentEnemies[0]);
        StartCoroutine(EnemyTurn(currentEnemies[0]));
    }

    IEnumerator EnemyTurn(GameObject enemy)
    {
        yield return new WaitForSeconds(bufferTime);
        bool effectActive = enemy.GetComponent<StatusEffects>().CheckStatusEffects();
        if (effectActive)
        {
            yield return new WaitForSeconds(statusTime);
        }
        var enemyAttacks = enemy.GetComponent<IEnemyCombat>();
        string action = enemyAttacks.DetermineAction();
        combatText.text = "Enemy uses " + action;
        currentEnemyIndex++;
    }

    public void AttackComplete()
    {
        combatText.text = "";
        if(combatState != CombatState.WIN && combatState != CombatState.LOSE)
        {
            if (currentEnemyIndex < currentEnemies.Count)
            {
                StartCoroutine(EnemyTurn(currentEnemies[currentEnemyIndex]));
            }
            else
            {
                combatState = CombatState.PLAYERTURN;
                currentEnemyIndex = 0;
                StartCoroutine(PlayerTurn());
            }
        }
    }

    public void EnemyDeath(GameObject enemy)
    {
        Debug.Log("Enemy Killed");
        currentEnemies.Remove(enemy);
        chipsWon += enemy.GetComponent<UnitStats>().chipValue;
        if(currentEnemies.Count > 0)
        {
            OverrideSetTarget(currentEnemies[0]);
            if(combatState == CombatState.ENEMYTURN)
            {
                currentEnemyIndex--;
            }
        }
        else
        {
            targetingCircle.SetActive(false);
            combatState = CombatState.WIN;
            PlayerWon();
        }
    }

    private void PlayerWon()
    {
        FindObjectOfType<BattleWin>().BattleWon(chipsLost, chipsWon);
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

    public void CombatTextMessage(string text)
    {
        combatText.text = text;
    }

    public void LoseChips(int chips)
    {
        chipsLost += chips;
    }
}
