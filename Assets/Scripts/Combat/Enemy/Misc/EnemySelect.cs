using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelect : MonoBehaviour
{
    [SerializeField] private float outlineAlpha = 0.75f;
    [SerializeField] private GameObject sprite = null;

    CombatManager combatManager;
    DiceManager diceManager;
    Material mat;

    private void Start()
    {
        combatManager = FindObjectOfType<CombatManager>();
        diceManager = FindObjectOfType<DiceManager>();
        mat = sprite.GetComponent<Renderer>().material;
        mat.SetFloat("_OutlineAlpha", 0);
    }

    private void OnMouseDown()
    {
        if (combatManager.combatState == CombatState.PLAYER_ATTACK)
            diceManager.TryAction("Enemy", gameObject);
    }

    private void OnMouseEnter()
    {
        if (combatManager.combatState == CombatState.PLAYER_ATTACK && diceManager.ValidAction("Enemy"))
            mat.SetFloat("_OutlineAlpha", outlineAlpha);
    }

    private void OnMouseExit()
    {
        mat.SetFloat("_OutlineAlpha", 0);
    }
}
