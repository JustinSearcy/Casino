using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FramesDisplay : MonoBehaviour
{
    TextMeshProUGUI framesText;
    private int avgFrameRate;

    private void Start()
    {
        framesText = this.GetComponent<TextMeshProUGUI>();
    }

    public void Update()
    {
        float current;
        current = Time.frameCount / Time.time;
        avgFrameRate = (int)current;
        framesText.text = avgFrameRate.ToString() + " FPS";
    }
}
