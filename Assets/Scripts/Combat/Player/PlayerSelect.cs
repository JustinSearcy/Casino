using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelect : MonoBehaviour
{
    [SerializeField] float outlineAlpha = 0.75f;

    CombatManager combatManager;
    DiceManager diceManager;
    Material mat;

    private void Start()
    {
        combatManager = FindObjectOfType<CombatManager>();
        diceManager = FindObjectOfType<DiceManager>();
        mat = gameObject.GetComponent<Renderer>().material;
        mat.SetFloat("_OutlineAlpha", 0);
    }

    private void OnMouseDown()
    {
        Debug.Log("PlayerClicked");
        if (combatManager.combatState == CombatState.PLAYER_ATTACK)
            diceManager.TryAction("Player", gameObject);
    }

    private void OnMouseEnter()
    {
        if (combatManager.combatState == CombatState.PLAYER_ATTACK && diceManager.ValidAction("Player"))
            mat.SetFloat("_OutlineAlpha", outlineAlpha);
    }

    private void OnMouseExit()
    {
        mat.SetFloat("_OutlineAlpha", 0);
    }
}
