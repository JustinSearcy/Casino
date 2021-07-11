using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateLines : MonoBehaviour
{
    private GameObject startObject;
    private GameObject endObject;
    private LineRenderer lr;

    public void SetLineTargets(GameObject start, GameObject end)
    {
        startObject = start;
        endObject = end;
        lr = this.GetComponent<LineRenderer>();
        SetInitialPos();
    }

    private void SetInitialPos()
    {
        LineRenderer lr = this.GetComponent<LineRenderer>();
        Vector3[] positions = { new Vector3(startObject.transform.position.x, startObject.transform.position.y, 0),
                        new Vector3(endObject.transform.position.x, endObject.transform.position.y, 0)};
        lr.SetPositions(positions);
    }

    void Update()
    {
        Vector3[] positions = { new Vector3(startObject.transform.position.x, startObject.transform.position.y, 0),
                        new Vector3(endObject.transform.position.x, endObject.transform.position.y, 0)};
        lr.SetPositions(positions);
    }
}
