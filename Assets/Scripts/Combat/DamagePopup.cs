using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] TextMeshPro damageText = null;
    [SerializeField] float enlargeTime = 0.5f;
    [SerializeField] float shrinkTime = 1f;
    [SerializeField] float yPosFirstIncrease = 1.5f;
    [SerializeField] float xPosMaxMovement = 0.5f;
    [SerializeField] float textSize = 2f;

    void Start()
    {
        StartCoroutine(Display());
    }

    IEnumerator Display()
    {
        GameObject textPopup = this.gameObject;
        LeanTween.scale(textPopup, new Vector2(textSize, textSize), enlargeTime);
        LeanTween.moveY(textPopup, (textPopup.transform.position.y + yPosFirstIncrease), enlargeTime + shrinkTime).setEaseOutExpo();
        LeanTween.moveX(textPopup, (textPopup.transform.position.x + xPosMaxMovement), enlargeTime + shrinkTime);
        yield return new WaitForSeconds(enlargeTime);
        LeanTween.scale(textPopup, Vector2.zero, shrinkTime);
        Destroy(this.gameObject, shrinkTime);
    }

    public void UpdateText(int damage)
    {
        damageText.text = "" + damage;
    }
}
