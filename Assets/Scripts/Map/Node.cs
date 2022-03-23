using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] Vector2 hoverSize = new Vector2(1.2f, 1.2f);
    [SerializeField] Vector2 initSize = new Vector2(1f, 1f);
    [SerializeField] float sizeChangeTime = 0.25f;

    MapManager manager;

    void Start()
    {
        manager = FindObjectOfType<MapManager>();
    }

    private void OnMouseDown()
    {
        
    }

    private void OnMouseEnter()
    {
        if (manager.ValidNextNode(gameObject))
        {
            LeanTween.scale(gameObject, hoverSize, sizeChangeTime).setEaseOutBack();
        }
    }

    private void OnMouseExit()
    {
        if (manager.ValidNextNode(gameObject))
        {
            LeanTween.scale(gameObject, initSize, sizeChangeTime).setEaseOutBack();
        }
    }
}
