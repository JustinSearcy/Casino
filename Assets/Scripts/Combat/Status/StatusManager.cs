using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class StatusManager : MonoBehaviour
{
    //Attach a status manager to each player/enemy
    [Header("IMPORTANT")]
    [SerializeField] bool isPlayer = false;

    [Header("Poison")]
    [SerializeField] int poisonCounter = 0;
    [SerializeField] float statusTime = 1.5f;

    CombatManager combatManager;

    Dictionary<Status, bool> allStatus;

    GameObject statusGrid;

    Status poison;
    GameObject poisonIcon;

    private void Start()
    {

        statusGrid = FindGameObjectInChildWithTag(gameObject, "StatusGrid");
        allStatus = new Dictionary<Status, bool>();
        combatManager = FindObjectOfType<CombatManager>();
        poison = FindObjectOfType<Poison>();
        allStatus.Add(poison, false);
    }

    public IEnumerator HandleStatusEffects()
    {
        foreach (KeyValuePair<Status, bool> entry in allStatus)
        {
            Debug.Log(entry.Key);
            Debug.Log(entry.Value);
            if (entry.Value)
                yield return StartCoroutine(ProcessStatus(entry.Key));
            if (combatManager.combatState == CombatState.LOSE || combatManager.combatState == CombatState.WIN)
                break;
        }
    }

    private IEnumerator ProcessStatus(Status status)
    {
        if (status == poison)
            yield return StartCoroutine(Poison());
        else
            Debug.Log("Status Not Recognized");
    }

    private void InitializeIcon(GameObject icon, int turnCount)
    {
        icon.transform.parent = statusGrid.transform;
        icon.GetComponent<RectTransform>().localScale = new Vector2(0.25f, 0.25f);
        icon.GetComponentInChildren<TextMeshPro>().text = turnCount.ToString();
    }

    private void UpdateIcon(GameObject icon, int turnCount)
    {
        icon.GetComponentInChildren<TextMeshPro>().text = turnCount.ToString();
    }

    private IEnumerator Poison()
    {
        poison.TakeEffect(gameObject);
        poisonCounter--;

        if (poisonCounter <= 0)
        {
            allStatus[poison] = false;
            Destroy(poisonIcon);
        }
        else
            UpdateIcon(poisonIcon, poisonCounter);

        yield return new WaitForSeconds(statusTime);
    }

    public void Poisoned()
    {
        if (!allStatus[poison])
        {
            poisonCounter = poison.InitCounter();
            allStatus[poison] = true;
            poisonIcon = Instantiate(poison.GetIcon(), Vector3.zero, Quaternion.identity);
            InitializeIcon(poisonIcon, poisonCounter);
        }
        else
        {
            poisonCounter += poison.InitCounter();
            UpdateIcon(poisonIcon, poisonCounter);
        }
        
    }

    public static GameObject FindGameObjectInChildWithTag(GameObject parent, string tag)
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == tag)
            {
                return tr.gameObject;
            }
        }
        return null;
    }
}
