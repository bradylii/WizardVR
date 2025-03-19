using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AbstractMazeCell is an abstract class that implements the MazeCell interface.
public abstract class AbstractMazeCell : MazeCell
{
    protected int x_position;
    protected int y_position;

    protected IntVect2 coordinates;
    protected IntVect2 size;

    // Dictionary that maps a direction to an edge
    protected Dictionary<MazeDirection, MazeEdge> edges;

    // Constructor for AbstractMazeCell that takes in the x and y position of the cell, as well as the size of the cell
    public AbstractMazeCell(int x, int y, int sizeX, int sizeY)
    {
        this.x_position = x;
        this.y_position = y;
        this.size = new IntVect2(sizeX, sizeY);

        this.coordinates = new IntVect2(x, y);
        this.edges = new Dictionary<MazeDirection, MazeEdge>();
    }

    public abstract GameObject Build();

    public void AddEdge(MazeDirection direction, MazeEdge edge)
    {
        edges.Add(direction, edge);
    }

    public void DestroyEdge(MazeDirection direction)
    {
        if (edges.ContainsKey(direction))
        {
            edges.Remove(direction);
        }
    }

    public MazeEdge GetEdge(MazeDirection direction)
    {
        if (edges.ContainsKey(direction))
        {
            return edges[direction];
        }
        return default(MazeEdge);
    }

    public void DestroyEdge(MazeCell cell) {
        MazeDirection direction = IntVect2.GetDirection(coordinates, cell.GetCoordinates());

        if (edges.ContainsKey(direction)) {
            edges.Remove(direction);
        }
    }

    public IntVect2 GetCoordinates() {
        return coordinates;
    }

    public IntVect2 GetSize() {
        return size;
    }

    public int GetX() {
        return x_position;
    }

    public int GetY() {
        return y_position;
    }

    public int GetSizeX() {
        return size.x;
    }

    public int GetSizeY() {
        return size.y;
    }

    public override string ToString() {
        string contents = "";
        foreach (KeyValuePair<MazeDirection, MazeEdge> edge in edges) {
            contents += edge.Key + " -> " + edge.Value + "\n";
        }
        
        contents = contents.TrimEnd('\n');
        return $"Cell at ({x_position}, {y_position})\n{contents}";
    }
}
