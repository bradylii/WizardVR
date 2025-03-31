using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MazeEdgeType is an enum representing the different types of edges in a maze
public enum MazeEdgeType {
    Wall,
    Passage,
    Doorway
}

// MazeEdge represents an edge between two cells in a maze
public class MazeEdge
{
    public MazeCell cellOne, cellTwo;

    public MazeEdgeType edgeType;

    public MazeEdge(MazeCell one, MazeCell two, MazeEdgeType type = MazeEdgeType.Wall)
    {
        cellOne = one;
        cellTwo = two;
        edgeType = type;
    }
}
 