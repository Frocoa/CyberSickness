using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeGenerator : MonoBehaviour
{
    [Range(5, 50)] public int width = 5, height = 5;
    public int startX, startY;

    private MazeCell[,] maze;

    private Vector2Int currentCell;

    public MazeCell[,] GetMaze()
    {
        maze = new MazeCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = new MazeCell(x, y);
            }
        }
        
        CarvePath(startX, startY);
        return maze;
    }

    private List<Direction> directions = new List<Direction>()
    {
        Direction.Up, Direction.Down, Direction.Left, Direction.Right
    };

    List<Direction> GetRandomDirections()
    {
        List<Direction> newDirection = new List<Direction>(this.directions);
        List<Direction> randomDirection = new List<Direction>();

        while (newDirection.Count > 0)
        {
            int random = Random.Range(0, newDirection.Count);
            randomDirection.Add(newDirection[random]);
            newDirection.RemoveAt(random);
        }

        return randomDirection;
    }

    private bool IsCellValid(int x, int y)
    {
        return x >= 0 && y >= 0 && x <= width - 1 && y <= height - 1 && !maze[x, y].Visited;
    }

    Vector2Int CheckNeighbour()
    {
        List<Direction> randomDirections = GetRandomDirections();

        foreach (Direction direction in randomDirections)
        {
            Vector2Int neighbour = currentCell;
            switch (direction)
            {
                case Direction.Up:
                    neighbour.y++;
                    break;
                case Direction.Down:
                    neighbour.y--;
                    break;
                case Direction.Left:
                    neighbour.x--;
                    break;
                case Direction.Right:
                    neighbour.x++;
                    break;
            }

            if (IsCellValid(neighbour.x, neighbour.y)) return neighbour;
        }

        return currentCell;
    }

    private void BreakWalls(Vector2Int primaryCell, Vector2Int secondaryCell)
    {
        if (primaryCell.x > secondaryCell.x)
        {
            maze[primaryCell.x, primaryCell.y].LeftWall = false;
        } else if (primaryCell.x < secondaryCell.x)
        {
            maze[secondaryCell.x, secondaryCell.y].LeftWall = false;
        }
        if (primaryCell.y > secondaryCell.y)
        {
            maze[primaryCell.x, primaryCell.y].TopWall = false;
        } else if (primaryCell.y < secondaryCell.y)
        {
            maze[secondaryCell.x, secondaryCell.y].TopWall = false;
        }
    }

    private void CarvePath(int x, int y)
    {
        if (x < 0 || y < 0 || x > width - 1 || y > height - 1)
        {
            x = y = 0;
            Debug.LogWarning("Starting position is out of bound, defaulting to 0, 0");
        }

        currentCell = new Vector2Int(x, y);
        List<Vector2Int> path = new List<Vector2Int>();

        bool deadEnd = false;
        while (!deadEnd)
        {
            Vector2Int nextCell = CheckNeighbour();

            if (nextCell == currentCell)
            {
                for (int i = path.Count - 1; i >= 0 ; i--)
                {
                    currentCell = path[i];
                    path.RemoveAt(i);
                    nextCell = CheckNeighbour();

                    if (nextCell != currentCell) break;
                }
                
                if (nextCell == currentCell)
                {
                    deadEnd = true;
                }
            }
            else
            {
                BreakWalls(currentCell, nextCell);
                maze[currentCell.x, currentCell.y].Visited = true;
                currentCell = nextCell;
                path.Add(currentCell);
            }
        }
        {
            
        }
    }
}


public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
