using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public enum CombatState { START, ENEMY_INTENT, PLAYER_TURN, PLAYER_ATTACK, ENEMY_TURN, WIN, LOSE }

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

    [Header("Timing")]
    [SerializeField] float enemyIntentDelay = 0.5f;
    [SerializeField] float intentToDiceLoadDelay = 0.75f;

    [Header("UI")]
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
    [SerializeField] public GameObject currentActionTarget = null;

    PlayerActions actions;
    DiceManager diceManager;

    GameObject player;

    public const String SPAWN = "Spawn";
    public const String ENEMY_INTENT = "Enemy Intent";
    public const String DICE_LOADED = "Dice Loaded";
    public const String DICE_ROLLED = "Dice Rolled";
    public const String ALL_PLAYER_ACTIONS_COMPLETE = "All Player Actions Complete";
    public const String ENEMY_ACTION_COMPLETE = "Enemy Action Complete";
    public const String ALL_ENEMY_ACTIONS_COMPLETE = "All Enemy Actions Complete";

    int currentEnemyIndex = 0;

    void Start()
    {
        combatState = CombatState.START;
        diceManager = FindObjectOfType<DiceManager>();
        SetUpBattle();
    }

    private void SetUpBattle()
    {
        StartCoroutine(SpawnCharacters());
    }

    IEnumerator SpawnCharacters() //Instantiate player/enemies move them to start position, set intial target
    {
        yield return new WaitForSeconds(waitToStartTime);
        player = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
        FindObjectOfType<ChipCombatUI>().UpdateChipText(FindObjectOfType<ChipSystem>().getChips());
        LeanTween.moveX(player, playerPos.position.x, moveTime).setEaseOutBack();
        //PlayerSetUp();
        yield return new WaitForSeconds(enemyWaitTime);
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            GameObject enemy = Instantiate(enemyPrefabs[i], enemySpawn.position, Quaternion.identity);
            float offset = enemy.GetComponent<Collider2D>().bounds.size.y / 2f;
            float yPos = floorYPos + offset;
            enemy.transform.position = new Vector2(enemy.transform.position.x, yPos);
            LeanTween.moveX(enemy, enemyPos[i].position.x, moveTime).setEaseOutBack();
            currentEnemies.Add(enemy);
            yield return new WaitForSeconds(enemyOffsetTime);
        }
        yield return new WaitForSeconds(enemyOffsetTime);
        ActionComplete(CombatManager.SPAWN);
    }

    //private void PlayerSetUp()
    //{
    //    actions = FindObjectOfType<PlayerActions>();
    //    for (int i = 0; i < 3; i++)
    //    {
    //        if (actions.currentActions[i] != null)
    //        {
    //            attackMenu.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = actions.currentActions[i].name;
    //        }
    //        //Disable button if no action available in slot?
    //    }
    //}

    IEnumerator EnemyIntent()
    {
        yield return new WaitForSeconds(enemyIntentDelay);
        foreach(GameObject enemy in currentEnemies)
        {
            enemy.GetComponent<IEnemyCombat>().DetermineAction();
        }
        yield return new WaitForSeconds(intentToDiceLoadDelay);
        ActionComplete(CombatManager.ENEMY_INTENT);      
    }

    

    //IEnumerator PlayerTurn()
    //{
    //    yield return new WaitForSeconds(turnChangeTime);
    //    bool effectActive = player.GetComponent<StatusEffects>().CheckStatusEffects();
    //    if (effectActive)
    //        yield return new WaitForSeconds(statusTime);
    //    playerMenu.SetActive(true);
    //}

    //public void OnAttackButton(int attackButton)
    //{
    //    if (combatState != CombatState.PLAYER_TURN && currentTarget != null)
    //    {
    //        return;
    //    }
    //    if (actions.currentActions[attackButton] != null)
    //    {
    //        combatState = CombatState.PLAYER_ATTACK;
    //        StartCoroutine(PlayerAction(attackButton));
    //    }
    //}

    //IEnumerator PlayerAction(int action)
    //{
    //    attackMenu.SetActive(false);
    //    itemMenu.SetActive(false);
    //    //disable other menu too
    //    GameObject currentAction = Instantiate(actions.currentActions[action]); //Delete action when done
    //    //combatText.text = "You used " + currentAction.GetComponent<IAction>().ActionName;
    //    currentAction.GetComponent<IAction>().StartAction(currentTarget);
    //    yield return new WaitForSeconds(attackTime);
    //}

    //public void TurnEnd()
    //{
    //    if (combatState != CombatState.WIN && combatState != CombatState.LOSE)
    //    {
    //        StartCoroutine(ChangeTurn());
    //    }
    //}

    IEnumerator EnemyTurn(GameObject enemy)
    {
        yield return new WaitForSeconds(bufferTime);
        bool effectActive = enemy.GetComponent<StatusEffects>().CheckStatusEffects();
        if (effectActive)
        {
            yield return new WaitForSeconds(statusTime);
        }
        enemy.GetComponent<IEnemyCombat>().Action();
        //string action = enemyAttacks.DetermineAction();
        //combatText.text = "Enemy uses " + action;
        currentEnemyIndex++;
    }

    public void EnemyDeath(GameObject enemy)
    {
        Debug.Log("Enemy Killed");
        currentEnemies.Remove(enemy);
        if (currentEnemies.Count > 0 && combatState == CombatState.ENEMY_TURN)
            currentEnemyIndex--;
        else
        {
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
    }

    public void LoseChips(int chips)
    {
        chipsLost += chips;
    }   

    public void SetActionTarget(GameObject target)
    {
        currentActionTarget = target;
    }


    public void ActionComplete(String action)
    {
        switch(action)
        {
            case CombatManager.SPAWN:
                combatState = CombatState.ENEMY_INTENT;
                StartCoroutine(EnemyIntent());
                break;

            case CombatManager.ENEMY_INTENT:
                combatState = CombatState.PLAYER_TURN;
                diceManager.LoadNewDice();
                break;

            case CombatManager.DICE_LOADED:
                break;

            case CombatManager.DICE_ROLLED:
                combatState = CombatState.PLAYER_ATTACK;
                break;

            case CombatManager.ALL_PLAYER_ACTIONS_COMPLETE:
                combatState = CombatState.ENEMY_TURN;
                StartCoroutine(EnemyTurn(currentEnemies[0]));
                break;

            case CombatManager.ENEMY_ACTION_COMPLETE:
                if (combatState != CombatState.WIN && combatState != CombatState.LOSE)
                {
                    if (currentEnemyIndex < currentEnemies.Count)
                        StartCoroutine(EnemyTurn(currentEnemies[currentEnemyIndex]));
                    else
                        ActionComplete(CombatManager.ALL_ENEMY_ACTIONS_COMPLETE);
                }
                break;

            case CombatManager.ALL_ENEMY_ACTIONS_COMPLETE:
                currentEnemyIndex = 0;
                combatState = CombatState.ENEMY_INTENT;
                StartCoroutine(EnemyIntent());
                break;

            default:
                Debug.LogError("I was not ready for this");
                break;
        }
    }
}
