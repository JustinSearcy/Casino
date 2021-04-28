using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelect : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("New Target Selected");
        FindObjectOfType<CombatManager>().SetTarget(this.gameObject);
    }
}
