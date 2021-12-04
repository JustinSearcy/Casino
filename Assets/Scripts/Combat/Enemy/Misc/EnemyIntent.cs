using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyIntent : MonoBehaviour
{
    [SerializeField] Sprite attackIcon = null;
    [SerializeField] Sprite randomIcon = null;
    [SerializeField] Sprite specialIcon = null;

    [SerializeField] float singleDigitOffset = 0.25f;

    public void SetAttack(GameObject intent, int amount)
    {
        GameObject icon = intent.transform.GetChild(0).gameObject;
        GameObject text = intent.transform.GetChild(1).gameObject;
        icon.GetComponent<SpriteRenderer>().sprite = attackIcon;
        text.GetComponent<TextMeshPro>().text = amount.ToString();
        icon.transform.localPosition = new Vector2(-singleDigitOffset, 0);
        text.transform.localPosition = new Vector2(singleDigitOffset, 0);
    }

    public void SetRandom(GameObject intent)
    {
        GameObject icon = intent.transform.GetChild(0).gameObject;
        GameObject text = intent.transform.GetChild(1).gameObject;
        icon.GetComponent<SpriteRenderer>().sprite = randomIcon;
        text.GetComponent<TextMeshPro>().text = string.Empty;
        icon.transform.localPosition = Vector2.zero;
    }

    public void SetSpecial(GameObject intent)
    {
        GameObject icon = intent.transform.GetChild(0).gameObject;
        GameObject text = intent.transform.GetChild(1).gameObject;
        icon.GetComponent<SpriteRenderer>().sprite = specialIcon;
        text.GetComponent<TextMeshPro>().text = string.Empty;
        icon.transform.localPosition = Vector2.zero;
    }
}
