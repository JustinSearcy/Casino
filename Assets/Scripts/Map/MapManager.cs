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
    [SerializeField] private Color32 disabledColor;

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
            UpdateDisabledNodes();
        }
        else
        {
            connections = generator.GenerateMap();
            layers = generator.layers;
            currentNode = layers[0][0];
            SetNextAvailableNodes(currentNode);
        }
    }

    private void UpdateDisabledNodes()
    {
        int currentLayer = 0;
        for (int i = 0; i < layers.Count; i++)
        {
            if (layers[i].Contains(currentNode)) {
                currentLayer = i;
                break;
            }
        }

        if (currentLayer > 0)
        {
            for (int i = 1; i <= currentLayer; i++)
            {
                foreach (GameObject node in layers[i])
                {
                    if (node != currentNode)
                        node.GetComponent<SpriteRenderer>().color = disabledColor;
                }
            }
        }
    }

    public void SelectNewNode(GameObject nextNode)
    {
        if(ValidNextNode(nextNode)) //Maybe disable selecting another node at this point
        {
            currentNode = nextNode;
            //Do not set next available for last node
            if (currentNode != layers[layers.Count - 1][0])
            {
                SetNextAvailableNodes(currentNode);
                UpdateDisabledNodes();
            }
        }
    }

    private void SetNextAvailableNodes(GameObject node)
    {
        nextAvailableNodes = connections[node];
    }

    public bool ValidNextNode(GameObject nextNode)
    {
        return nextAvailableNodes.Contains(nextNode);
    }
}
