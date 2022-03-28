using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private Vector2 hoverSize = new Vector2(1.2f, 1.2f);
    [SerializeField] private Vector2 initSize = new Vector2(1f, 1f);
    [SerializeField] private float sizeChangeTime = 0.25f;
    [SerializeField] private Color32 initColor;
    [SerializeField] private Color32 hoverColor;
    [SerializeField] public bool disabled = false;

    MapManager manager;
    SpriteRenderer sprite;

    void Start()
    {
        manager = FindObjectOfType<MapManager>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        sprite.color = initColor;
    }

    private void OnMouseDown()
    {
        manager.SelectNewNode(gameObject);
    }

    private void OnMouseEnter()
    {
        if (manager.ValidNextNode(gameObject))
        {
            LeanTween.scale(gameObject, hoverSize, sizeChangeTime).setEaseOutBack();
            sprite.color = hoverColor;
        }
    }

    private void OnMouseExit()
    {
        LeanTween.scale(gameObject, initSize, sizeChangeTime).setEaseOutBack();
        sprite.color = initColor;
    }
}
