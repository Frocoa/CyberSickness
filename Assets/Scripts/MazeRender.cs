using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRender : MonoBehaviour
{
    [SerializeField] private MazeGenerator mazeGenerator;
    [SerializeField] private GameObject mazeCellPrefab;

    public float CellSize = 1;

    private void Start()
    {
        MazeCell[,] maze = mazeGenerator.GetMaze();

        for (int x = 0; x < mazeGenerator.width; x++)
        {
            for (int y = 0;
                 y < mazeGenerator.height;
                 y++)
            {
                GameObject newCell = Instantiate(mazeCellPrefab, new Vector3((float) x * CellSize,0f,  (float) y * CellSize), Quaternion.identity, mazeGenerator.transform);
                MazeCellObject mazeCell = newCell.GetComponent<MazeCellObject>();

                bool top = maze[x, y].TopWall;
                bool left = maze[x, y].LeftWall;

                bool right = false;
                bool bottom = false;
                if (x == mazeGenerator.width - 1) right = true;
                if (y == 0) bottom = true;
                
                mazeCell.Init(top, bottom, right, left);
            }
        }
    }
}
