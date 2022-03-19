using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using System;

public class MapGeneration : MonoBehaviour
{
    [Header("Main Path Options")]
    [SerializeField] int minPathOptions = 3;
    [SerializeField] int maxPathOptions = 5;
    [SerializeField] int pathLength = 10;

    [Header("Path Display")]
    [SerializeField] public Vector2 startPos = new Vector2(0f, -4.5f);
    [SerializeField] public Vector2 stopPos = new Vector2(0f, 4.5f);
    [SerializeField] float yPathMax = -3f;
    [SerializeField] float yPathMin = 3f;
    [SerializeField] float xPathMin = -6;
    [SerializeField] float xPathMax = 6;
    [SerializeField] float xMaxOffset = 0.8f;
    [SerializeField] float yMaxOffset = 0.5f;

    [Header("Path Line")]
    [SerializeField] public GameObject line = null;
    [SerializeField] public GameObject lines = null;
    [SerializeField] float pathSplitChance = 0.2f;


    [Header("Nodes")]
    [SerializeField] public GameObject placeholderNode = null;
    [SerializeField] public GameObject fightNode = null;
    [SerializeField] public GameObject eventNode = null;
    [SerializeField] public GameObject itemNode = null;
    [SerializeField] public GameObject nodes = null;

    [Header("Debug")]
    [SerializeField] Dictionary<int, List<GameObject>> layers;
    [SerializeField] Dictionary<GameObject, List<GameObject>> connections;
    [SerializeField] List<GameObject> currentLayerList = null;
    [SerializeField] List<GameObject> currentConnections = null;
    [SerializeField] List<GameObject> allNodes = null;

    private List<GameObject> nextLayer;
    private List<GameObject> currentLayer;
    private List<GameObject> nodeConnections;

    string path;

    void Start()
    {
        GenerateMap();
    }

   private void GenerateMap()
    {
        path = Application.dataPath + "/Map.txt";
        File.WriteAllText(path, ""); //Overide any existing text
        layers = new Dictionary<int, List<GameObject>>();
        connections = new Dictionary<GameObject, List<GameObject>>();
        GameObject start = Instantiate(placeholderNode, startPos, Quaternion.identity);
        int nodeCount = 1;
        int layerCount = 0;
        start.transform.parent = nodes.transform;
        start.name = "start";
        allNodes.Add(start);
        layers.Add(layerCount, new List<GameObject>() { start });
        layerCount++;

        for (int i = 0; i < pathLength; i++)
        {
            int rowOptions = UnityEngine.Random.Range(minPathOptions, maxPathOptions + 1);
            float xStepSize = (Mathf.Abs(xPathMin) + Mathf.Abs(xPathMax)) / (rowOptions - 1);
            float yStepSize = (Mathf.Abs(yPathMin) + Mathf.Abs(yPathMax)) / (pathLength - 1);
            float yPos = yPathMin + (yStepSize * i);

            for (int j = 0; j < rowOptions; j++)
            {
                float xPos = xPathMin + (xStepSize * j);
                float xOffset = UnityEngine.Random.Range(-xMaxOffset, xMaxOffset);
                float yOffset = UnityEngine.Random.Range(-yMaxOffset, yMaxOffset);
                GameObject node = DetermineNode(i);
                GameObject newNode = Instantiate(node, new Vector2(xPos + xOffset, yPos + yOffset), Quaternion.identity);
                newNode.transform.parent = nodes.transform;
                newNode.name = "node: " + nodeCount;
                nodeCount++;
                currentLayerList.Add(newNode);
                allNodes.Add(newNode);
            }

            layers.Add(layerCount, new List<GameObject>(currentLayerList));
            currentLayerList.Clear();
            layerCount++;
        }

        GameObject stop = Instantiate(placeholderNode, stopPos, Quaternion.identity);
        stop.transform.parent = nodes.transform;
        stop.name = "stop";
        allNodes.Add(stop);
        layers.Add(layerCount, new List<GameObject>() { stop });

        AddConnections();
   }

    private GameObject DetermineNode(int row) //Also change based on level, determine that later
    {
        float rand = UnityEngine.Random.Range(0, 1f);
        if (row == 0)
            return fightNode;
        else if(row > 0) //Change up when more nodes
        {
            if (rand > 0.5)
                return fightNode;
            else if (rand > 0.25)
                return itemNode;
            else
                return eventNode;
        }
        return fightNode; //In case something goes wrong
    }

    private void AddConnections()
    {
        for(int i = 0; i < layers.Count - 1; i++)
        {
            if(layers.TryGetValue(i + 1, out nextLayer) && layers.TryGetValue(i, out currentLayer))
            {
                int currentCount = currentLayer.Count;
                int nextCount = nextLayer.Count;

                if (i == 0)
                    ConnectFirstLayer();
                else if(i == layers.Count - 2)
                    ConnectFinalLayer();
                else
                {
                    string mapRow = "";
                    
                    for (int j = 0; j < currentCount; j++)
                    {
                        mapRow += GetNodeType(currentLayer[j]) + ",";
                        if (currentCount == nextCount)
                            mapRow += SameSizeConnection(j, currentCount);
                        else if (currentCount < nextCount)
                            mapRow += SmallToLargeConnection(currentCount, nextCount, j);
                        else
                            mapRow += LargeToSmallConnection(currentCount, nextCount, j);
                        connections.Add(currentLayer[j], new List<GameObject>(currentConnections));
                        currentConnections.Clear();
                        mapRow += $",{Math.Round(currentLayer[j].transform.localPosition.x, 2)},{Math.Round(currentLayer[j].transform.localPosition.y, 2)}";
                        if (j < currentCount - 1)
                            mapRow += "|";
                    }
                    mapRow += "\n";
                    File.AppendAllText(path, mapRow);
                }
            }
            currentConnections.Clear();
        }
        DrawLines();
    }

    private void ConnectFirstLayer()
    {
        foreach (GameObject node in nextLayer)
            currentConnections.Add(node);
        connections.Add(currentLayer[0], new List<GameObject>(currentConnections));
    }

    private void ConnectFinalLayer()
    {
        string mapRow = "";
        for (int i = 0; i < currentLayer.Count; i++)
        {
            connections.Add(currentLayer[i], new List<GameObject>() { allNodes[allNodes.Count - 1] });
            mapRow += $"{GetNodeType(currentLayer[i])},0,{Math.Round(currentLayer[i].transform.localPosition.x, 2)},{Math.Round(currentLayer[i].transform.localPosition.y, 2)}";
            if (i < currentLayer.Count - 1)
                mapRow += "|";
        }
        File.AppendAllText(path, mapRow);
        
    }

    private string GetNodeType(GameObject node)
    {
        switch (node.tag)
        {
            case "FightNode":
                return "F";
            case "ItemNode":
                return "I";
            case "EventNode":
                return "E";
            default:
                return "Node Not Found";
        }
    }

    private string SameSizeConnection(int j, int currentCount)
    {
        currentConnections.Add(nextLayer[j]);
        string output = j.ToString();
        if (Split())
        {
            if (j == 0)
            {
                currentConnections.Add(nextLayer[j + 1]);
                output += (j + 1).ToString();
            }
            else if (j == currentCount - 1)
            {
                currentConnections.Add(nextLayer[j - 1]);
                output += (j - 1).ToString();
            }    
            else
                output += SplitLeftOrRight(j);
        }
        return output;
    }

    private string SmallToLargeConnection(int currentCount, int nextCount, int j)
    {
        int difference = nextCount - currentCount;
        if (j == 0)
        {
            currentConnections.Add(nextLayer[0]);
            currentConnections.Add(nextLayer[1]);
            return "01";
        }
        else if (j == currentCount - 1)
        {
            currentConnections.Add(nextLayer[j + difference - 1]);
            currentConnections.Add(nextLayer[j + difference]);
            return (j + difference - 1).ToString() + (j + difference).ToString();
        }
        else
        {
            if (difference == 1)
            {
                currentConnections.Add(nextLayer[j]);
                string output = j.ToString();
                if (Split())
                    output += SplitLeftOrRight(j);
                return output;
            }
            else
            {
                currentConnections.Add(nextLayer[j + 1]);
                currentConnections.Add(nextLayer[j + 2]);
                return (j + difference - 1).ToString() + (j + difference).ToString();
            }
        }
    }

    private string LargeToSmallConnection(int currentCount, int nextCount, int j)
    {
        if (j == 0)
        {
            currentConnections.Add(nextLayer[0]);
            string output = "0";
            if (Split())
            {
                currentConnections.Add(nextLayer[1]);
                output += "1";
            }
            return output;
        }
        else if (j == currentCount - 1)
        {
            currentConnections.Add(nextLayer[nextCount - 1]);
            return (nextCount - 1).ToString();
        }
        else
        {
            currentConnections.Add(nextLayer[(j / 2) + 1]);
            return ((j / 2) + 1).ToString();
        }
            
    }

    private bool Split()
    {
        float rand = UnityEngine.Random.Range(0, 1f);
        if(rand < pathSplitChance)
            return true;
        return false;
    }

    private string SplitLeftOrRight(int j)
    {
        if (SplitDirection())
        {
            currentConnections.Add(nextLayer[j - 1]);
            return (j - 1).ToString();
        } 
        else
        {
            currentConnections.Add(nextLayer[j + 1]);
            return (j + 1).ToString();
        }
    }

    private bool SplitDirection() //Return true to split left, false to split right
    {
        float rand = UnityEngine.Random.Range(0, 1f);
        if (rand < 0.5f)
            return true;
        return false;
    }

    private void DrawLines()
    {
        for(int i = 0; i <= connections.Count; i++)
        {
            if(connections.TryGetValue(allNodes[i], out nodeConnections)){
                foreach(GameObject node in nodeConnections) { 
                    GameObject newLine = Instantiate(line, Vector2.zero, Quaternion.identity);
                    newLine.GetComponent<UpdateLines>().SetLineTargets(allNodes[i], node);
                    newLine.transform.parent = lines.transform;
                }
            }
        }
    }

    public void DeleteMap()
    {
        foreach (Transform node in nodes.transform)
            Destroy(node.gameObject);

        foreach (Transform line in lines.transform)
            Destroy(line.gameObject);

        allNodes.Clear();

        layers.Clear();
        
        connections.Clear();
    }
}
