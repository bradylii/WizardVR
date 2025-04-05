using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MazeCell represents a cell in a maze
public interface MazeCell
{
    // Build the cell and return the GameObject representing it
    GameObject Build();

    // Add an edge to the cell in the specified direction
    void AddEdge(MazeDirection direction, MazeEdge edge);

    // Destroy the edge in the specified direction
    void DestroyEdge(MazeDirection direction);

    // Get the edge in the specified direction
    MazeEdge GetEdge(MazeDirection direction);

    // Destroy the edge between this cell and the specified cell
    void DestroyEdge(MazeCell cell);

    // Get the coordinates of the cell
    IntVect2 GetCoordinates();

    // Get the x position of the cell
    int GetX();

    // Get the y position of the cell
    int GetY();

    // Get the size of the cell in the x direction
    int GetSizeX();

    // Get the size of the cell in the y direction
    int GetSizeY();

    // Get the size of the cell
    IntVect2 GetSize();

    // Get the related GameObject
    GameObject GetGameObject();

    // Get the string representation of the cell
    string ToString();
}
