using UnityEngine;
using System.Collections;

public class StatusEffects : MonoBehaviour
{
    [Header("IMPORTANT")]
    [SerializeField] bool isPlayer = false;

    [Header("Poison")]
    [SerializeField] int poisonCounter = 0;
    [SerializeField] int poisonDamage = 10;
    [SerializeField] bool isPoisoned = false;
    [SerializeField] GameObject poisonIcon;

    CombatManager combatManager;

    private void Start()
    {
        combatManager = FindObjectOfType<CombatManager>();
    }

    public bool CheckStatusEffects()
    {
        if (isPoisoned)
        {
            Poison();
            return true;
        }
        return false;
    }

    private void Poison()
    {
        poisonCounter--;
        this.gameObject.GetComponent<Health>().TakeDamage(poisonDamage);

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
