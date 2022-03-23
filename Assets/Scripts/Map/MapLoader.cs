using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class MapLoader : MonoBehaviour
{
    MapGenerator mapGen;

    Dictionary<GameObject, string> futureConnections;
    Dictionary<GameObject, List<GameObject>> connections;
    public List<List<GameObject>> layers;
    List<GameObject> currentLayer;
    List<GameObject> allNodes;

    private void Awake()
    {
        mapGen = FindObjectOfType<MapGenerator>();
        futureConnections = new Dictionary<GameObject, string>();
        layers = new List<List<GameObject>>();
        currentLayer = new List<GameObject>();
        connections = new Dictionary<GameObject, List<GameObject>>();
        allNodes = new List<GameObject>();
    }

    public Dictionary<GameObject, List<GameObject>> LoadMap()
    {
        Debug.Log("File Found");
        string[] map = Initialize();
        GameObject start = Instantiate(mapGen.placeholderNode, mapGen.startPos, Quaternion.identity);
        start.transform.parent = mapGen.nodes.transform;
        layers.Add(new List<GameObject>() { start });
        allNodes.Add(start);
        foreach (string row in map)
            CreateRow(row);
        GameObject end = Instantiate(mapGen.placeholderNode, mapGen.stopPos, Quaternion.identity);
        end.transform.parent = mapGen.nodes.transform;
        layers.Add(new List<GameObject>() { end });
        allNodes.Add(end);
        AddConnections(start, end);
        DrawLines();

        return connections;
    }

    private string[] Initialize()
    {
        layers.Clear();
        futureConnections.Clear();
        connections.Clear();
        allNodes.Clear();
        return File.ReadAllLines(Application.dataPath + "/Map.txt");
    }

    private void CreateRow(string row)
    {
        currentLayer = new List<GameObject>();
        string[] nodes = row.Split('|');
        foreach (string node in nodes)
            CreateNode(node);
        layers.Add(currentLayer);
    }

    private void CreateNode(string node)
    {
        string[] data = node.Split(',');
        GameObject nodeType = GetNodeType(data[0]);
        Vector2 pos = new Vector2(float.Parse(data[2]), float.Parse(data[3])); //maybe check with try parse first?
        GameObject newNode = Instantiate(nodeType, Vector2.zero, Quaternion.identity);
        newNode.transform.parent = mapGen.nodes.transform;
        newNode.transform.localPosition = pos;
        currentLayer.Add(newNode);
        futureConnections.Add(newNode, data[1]);
        newNode.name = "node: " + allNodes.Count;
        allNodes.Add(newNode);
    }

    private GameObject GetNodeType(string type)
    {
        switch (type)
        {
            case "F":
                return mapGen.fightNode;
            case "I":
                return mapGen.itemNode;
            case "E":
                return mapGen.eventNode;
            default:
                return mapGen.fightNode;
        }
    }

    private void AddConnections(GameObject start, GameObject end)
    {
        connections.Add(start, layers[1]); //Connect start node to every node in first layer

        for (int i = 1; i < layers.Count - 2; i++)
        {
            Debug.Log("layer: " + i);
            foreach (GameObject node in layers[i])
            {
                List<GameObject> currentConnections = new List<GameObject>();
                string connect = futureConnections[node];
                for (int j = 0; j < connect.Length; j++)
                {
                    int index = Int32.Parse(connect[j].ToString());
                    currentConnections.Add(layers[i + 1][index]);
                }
                connections.Add(node, currentConnections);
            }
        }

        foreach (GameObject node in layers[layers.Count - 2])
        {
            connections.Add(node, new List<GameObject>() { end });
        }
    }

    public void DrawLines()
    {
        foreach (KeyValuePair<GameObject, List<GameObject>> connects in connections)
        {
            foreach (GameObject node in connects.Value)
            {
                GameObject newLine = Instantiate(mapGen.line, Vector2.zero, Quaternion.identity);
                newLine.GetComponent<UpdateLines>().SetLineTargets(connects.Key, node);
                newLine.transform.parent = mapGen.lines.transform;
            }
        }
    }
}
