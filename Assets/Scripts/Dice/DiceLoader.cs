using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceLoader : MonoBehaviour
{
    [SerializeField] int stdLoadAmt = 7;
    [SerializeField] List<GameObject> loadedDice = null;
    [SerializeField] List<Transform> loadPositions = null;
    [SerializeField] Transform spawnPos = null;
    [SerializeField] Transform destroyPos = null;
    [SerializeField] float loadInterval = 0.2f;
    [SerializeField] float firstLoadTime = 0.3f;
    [SerializeField] float lastUnloadTime = 0.3f;
    [SerializeField] float moveTime = 0.2f;
    [SerializeField] GameObject DiceParent = null;

    CombatManager combatManager;
    DiceManager diceManager;

    private void Start()
    {
        combatManager = FindObjectOfType<CombatManager>();
        diceManager = FindObjectOfType<DiceManager>();
    }

    public void LoadDice(List<GameObject> currentDice)
    {
        int currentDiceAmt = currentDice.Count;
        int loadAmt = (currentDiceAmt > stdLoadAmt) ? stdLoadAmt : currentDiceAmt;
        StartCoroutine(LoadDiceCoroutine(loadAmt, currentDice));
    }

    IEnumerator LoadDiceCoroutine(int loadAmt, List<GameObject> currentDice)
    {
        for(int i = 0; i < loadAmt; i++)
        {
            int randIndex = Random.Range(0, currentDice.Count);
            GameObject newDie = Instantiate(currentDice[randIndex], spawnPos.position, Quaternion.identity);
            newDie.transform.parent = DiceParent.transform;
            newDie.GetComponent<Dice>().SetLoadIndex(i);
            loadedDice.Add(newDie);
            float loadTime = firstLoadTime - (0.02f * i);
            LeanTween.moveX(newDie, loadPositions[i].position.x, loadTime).setEaseOutQuad();
            yield return new WaitForSeconds(loadInterval);
            currentDice.RemoveAt(randIndex);
        }
        diceManager.DiceLoaded();
        combatManager.ActionComplete(CombatManager.DICE_LOADED);
    }

    public void PlaceDieBackOnLoader(GameObject die)
    {
        LeanTween.move(die, loadPositions[die.GetComponent<Dice>().loadIndex], moveTime).setEaseOutQuad();
    }

    public void UnloadDice()
    {
        StartCoroutine(UnloadDiceCoroutine());
    }

    IEnumerator UnloadDiceCoroutine()
    {
        int count = loadedDice.Count - 1;
        foreach (GameObject die in loadedDice)
        {
            float unloadTime = lastUnloadTime - (0.02f * count);
            LeanTween.moveX(die, destroyPos.position.x, unloadTime).setEaseInQuad();
            yield return new WaitForSeconds(loadInterval);
            count--;
        }
        combatManager.ActionComplete(CombatManager.DICE_UNLOADED);
    }
}
