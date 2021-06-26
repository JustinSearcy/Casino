using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberAnimator : MonoBehaviour
{
    [SerializeField] int countFPS = 30;

    public float duration = 1f;

    private Coroutine CountingCoroutine;

    public void UpdateText(int newValue, TextMeshProUGUI text)
    {
        if (CountingCoroutine != null)
        {
            StopCoroutine(CountingCoroutine);
        }

        CountingCoroutine = StartCoroutine(CountText(newValue, text));
    }

    private IEnumerator CountText(int newValue, TextMeshProUGUI text)
    {
        if(newValue == 0)
        {
            text.text = newValue.ToString();
        }
        else
        {
            WaitForSeconds wait = new WaitForSeconds(1f / countFPS);
            int previousValue = 0;
            int stepAmount;

            if (newValue - previousValue < 0)
            {
                stepAmount = Mathf.FloorToInt((newValue - previousValue) / (countFPS * duration));
            }
            else
            {
                stepAmount = Mathf.CeilToInt((newValue - previousValue) / (countFPS * duration));
            }

            if (previousValue < newValue)
            {
                while (previousValue < newValue)
                {
                    previousValue += stepAmount;
                    if (previousValue > newValue)
                    {
                        previousValue = newValue;
                    }

                    text.text = previousValue.ToString();

                    yield return wait;
                }
            }
        }
    }
}
