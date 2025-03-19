using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BasicMazeCell is a concrete implementation of the MazeCell interface
// It represents a basic cell in a maze, with a floor and edges
public class BasicMazeCell : AbstractMazeCell
{
    private GameObject floorPrefab;

    public BasicMazeCell(int x, int y, int sizeX, int sizeY, GameObject floorPrefab) : base(x, y, sizeX, sizeY)
    {
        this.floorPrefab = floorPrefab;
    }

    public override GameObject Build()
    {
        GameObject cellObject = new GameObject($"Cell_{x_position}_{y_position}");
        cellObject.transform.position = new Vector3(x_position * size.x, 0, y_position * size.y);

        GameObject floor = Object.Instantiate(floorPrefab, cellObject.transform);
        floor.transform.localScale = new Vector3(size.x, floor.transform.localScale.y, size.y);

        return cellObject;
    }
}
