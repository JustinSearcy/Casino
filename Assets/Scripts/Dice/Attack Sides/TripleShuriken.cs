using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShuriken : MonoBehaviour, IDiceSide
{
    private ActionTargets actionTarget = ActionTargets.SINGLE_TARGET_ENEMY;
    private ActionNames actionName = ActionNames.TRIPLE_SHURIKEN;

    [SerializeField] int damage = 3;
    [SerializeField] int attackTimes = 3;
    [SerializeField] float timeBetweenAttacks = 0.2f;

    Buffs buffs;
    DiceManager manager;

    public ActionTargets ActionTarget
    {
        get => actionTarget;
    }

    public ActionNames ActionName
    {
        get => actionName;
    }

    public string Description
    {
        get => "Deal " + GetUpdatedColor() + " Damage To A Single Enemy " + attackTimes + " Times";
    }

    public string RolledDescription
    {
        get => "Deal " + GetUpdatedRolledColor() + " Damage To A Single Enemy " + attackTimes + " Times";
    }

    public string GetUpdatedColor()
    {
        manager = manager == null ? FindObjectOfType<DiceManager>() : manager;
        return manager.GetDescriptionColor(damage, GetUpdatedVal());
    }

    public string GetUpdatedRolledColor()
    {
        manager = manager == null ? FindObjectOfType<DiceManager>() : manager;
        return manager.GetDescriptionColor(damage, GetUpdatedRolledVal());
    }

    public int GetUpdatedVal()
    {
        buffs = buffs == null ? GameObject.FindGameObjectWithTag("Player").GetComponent<Buffs>() : buffs;
        float buff = buffs.attackBuff;
        return (int)(damage * buff);
    }

    public int GetUpdatedRolledVal()
    {
        buffs = buffs == null ? GameObject.FindGameObjectWithTag("Player").GetComponent<Buffs>() : buffs;
        float buff = buffs.attackBuff;
        return (int)(damage * buff);
    }

    public void Action()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        ActionManager actionManager = FindObjectOfType<ActionManager>();
        CombatManager combatManager = FindObjectOfType<CombatManager>();
        for(int i = 0; i < attackTimes; i++)
        {
            if(combatManager.NoVictoryOrDefeat())
            {
                actionManager.DealDamageToCurrentTarget(GetUpdatedRolledVal());
                yield return new WaitForSeconds(timeBetweenAttacks);
            }
                
        }
    }
}
