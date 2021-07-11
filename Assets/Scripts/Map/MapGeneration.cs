using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    [Header("Main Path Options")]
    [SerializeField] int minPathOptions = 3;
    [SerializeField] int maxPathOptions = 5;
    [SerializeField] int pathLength = 10;

    [Header("Path Display")]
    [SerializeField] Vector2 startPos = new Vector2(0f, -4.5f);
    [SerializeField] Vector2 stopPos = new Vector2(0f, 4.5f);
    [SerializeField] float yPathMax = -3f;
    [SerializeField] float yPathMin = 3f;
    [SerializeField] float xPathMin = -6;
    [SerializeField] float xPathMax = 6;
    [SerializeField] float xMaxOffset = 0.8f;
    [SerializeField] float yMaxOffset = 0.5f;

    [Header("Path Line")]
    [SerializeField] GameObject line = null;
    [SerializeField] GameObject lines = null;
    [SerializeField] float pathSplitChance = 0.2f;


    [Header("Misc")]
    [SerializeField] GameObject placeholderIcon = null;
    [SerializeField] GameObject nodes = null;

    [Header("Debug")]
    [SerializeField] Dictionary<int, List<GameObject>> layers;
    [SerializeField] Dictionary<GameObject, List<GameObject>> connections;
    [SerializeField] List<GameObject> currentLayerList = null;
    [SerializeField] List<GameObject> currentConnections = null;
    [SerializeField] List<GameObject> allNodes = null;

    private List<GameObject> nextLayer;
    private List<GameObject> currentLayer;
    private List<GameObject> nodeConnections;

    void Start()
    {
        GenerateMap();
    }

   private void GenerateMap()
    {
        layers = new Dictionary<int, List<GameObject>>();
        connections = new Dictionary<GameObject, List<GameObject>>();
        GameObject start = Instantiate(placeholderIcon, startPos, Quaternion.identity);
        int nodeCount = 1;
        int layerCount = 0;
        start.transform.parent = nodes.transform;
        start.name = "start";
        allNodes.Add(start);
        layers.Add(layerCount, new List<GameObject>() { start });
        layerCount++;

        for (int i = 0; i < pathLength; i++)
        {
            int rowOptions = Random.Range(minPathOptions, maxPathOptions + 1);
            float xStepSize = (Mathf.Abs(xPathMin) + Mathf.Abs(xPathMax)) / (rowOptions - 1);
            float yStepSize = (Mathf.Abs(yPathMin) + Mathf.Abs(yPathMax)) / (pathLength - 1);
            float yPos = yPathMin + (yStepSize * i);

            for (int j = 0; j < rowOptions; j++)
            {
                float xPos = xPathMin + (xStepSize * j);
                float xOffset = Random.Range(-xMaxOffset, xMaxOffset);
                float yOffset = Random.Range(-yMaxOffset, yMaxOffset);
                GameObject newNode = Instantiate(placeholderIcon, new Vector2(xPos + xOffset, yPos + yOffset), Quaternion.identity);
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

        GameObject stop = Instantiate(placeholderIcon, stopPos, Quaternion.identity);
        stop.transform.parent = nodes.transform;
        stop.name = "stop";
        allNodes.Add(stop);
        layers.Add(layerCount, new List<GameObject>() { stop });

        AddConnections();
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
                {
                    ConnectFirstLayer();
                }
                else if(i == layers.Count - 2)
                {
                    ConnectFinalLayer();
                }
                else
                {
                    for (int j = 0; j < currentCount; j++)
                    {
                        if (currentCount == nextCount)
                        {
                            SameSizeConnection(j, currentCount);
                        }
                        else if (currentCount < nextCount)
                        {
                            SmallToLargeConnection(currentCount, nextCount, j);
                        }
                        else
                        {
                            LargeToSmallConnection(currentCount, nextCount, j);
                        }
                        connections.Add(currentLayer[j], new List<GameObject>(currentConnections));
                        currentConnections.Clear();
                    }
                }
            }
            currentConnections.Clear();
        }
        DrawLines();
    }

    private void ConnectFirstLayer()
    {
        foreach (GameObject node in nextLayer)
        {
            currentConnections.Add(node);
        }
        connections.Add(currentLayer[0], new List<GameObject>(currentConnections));
    }

    private void ConnectFinalLayer()
    {
        foreach (GameObject node in currentLayer)
        {
            connections.Add(node, new List<GameObject>() { allNodes[allNodes.Count - 1] });
        }
    }

    private void SameSizeConnection(int j, int currentCount)
    {
        currentConnections.Add(nextLayer[j]);
        if (Split())
        {
            if (j == 0)
            {
                currentConnections.Add(nextLayer[j + 1]);
            }
            else if (j == currentCount - 1)
            {
                currentConnections.Add(nextLayer[j - 1]);
            }
            else
            {
                SplitLeftOrRight(j);
            }
        }
    }

    private void SmallToLargeConnection(int currentCount, int nextCount, int j)
    {
        int difference = nextCount - currentCount;
        if (j == 0)
        {
            currentConnections.Add(nextLayer[0]);
            currentConnections.Add(nextLayer[1]);
        }
        else if (j == currentCount - 1)
        {
            currentConnections.Add(nextLayer[j + difference - 1]);
            currentConnections.Add(nextLayer[j + difference]);
        }
        else
        {
            if (difference == 1)
            {
                currentConnections.Add(nextLayer[j]);
                if (Split())
                {
                    SplitLeftOrRight(j);
                }
            }
            else
            {
                currentConnections.Add(nextLayer[j + 1]);
                currentConnections.Add(nextLayer[j + 2]);
            }
        }
    }

    private void LargeToSmallConnection(int currentCount, int nextCount, int j)
    {
        if (j == 0)
        {
            currentConnections.Add(nextLayer[0]);
            if (Split())
            {
                currentConnections.Add(nextLayer[1]);
            }
        }
        else if (j == currentCount - 1)
        {
            currentConnections.Add(nextLayer[nextCount - 1]);
        }
        else
        {
            currentConnections.Add(nextLayer[(j / 2) + 1]);
        }
    }

    private bool Split()
    {
        float rand = Random.Range(0, 1f);
        if(rand < pathSplitChance)
        {
            return true;
        }
        return false;
    }

    private void SplitLeftOrRight(int j)
    {
        if (SplitDirection())
        {
            currentConnections.Add(nextLayer[j - 1]);
        }
        else
        {
            currentConnections.Add(nextLayer[j + 1]);
        }
    }

    private bool SplitDirection() //Return true to split left, false to split right
    {
        float rand = Random.Range(0, 1f);
        if (rand < 0.5f)
        {
            return true;
        }
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
        {
            Destroy(node.gameObject);
        }

        foreach (Transform line in lines.transform)
        {
            Destroy(line.gameObject);
        }

        allNodes.Clear();

        layers.Clear();
        
        connections.Clear();
    }
}
