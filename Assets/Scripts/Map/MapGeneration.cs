using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    [Header("Main Path Options")]
    [SerializeField] int minPathOptions = 3;
    [SerializeField] int maxPathOptions = 5;
    [SerializeField] int pathLength = 10;

    [Header("Path Display")]
    [SerializeField] Vector2 startPos = new Vector2(0f, -4.5f);
    [SerializeField] float yPathMax = -3f;
    [SerializeField] float yPathMin = 4.5f;
    [SerializeField] float xPathMin = -6;
    [SerializeField] float xPathMax = 6;
    [SerializeField] float xMaxOffset = 0.8f;
    [SerializeField] float yMaxOffset = 0.5f;

    [Header("Path Line")]
    [SerializeField] GameObject pathPoint;
    [SerializeField] GameObject pathPoints;
    [SerializeField] int numberOfPoints = 10;
    [SerializeField] float spaceBetweenPoints = 0.2f;


    [Header("Misc")]
    [SerializeField] GameObject placeholderIcon;
    [SerializeField] GameObject nodes;

    [Header("Debug")]
    [SerializeField] Dictionary<int, List<GameObject>> layers;
    [SerializeField] Dictionary<GameObject, List<GameObject>> connections;
    [SerializeField] List<GameObject> currentLayer;

    private List<GameObject> nextLayer;

    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        layers = new Dictionary<int, List<GameObject>>();
        GameObject start = Instantiate(placeholderIcon, startPos, Quaternion.identity);
        start.transform.parent = nodes.transform;
        start.name = "start";
        currentLayer.Add(start);
        layers.Add(0, currentLayer);
        currentLayer.Clear();

        for (int i = 0; i < pathLength; i++)
        {
            int rowOptions = Random.Range(minPathOptions, maxPathOptions);
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
                currentLayer.Add(newNode);
            }

            layers.Add(i + 1, currentLayer);
            currentLayer.Clear();
        }

        AddConnections();
    }

    private void AddConnections()
    {
        for(int i = 0; i < layers.Count; i++)
        {
            if(layers.TryGetValue(i + 1, out nextLayer))
            {
                if (i == 0)
                {
                    foreach (GameObject node in nextLayer)
                    {
                        currentLayer.Add(node);
                    }
                    
                }
            }
        }

        DrawLines();
    }

    private void DrawLines()
    {

    }

    public void DeleteMap()
    {
        foreach (Transform node in nodes.transform)
        {
            Destroy(node.gameObject);
        }

        layers.Clear();
        //connections.Clear();
    }
}
