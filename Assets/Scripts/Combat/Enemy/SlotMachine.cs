using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachine : MonoBehaviour, IEnemyCombat
{
    [Header("Timing")]
    [SerializeField] float timeBeforeAttack = 0.5f;
    [SerializeField] float timeAfterAttack = 0.75f;
    [SerializeField] float timeBetweenJackpotAttacks = 0.25f;
    [SerializeField] float luckyCloverTime = 2.5f;

    [Header("Spin Odds")]
    //[SerializeField] float jackpotOdds = 0.1f;
    [SerializeField] float luckyCloverOdds = 0.4f;
    [SerializeField] float backfireOdds = 0.15f;
    [SerializeField] float freeSpinOdds = 0.15f;
    [SerializeField] float nothingOdds = 0.2f;

    [Header("Damage")]
    [SerializeField] float backfireMinLoss = 0.15f;
    [SerializeField] float backfireMaxLoss = 0.2f;

    [Header("Prefabs")]
    [SerializeField] GameObject luckyCloverParticles = null;

    public string actionOneName = "Spin";

    Health health;
    Health playerHealth;
    CombatManager combatManager;

    private void Start()
    {
        health = this.gameObject.GetComponent<Health>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        combatManager = FindObjectOfType<CombatManager>();
    }

    public void DetermineAction()
    {
        
    }

    public void Action()
    {
        StartCoroutine(ActionOne());
    }

    IEnumerator ActionOne()
    {
        float num = Random.Range(0, 1f);
        yield return new WaitForSeconds(timeBeforeAttack);
        if (num <= luckyCloverOdds)
        {
            this.gameObject.GetComponent<Animator>().SetTrigger("LuckyClover");
        }
        else if (num <= (luckyCloverOdds + nothingOdds))
        {
            this.gameObject.GetComponent<Animator>().SetTrigger("Nothing");
        }
        else if(num <= (luckyCloverOdds + nothingOdds + backfireOdds))
        {
            this.gameObject.GetComponent<Animator>().SetTrigger("Backfire");
        }
        else if (num <= (luckyCloverOdds + nothingOdds + backfireOdds + freeSpinOdds)) 
        {
            this.gameObject.GetComponent<Animator>().SetTrigger("FreeSpin");
        }
        else 
        {
            this.gameObject.GetComponent<Animator>().SetTrigger("Jackpot");
        }

        //if(FindObjectOfType<CameraZoom>().gameObject.GetComponent<Camera>().orthographicSize == 5)
        //{
        //    FindObjectOfType<CameraZoom>().ZoomTarget(this.gameObject.transform);
        //}
    }

    public void LuckyClover()
    {
        StartCoroutine(LuckyCloverHit());
    }

    IEnumerator LuckyCloverHit()
    {
        FindObjectOfType<CameraZoom>().ZoomCenter();
        int magic = this.gameObject.GetComponent<UnitStats>().magic;
        int magicDefense = playerHealth.gameObject.GetComponent<UnitStats>().magDefense;
        int damage = (magic * 4) - magicDefense;
        Vector2 particlePos = new Vector2(playerHealth.gameObject.transform.position.x, 5.5f);
        GameObject particles = Instantiate(luckyCloverParticles, particlePos, Quaternion.identity);
        Destroy(particles, 4f);
        yield return new WaitForSeconds(luckyCloverTime);
        playerHealth.TakeDamage(damage);
        StartCoroutine(NextCharacter());
    }

    public void Nothing()
    {
        FindObjectOfType<CameraZoom>().ZoomCenter();
        StartCoroutine(NextCharacter());
    }

    public void Backfire()
    {
        FindObjectOfType<CameraZoom>().ZoomCenter();
        float healthLossPercent = Random.Range(backfireMinLoss, backfireMaxLoss);
        int healthLoss = (int)(health.currentHealth * healthLossPercent);
        health.TakeDamage(healthLoss);
        StartCoroutine(NextCharacter());
    }

    public void FreeSpin()
    {
        DetermineAction();
    }

    public void Jackpot()
    {
        FindObjectOfType<CameraZoom>().ZoomCenter();
        StartCoroutine(HitJackpot());
    }

    IEnumerator HitJackpot()
    {
        for (int i = 0; i < 3; i++)
        {
            playerHealth.TakeDamage(7);
            yield return new WaitForSeconds(timeBetweenJackpotAttacks);
        }
        StartCoroutine(NextCharacter());
    }

    IEnumerator NextCharacter()
    {
        yield return new WaitForSeconds(timeAfterAttack);
        combatManager.ActionComplete(CombatManager.ENEMY_ACTION_COMPLETE);
    }
}
