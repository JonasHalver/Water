using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Noise : MonoBehaviour
{
    [Range(0,5)]
    public int smoothness;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Node n in Water.grid)
        {
            if (!TooClose(n))
            {
                n.vertexObj.transform.position = Vector3.Lerp(n.vertexObj.transform.position, AvgPos(n), 2f * Time.deltaTime);
            }

            n.worldPos = n.vertexObj.transform.position;
        }
    }

    public bool TooClose(Node n)
    {
        bool b = false;
        if (Vector3.Distance(n.worldPos, Water.grid[n.indexX, Mathf.Max(0, n.indexY - 1)].worldPos) < Water.nodeDiameter/2)
        {
            b = true;
        }
        else if (Vector3.Distance(n.worldPos, Water.grid[n.indexX, Mathf.Min(Water.gridSizeY-1, n.indexY + 1)].worldPos) < Water.nodeDiameter/2)
        {
            b = true;
        }
        else if (Vector3.Distance(n.worldPos, Water.grid[Mathf.Max(0, n.indexX - 1), n.indexY].worldPos) < Water.nodeDiameter/2)
        {
            b = true;
        }
        else if (Vector3.Distance(n.worldPos, Water.grid[Mathf.Min(Water.gridSizeX-1, n.indexX + 1), n.indexY].worldPos) < Water.nodeDiameter/2)
        {
            b = true;
        }

        return b;
    }

    public Vector3 AvgPos (Node node)
    {
        Vector3 totalPos = node.vertexObj.transform.position;
        List<Node> nearbyNodes = new List<Node>();
        for (int i = 1; i < smoothness; i++)
        {
            nearbyNodes.Add(Water.grid[node.indexX, Mathf.Max(0, node.indexY - i)]);
            nearbyNodes.Add(Water.grid[node.indexX, Mathf.Min(Water.gridSizeY - 1, node.indexY + i)]);

            nearbyNodes.Add(Water.grid[Mathf.Min(Water.gridSizeX - 1, node.indexX + i), Mathf.Min(Water.gridSizeY - 1, node.indexY + i)]);
            nearbyNodes.Add(Water.grid[Mathf.Min(Water.gridSizeX - 1, node.indexX + i), Mathf.Max(0, node.indexY - i)]);
            nearbyNodes.Add(Water.grid[Mathf.Max(0, node.indexX - i), Mathf.Min(Water.gridSizeY - 1, node.indexY + i)]);
            nearbyNodes.Add(Water.grid[Mathf.Max(0, node.indexX - i), Mathf.Max(0, node.indexY - i)]);

            nearbyNodes.Add(Water.grid[Mathf.Max(0, node.indexX - i), node.indexY]);
            nearbyNodes.Add(Water.grid[Mathf.Min(Water.gridSizeX - 1, node.indexX + i), node.indexY]);
        }

        foreach (Node n in nearbyNodes)
        {
            totalPos += n.worldPos;
        }

        return totalPos / (nearbyNodes.Count + 1);
    }
}
