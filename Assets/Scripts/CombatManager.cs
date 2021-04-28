using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum CombatState { START, PLAYERTURN, ENEMYTURN, WIN, LOSE }

public class CombatManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject[] enemyPrefabs = null;
    [SerializeField] GameObject playerPrefab = null;

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

    [Header("UI")]
    [SerializeField] TextMeshProUGUI combatText = null;
    [SerializeField] GameObject playerMenu = null;

    [Header("Misc")]
    [SerializeField] CombatState combatState;
    [SerializeField] List<GameObject> currentEnemies = null;
    [SerializeField] GameObject currentTarget = null;

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
        GameObject player = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
        LeanTween.moveX(player, playerPos.position.x, moveTime).setEaseOutBack();
        yield return new WaitForSeconds(enemyWaitTime);
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            GameObject enemy = Instantiate(enemyPrefabs[i], enemySpawn.position, Quaternion.identity);
            LeanTween.moveX(enemy, enemyPos[i].position.x, moveTime).setEaseOutBack();
            currentEnemies.Add(enemy);
            yield return new WaitForSeconds(enemyOffsetTime);
        }
        currentTarget = currentEnemies[0];
        yield return new WaitForSeconds(enemyOffsetTime);
        combatState = CombatState.PLAYERTURN;
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        combatText.text = "Your Turn";
        playerMenu.SetActive(true);
    }

    public void OnAttackButton()
    {
        if(combatState != CombatState.PLAYERTURN && currentTarget != null)
        {
            return;
        }


        currentTarget.GetComponent<EnemyHealth>().takeDamage(100);
        combatState = CombatState.ENEMYTURN;
    }

    public void EnemyDeath(GameObject enemy)
    {
        Debug.Log("Enemy Killed");
        currentEnemies.Remove(enemy);
        SetTarget(currentEnemies[0]);
        if (currentEnemies.Count == 0)
        {
            combatState = CombatState.WIN;
        }
    }

    public void SetTarget(GameObject newTarget)
    {
        currentTarget = newTarget;
    }
}
