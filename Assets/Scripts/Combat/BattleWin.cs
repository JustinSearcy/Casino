using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleWin : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI chipLostText = null;
    [SerializeField] TextMeshProUGUI chipWonText = null;
    [SerializeField] TextMeshProUGUI chipNetText = null;
    [SerializeField] float moveTime = 2f;
    [SerializeField] float chipDisplayBufferTime = 0.25f;
    [SerializeField] float initialNetTextSize = 60f;
    [SerializeField] float finalNetTextSize = 40f;
    [SerializeField] float netTextAnimTime = 0.2f;

    NumberAnimator numAnim;

    private int chipsNet;
    private int chipsLost;
    private int chipsWon;

    void Start()
    {
        chipLostText.text = "";
        chipWonText.text = "";
        chipNetText.text = "";
        numAnim = FindObjectOfType<NumberAnimator>();
    }

    public void BattleWon(int lost, int won)
    {
        chipsLost = lost;
        chipsWon = won;
        LeanTween.moveLocalY(this.gameObject, 0, moveTime).setEaseOutBack();
        chipsNet = chipsWon - chipsLost;
        StartCoroutine(ShowDetails());
    }

    private IEnumerator ShowDetails()
    {
        yield return new WaitForSeconds(moveTime + 1f);
        numAnim.UpdateText(chipsLost, chipLostText);
        yield return new WaitForSeconds(numAnim.duration + chipDisplayBufferTime);
        numAnim.UpdateText(chipsWon, chipWonText);
        yield return new WaitForSeconds(numAnim.duration + chipDisplayBufferTime);
        chipNetText.text = chipsNet.ToString();
        if(chipsNet > 0)
        {
            chipNetText.color = new Color32(57, 226, 58, 255);
        }
        else if(chipsNet < 0)
        {
            chipNetText.color = new Color32(224, 14, 14, 255);
        }
        LeanTween.value(chipNetText.gameObject, initialNetTextSize, finalNetTextSize, netTextAnimTime).setLoopPingPong().setOnUpdate(SetFontSize);
    }

    private void SetFontSize(float value)
    {
        chipNetText.fontSize = value;
    }
}
