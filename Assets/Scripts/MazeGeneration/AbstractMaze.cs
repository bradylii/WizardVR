using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AbstractMaze is an abstract class that implements the MazeWorld interface. 
// It provides a basic implementation of the GetCell, GetHeight, and GetWidth methods. 
// It also provides a List of Lists of MazeCells to store the cells of the maze. 
// The Generate and Build methods are declared as abstract, so they must be implemented by subclasses of AbstractMaze.
public abstract class AbstractMaze : MonoBehaviour, MazeWorld
{
    [SerializeField]
    protected int width = 10;

    [SerializeField]
    protected int height = 10;

    [SerializeField]
    protected int cellSize = 1;

    protected List<List<MazeCell>> cells = new List<List<MazeCell>>();

    public abstract void Generate();
    public abstract GameObject Build();

    public MazeCell GetCell(int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height)
        {
            return null;
        }
        else
        {
            return cells[x][y];
        }
    }


    public int GetHeight()
    {
        return height;
    }

    public int GetWidth()
    {
        return width;
    }

    public MazeCell GetPositionInMaze(float x, float z)
    {
        int relativePosX = Mathf.FloorToInt(x / cellSize);
        int relativePosY = Mathf.FloorToInt(z / cellSize); // Z if top-down 3D

        return cells[relativePosX][relativePosY];
    }
}
