using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSelectLine : MonoBehaviour
{
    [SerializeField] Transform currentTarget = null;
    [SerializeField] GameObject point = null;
    [SerializeField] GameObject[] points = null;
    [SerializeField] int numPoints = 20;
    [SerializeField] bool lineActive = false;
    [SerializeField] float curvePointDistance = 1.5f; 

    private GameObject line;
    private float stepSize;

    private void Start()
    {
        line = gameObject.transform.GetChild(0).gameObject;
        points = new GameObject[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            points[i] = Instantiate(point, transform.position, Quaternion.identity);
            points[i].transform.parent = line.transform;
        }
        line.SetActive(false);
    }

    private void Update()
    {
        if (lineActive)
        {
            for (int i = 0; i < numPoints; i++)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                stepSize = i / (float)numPoints;
                Vector2 curvePoint = CalculateCurvePoint(mousePos);
                points[i].transform.position = CalculateQuadraticBezierPoint(stepSize, currentTarget.position, curvePoint, mousePos);
            }
        }
    }

    private Vector2 CalculateCurvePoint(Vector2 mousePos)
    {
        Vector2 middle = (mousePos + new Vector2(currentTarget.position.x, currentTarget.position.y)) / 2;
        Vector2 perp = Vector2.Perpendicular(middle);
        return perp * curvePointDistance;
    }

    private Vector2 CalculateQuadraticBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector2 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }

    public void setNewTarget(Transform target) {
        currentTarget = target;
        lineActive = true;
        line.SetActive(true);
    }

    public void deactivateLine()
    {
        lineActive = false;
        line.SetActive(false);
    }
}
