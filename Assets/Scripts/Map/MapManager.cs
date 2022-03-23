using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Dictionary<GameObject, List<GameObject>> connections;
    [SerializeField] private List<List<GameObject>> layers;
    [SerializeField] private GameObject currentNode;
    [SerializeField] private List<GameObject> nextAvailableNodes;

    MapGenerator generator;
    MapLoader loader;

    private void Awake()
    {
        generator = GetComponent<MapGenerator>();
        loader = GetComponent<MapLoader>();
    }

    void Start()
    {
        if (File.Exists(Application.dataPath + "/Map.txt"))
        {
            connections = loader.LoadMap();
            layers = loader.layers;   
            currentNode = layers[0][0]; //For now before entire system set
            SetNextAvailableNodes(currentNode); //PLEASE REMOVE LATER
        }
        else
        {
            connections = generator.GenerateMap();
            layers = generator.layers;
            currentNode = layers[0][0];
            SetNextAvailableNodes(currentNode);
        }
    }

    private void SetNextAvailableNodes(GameObject currentNode)
    {
        nextAvailableNodes = connections[currentNode];
    }

    public bool ValidNextNode(GameObject nextNode)
    {
        return nextAvailableNodes.Contains(nextNode);
    }
}
