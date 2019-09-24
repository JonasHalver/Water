using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public static Node[,] grid;

    public Vector2 gridWorldSize;

    [Range(0.5f,1f)]
    public float nodeRadius;
    public static float nodeDiameter;
    public static int gridSizeX, gridSizeY;

    public GameObject vertexObj;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uv = new List<Vector2>();

    [Range(-5,5)]
    public int tAdjust = 1;

    // Start is called before the first frame update
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        

        viewMesh = new Mesh();
        viewMesh.name = "Water Mesh";
        viewMeshFilter.mesh = viewMesh;

        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LateUpdate()
    {
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                vertices[n.vertexIndex] = n.vertexObj.transform.position;
            }
            DrawMesh();
        }
    }

    //private void OnValidate()
    //{
    //    nodeDiameter = nodeRadius * 2;
    //    gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
    //    gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
    //
    //
    //    viewMesh = new Mesh();
    //    viewMesh.name = "Water Mesh";
    //    viewMeshFilter.mesh = viewMesh;
    //
    //    CreateGrid();
    //    if (grid != null)
    //    {
    //        DrawMesh();
    //    }
    //}

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            foreach (Node n in grid)
            {
                if (n.connectedN != null)
                {
                    Gizmos.DrawLine(n.worldPos, n.connectedN.worldPos);
                }
                if (n.connectedE != null)
                {
                    Gizmos.DrawLine(n.worldPos, n.connectedE.worldPos);
                }
                if (n.connectedNE != null)
                {
                    Gizmos.DrawLine(n.worldPos, n.connectedNE.worldPos);
                }
                if (n.connectedSE != null)
                {
                    Gizmos.DrawLine(n.worldPos, n.connectedSE.worldPos);
                }
            }
            
        }
    }

    public void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (x % 2 != 0)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    GameObject newVertex = Instantiate(vertexObj, worldPoint, Quaternion.Euler(0, 0, 0), transform);
                    grid[x, y] = new Node(worldPoint, newVertex, x, y);
                }
                else
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter);
                    GameObject newVertex = Instantiate(vertexObj, worldPoint, Quaternion.Euler(0, 0, 0), transform);
                    grid[x, y] = new Node(worldPoint, newVertex, x, y);
                }                
            }
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (y + 1 < gridSizeY) 
                {
                    grid[x, y].connectedN = grid[x, y + 1];
                }
                if (x + 1 < gridSizeX)
                {
                    grid[x, y].connectedE = grid[x + 1, y];
                }
                if (x % 2 != 0)
                {
                    if (x + 1 < gridSizeX && y + 1 < gridSizeY)
                    {
                        grid[x, y].connectedNE = grid[x + 1, y + 1];
                    }
                }
                else
                {
                    if (x + 1 < gridSizeX && y - 1 > -1)
                    {
                        grid[x, y].connectedSE = grid[x + 1, y - 1];
                    }
                }
                uv.Add(new Vector2(x / gridSizeX, y / gridSizeY));
            }
        }
        FindVerticesAndTriangles();
    }

    public void FindVerticesAndTriangles()
    {
        //int vertexCount = vertices.Count + 1;
        //int[] triangles = new int[(vertexCount - gridSizeX) * 3];
        
        foreach (Node n in grid)
        {
            vertices.Add(n.worldPos);
            n.nodeSize = nodeRadius;
            n.vertexIndex = vertices.Count - 1;
        }
        int t = 0;

       
        for (int x = 0; x < gridSizeX - 1; x++)
        {
            t = x * gridSizeY;
            if (x % 2 == 0)
            {
                for (int i = 0; i < (gridSizeY * 2) - 2; i++)
                {
                    if (i % 2 == 0)
                    {
                        triangles.Add(t);
                        triangles.Add(t + 1);
                        triangles.Add(t + gridSizeY);

                        if (i != (gridSizeY * 2) - 2)
                        {
                            t++;
                        }
                    }
                    else
                    {
                        triangles.Add(t);
                        triangles.Add(t + gridSizeY);
                        triangles.Add(t + gridSizeY - 1);
                    }
                }
            }
            else
            {
                t += gridSizeY - tAdjust;
                // ITERATE IN REVERSE
                for (int i = 0; i < (gridSizeY * 2) - 2; i++)
                {
                    if (i % 2 == 0)
                    {
                        triangles.Add(t);
                        triangles.Add(t + gridSizeY);
                        triangles.Add(t - 1);

                        if (i != (gridSizeY * 2) - 2)
                        {
                            t--;
                        }
                    }
                    else
                    {
                        triangles.Add(t);
                        triangles.Add(t + gridSizeY + 1);
                        triangles.Add(t + gridSizeY);
                        
                    }
                }               
            }
        }
    }

    public void DrawMesh()
    {
        viewMesh.vertices = vertices.ToArray();
        viewMesh.triangles = triangles.ToArray();
        viewMesh.uv = uv.ToArray();
        viewMesh.RecalculateNormals();        
    }
}

public class Node
{
    public Vector3 worldPos;
    public int vertexIndex;
    public GameObject vertexObj;
    public float nodeSize;
    public Node connectedN, connectedNE, connectedE, connectedSE;
    public int indexX, indexY;


    public Node(Vector3 _worldPos, GameObject _vertexObj, int _indexX, int _indexY)
    {
        worldPos = _worldPos;
        vertexObj = _vertexObj;
        indexX = _indexX;
        indexY = _indexY;
    }
}
