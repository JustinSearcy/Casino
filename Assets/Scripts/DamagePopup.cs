using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] TextMeshPro damageText = null;
    [SerializeField] float enlargeTime = 0.5f;
    [SerializeField] float shrinkTime = 1f;
    [SerializeField] float yPosFirstIncrease = 0.75f;
    [SerializeField] float yPosSecondIncrease = 0.5f;

    void Start()
    {
        StartCoroutine(Display());
    }

    IEnumerator Display()
    {
        GameObject textPopup = this.gameObject;
        LeanTween.scale(textPopup, Vector2.one, enlargeTime);
        LeanTween.moveY(textPopup, (textPopup.transform.position.y + yPosFirstIncrease), enlargeTime);
        yield return new WaitForSeconds(enlargeTime);
        LeanTween.scale(textPopup, Vector2.zero, shrinkTime);
        LeanTween.moveY(textPopup, (textPopup.transform.position.y + yPosSecondIncrease), shrinkTime);
        Destroy(this.gameObject, shrinkTime);
    }

    public void UpdateText(int damage)
    {
        damageText.text = "" + damage;
    }
}
