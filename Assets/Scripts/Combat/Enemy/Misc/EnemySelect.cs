using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelect : MonoBehaviour
{
    CombatManager combatManager;
    DiceManager diceManager;

    private void Start()
    {
        combatManager = FindObjectOfType<CombatManager>();
        diceManager = FindObjectOfType<DiceManager>();
    }

    private void OnMouseDown()
    {
        Debug.Log("EnemyClicked");
        if (combatManager.combatState == CombatState.PLAYER_ATTACK)
            diceManager.TryAction("Enemy", gameObject);
    }
}
