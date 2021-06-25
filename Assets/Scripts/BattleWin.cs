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

    void Start()
    {
        chipLostText.text = "";
        chipWonText.text = "";
        chipNetText.text = "";
    }

    public void BattleWon(int chipsLost, int chipsWon)
    {
        LeanTween.moveLocalY(this.gameObject, 0, moveTime).setEaseOutBack();
        int chipsNet = chipsWon - chipsLost;
        chipLostText.text = "" + chipsLost;
        chipWonText.text = "" + chipsWon;
        chipNetText.text = "" + chipsNet;
    }

}
