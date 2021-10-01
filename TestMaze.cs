using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MazeGenerator;
public class TestMaze : MonoBehaviour
{
    [Header("righe e colonne del maze")]
    public int rows=2;
    public int columns=2;
    [Header("Moltiplicatore dimensione dei tiles")]
    public float scalarTile=1;
    private Vector3 startPosition = new Vector3();
    private Transform parentMaze;
    [Header("Tile strada dritta")]
    public GameObject tileStraightStreet;
    [Header("Tile strada dritta chiusa")]
    public GameObject tileStraightClosedStreet;
    [Header("Tile strada incrocio a T")]
    public GameObject tileTStreet; 
    [Header("Tile strada curva")]
    public GameObject tileCurveStreet; 
    [Header("Tile strada incrocio")]
    public GameObject tileCrossStreet; 
    MazeDFS mazeDfs;
    private void Update()
    {
        if (Input.GetKeyDown("g"))
        {
            ClearMaze();
            GenerateMaze();
        }
    }
    void Start()
    {
        parentMaze = transform;
        scalarTile = Mathf.Max(0.1f, scalarTile);
        startPosition = parentMaze.position + new Vector3(scalarTile , 0,scalarTile);
        rows = Mathf.Max(1, rows);
        columns = Mathf.Max(1, columns);
        mazeDfs = new MazeDFS(rows,columns);
        GenerateMaze();
    }
    void ClearMaze()
    {
        // Distruggi tutti i bambini
        int childs = parentMaze.childCount;
        for(int i = 0;i<childs;i++)
            Destroy(parentMaze.GetChild(i).gameObject);
    }
    void GenerateMaze()
    {
        Cell[,] grid = mazeDfs.Generate();
        if (grid == null)
        {
            Debug.Log("grid null");
            return;
        }

        for (int i = 0; i < mazeDfs.Rows; i++)
            for (int j = 0; j < mazeDfs.Cols; j++)
            {
                Cell c = grid[j, i];
                int n = c.CountWalls();
                switch (n)
                {
                    case 3:
                        ThreeWalls(c);
                        break;
                    case 2:
                        TwoWalls(c);
                        break;
                    case 1:
                        OneWall(c);
                        break;
                    case 0:
                        FourWalls(c);
                        break;
                }
            }
    }
    void ThreeWalls(Cell c)
    {
        Vector3 position = startPosition + new Vector3(c.GetX, 0, c.GetY)* scalarTile * 2;
        GameObject go = Instantiate(tileStraightClosedStreet, position, Quaternion.identity,parentMaze);
        
        if ((!c.WallLeft || !c.WallRight))
        {
           go.transform.Rotate(Vector3.up, 90);
        }

        go.transform.localScale *= scalarTile;
    }
    void TwoWalls(Cell c)
    {
        Vector3 position = startPosition + new Vector3(c.GetX, 0, c.GetY)* scalarTile * 2;
        
        GameObject go;
        if((!c.WallLeft && !c.WallRight) || ((!c.WallUp && !c.WallDown)))
        {
            go = Instantiate(tileStraightStreet, position, Quaternion.identity,parentMaze);
            if((!c.WallLeft && !c.WallRight))
                go.transform.Rotate(Vector3.up, 90);

            go.transform.localScale *= scalarTile;
            return;
        }
        go = Instantiate(tileCurveStreet, position, Quaternion.identity,parentMaze);
        
        
        if (!c.WallUp && !c.WallRight)
            go.transform.Rotate(Vector3.up, 90);
        if (!c.WallDown && !c.WallRight)
            go.transform.Rotate(Vector3.up, 180);
        if (!c.WallLeft && !c.WallDown)
            go.transform.Rotate(Vector3.up, 270);

        go.transform.localScale *= scalarTile;
    }
    void OneWall(Cell c)
    {
        Vector3 position = startPosition + new Vector3(c.GetX, 0, c.GetY) * scalarTile * 2;
        
        GameObject go = Instantiate(tileTStreet, position, Quaternion.identity,parentMaze);

        go.transform.Rotate(Vector3.up, 180);
        if (!c.WallUp && !c.WallRight &&!c.WallDown)
            go.transform.Rotate(Vector3.up, 90);
        else if (!c.WallRight && !c.WallDown && !c.WallLeft)
            go.transform.Rotate(Vector3.up, 180);
        else if (!c.WallDown && !c.WallLeft && !c.WallUp )
            go.transform.Rotate(Vector3.up, 270);

        go.transform.localScale *= scalarTile;
    }
    void FourWalls(Cell c)
    {
        Vector3 position = startPosition + new Vector3(c.GetX, 0, c.GetY) * scalarTile * 2;
        
        GameObject go = Instantiate(tileCrossStreet, position, Quaternion.identity,parentMaze);
        
        go.transform.localScale *= scalarTile;
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(parentMaze.position, scalarTile/2);
    }
}
