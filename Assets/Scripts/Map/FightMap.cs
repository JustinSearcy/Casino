using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMap : MonoBehaviour
{
    [Header("Floor 1, Level 1 - 3")]
    [SerializeField] List<GameObject> floor1Enemies1List1;
    [SerializeField] List<GameObject> floor1Enemies1List2;
    [SerializeField] List<GameObject> floor1Enemies1List3;

    [Header("Floor 1, Level 2 - 5")]
    [SerializeField] List<GameObject> floor1Enemies2List1;

    [Header("Floor 1, Level 4 - 5")]
    [SerializeField] List<GameObject> floor1Enemies3List1;

    [Header("Floor 1, Level 6 - 7")]
    [SerializeField] List<GameObject> floor1Enemies4List1;
}
