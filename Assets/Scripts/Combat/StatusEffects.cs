using UnityEngine;
using System.Collections;

public class StatusEffects : MonoBehaviour
{
    [Header("Poison")]
    [SerializeField] int poisonCounter = 0;
    [SerializeField] int poisonDamage = 10;
    [SerializeField] bool isPoisoned = false;

    private bool isPlayer = false;

    CombatManager combatManager;

    private void Start()
    {
        if(this.gameObject.tag == "Player")
        {
            isPlayer = true;
        }

        combatManager = FindObjectOfType<CombatManager>();
    }

    public bool CheckStatusEffects()
    {
        if (isPoisoned)
        {
            Poison();
            combatManager.CombatTextMessage("Poison took effect!");
            return true;
        }
        return false;
    }

    private void Poison()
    {
        poisonCounter--;
        if (isPlayer) {
            this.gameObject.GetComponent<ChipSystem>().LoseChips(poisonDamage);
        }
        else
        {
            this.gameObject.GetComponent<EnemyHealth>().TakeDamage(poisonDamage);
        }

        if(poisonCounter <= 0)
        {
            isPoisoned = false;
        }
    }

    public void Poisoned(int length)
    {
        poisonCounter = length;
        isPoisoned = true;
    }
}
