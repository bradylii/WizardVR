using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MazeWorld is an interface representing a maze
public interface MazeWorld
{
    // Get the width of the maze
    int GetWidth();
    // Get the height of the maze
    int GetHeight();
    // Generate the maze
    void Generate();
    // Build the maze and return the GameObject representing it
    GameObject Build();
    // Get the cell at the specified coordinates
    MazeCell GetCell(int x, int y);

    MazeCell GetPositionInMaze(float x, float z);
    
}
