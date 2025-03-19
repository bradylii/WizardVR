using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This struct is used to represent a 2D integer vector
// Used for representing coordinates in the maze and for representing the size of the maze
public struct IntVect2
{
    public int x, y;

    public IntVect2 (int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static IntVect2 operator +(IntVect2 a, IntVect2 b)
    {
        a.x += b.x;
        a.y += b.y;
        return a;
    }

    public static IntVect2 operator -(IntVect2 a, IntVect2 b)
    {
        a.x -= b.x;
        a.y -= b.y;
        return a;
    }

    // Returns the direction from a to b
    public static MazeDirection GetDirection(IntVect2 a, IntVect2 b)
    {
        IntVect2 diff = a - b;
        if (diff.x == 1 && diff.y == 0) return MazeDirection.West;
        if (diff.x == -1 && diff.y == 0) return MazeDirection.East;
        if (diff.x == 0 && diff.y == 1) return MazeDirection.North;
        if (diff.x == 0 && diff.y == -1) return MazeDirection.South;

        return MazeDirection.None; // No valid direction found
    }
}
