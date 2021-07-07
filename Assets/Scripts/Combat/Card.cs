using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card")]
public class Card : ScriptableObject
{
    public int value;
    public string cardName;
    public string display;

    public Sprite cardFront;
    public Sprite cardBack;
    public Sprite suit;
}
