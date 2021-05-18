using UnityEngine;
using System.Collections;
using TMPro;

public class EnemyTooltip : MonoBehaviour
{
    [SerializeField] float xOffset = 1f;
    [SerializeField] float yOffset = 3f;

    private GameObject tooltip;
    private UnitStats unitStats;
    private EnemyHealth health;
    private bool isActive = false;

    private void Awake()
    {
        tooltip = GameObject.FindGameObjectWithTag("Tooltip");
        unitStats = this.gameObject.GetComponent<UnitStats>();
        health = this.gameObject.GetComponent<EnemyHealth>();
    }
    private void Update()
    {
        if (isActive)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 displayPos = new Vector2(mousePos.x - xOffset, mousePos.y + yOffset);
            tooltip.transform.position = displayPos;
        }
    }

    public void OnMouseEnter()
    {
        tooltip.SetActive(true);
        isActive = true;
        UpdateTooltip();
    }

    private void UpdateTooltip()
    {
        tooltip.transform.GetChild(1).GetComponent<TextMeshPro>().text = unitStats.unitName;
        tooltip.transform.GetChild(3).GetComponent<TextMeshPro>().text = health.currentHealth + "/" + health.maxHealth;
        tooltip.transform.GetChild(5).GetComponent<TextMeshPro>().text = unitStats.strength + "";
        tooltip.transform.GetChild(7).GetComponent<TextMeshPro>().text = unitStats.magic + "";
        tooltip.transform.GetChild(9).GetComponent<TextMeshPro>().text = unitStats.physDefense + "";
        tooltip.transform.GetChild(11).GetComponent<TextMeshPro>().text = unitStats.magDefense + "";
    }

    public void OnMouseExit()
    {
        tooltip.SetActive(false);
        isActive = false;
    }
}
