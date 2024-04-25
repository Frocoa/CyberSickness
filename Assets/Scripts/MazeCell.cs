using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell
{
    public bool Visited;
    public int x, y;

    public bool TopWall;
    public bool LeftWall;

    public Vector2Int position => new(x, y);

    public MazeCell(int x, int y)
    {
        this.x = x;
        this.y = y;

        Visited = false;

        TopWall = LeftWall = true;
    }
}