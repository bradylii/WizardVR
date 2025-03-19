using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Generates a maze using the Kruskal algorithm
public class KruskalMaze : AbstractMaze
{
    
    public GameObject floorPrefab; // Prefab for the floor of the maze cells
    public GameObject wallPrefab; // Prefab for the walls of the maze cells

    public bool haveEntranceExit = false;

    private List<MazeEdge> walls = new List<MazeEdge>();
    private Dictionary<MazeCell, MazeCell> unions = new Dictionary<MazeCell, MazeCell>();

    private void Start()
    {
        Generate();
        Build();
    }

    // Initialize the cells of the maze
    private void InitializeCells()
    {
        for (int x = 0; x < width; x++)
        {
            cells.Add(new List<MazeCell>());
            for (int y = 0; y < height; y++)
            {
                AbstractMazeCell cell = new BasicMazeCell(x, y, 1, 1, floorPrefab);
                cells[x].Add(cell);

                // Initialize the unions dictionary
                unions[cell] = cell;  // Each cell is its own union initially
            }
        }
    }

    // Initialize the walls of the maze, making sure that each cell has walls on all sides
    // First step of the Kruskal algorithm
    private void InitializeWalls()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                MazeCell cell = cells[x][y];
                if (x < width - 1) // Check right neighbor
                {
                    walls.Add(new MazeEdge(cell, cells[x + 1][y]));
                    cell.AddEdge(MazeDirection.East, new MazeEdge(cell, cells[x + 1][y]));
                }
                else
                {
                    cell.AddEdge(MazeDirection.East, new MazeEdge(cell, null));
                }

                if (y < height - 1) // Check bottom neighbor
                {
                    walls.Add(new MazeEdge(cell, cells[x][y + 1]));
                    cell.AddEdge(MazeDirection.South, new MazeEdge(cell, cells[x][y + 1]));
                }
                else
                {
                    cell.AddEdge(MazeDirection.South, new MazeEdge(cell, null));
                }

                if (x == 0) // Left edge
                {
                    cell.AddEdge(MazeDirection.West, new MazeEdge(cell, null)); // No left neighbor
                }
                else {
                    cell.AddEdge(MazeDirection.West, GetCell(x - 1, y).GetEdge(MazeDirection.East));
                }

                if (y == 0) // Top edge
                {
                    cell.AddEdge(MazeDirection.North, new MazeEdge(cell, null)); // No top neighbor
                }
                else {
                    cell.AddEdge(MazeDirection.North, GetCell(x, y - 1).GetEdge(MazeDirection.South));
                }
            }
        }
    }

    // Find the representative of a set
    private MazeCell FindRepresentative(MazeCell cell)
    {
        if (unions[cell] != cell)
        {
            unions[cell] = FindRepresentative(unions[cell]);
        }
        return unions[cell];
    }

    // Union two sets
    private void Union(MazeCell cellA, MazeCell cellB)
    {
        MazeCell repA = FindRepresentative(cellA);
        MazeCell repB = FindRepresentative(cellB);

        if (repA != repB)
        {
            unions[repA] = repB; // Union the two sets
        }
    }

    // Generate the maze using the Kruskal algorithm
    public override void Generate()
    {
        InitializeCells();
        InitializeWalls();

        // Shuffle walls to randomize the maze generation
        var rand = new System.Random();
        walls = walls.OrderBy(_ => rand.Next()).ToList();

        foreach (MazeEdge wall in walls)
        {
            MazeCell cellA = wall.cellOne;
            MazeCell cellB = wall.cellTwo;

            if (FindRepresentative(cellA) != FindRepresentative(cellB))
            {
                // Remove the wall between the two cells
                cellA.DestroyEdge(cellB);
                cellB.DestroyEdge(cellA);

                MazeDirection directionAToB = IntVect2.GetDirection(cellA.GetCoordinates(), cellB.GetCoordinates());
                MazeDirection directionBToA = IntVect2.GetDirection(cellB.GetCoordinates(), cellA.GetCoordinates());    

                cellA.AddEdge(directionAToB, new MazeEdge(cellA, cellB, MazeEdgeType.Passage));
                cellB.AddEdge(directionBToA, new MazeEdge(cellB, cellA, MazeEdgeType.Passage));

                // Union the two cells
                Union(cellA, cellB);
            }
        }

        // Add entrance and exit to the maze if needed
        if (haveEntranceExit)
        {
            MazeCell entrance = GetCell(0, 0);
            MazeCell exit = GetCell(width - 1, height - 1);

            entrance.DestroyEdge(MazeDirection.West);
            entrance.AddEdge(MazeDirection.West, new MazeEdge(entrance, null, MazeEdgeType.Passage));

            exit.DestroyEdge(MazeDirection.East);
            exit.AddEdge(MazeDirection.East, new MazeEdge(exit, null, MazeEdgeType.Passage));
        }
    }

    public override GameObject Build()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                MazeCell cell = cells[x][y];
                GameObject cellGO = cell.Build();
                cellGO.transform.parent = transform;

                // Build the right and bottom edges of the maze for each cell
                // Also build the walls if cell possesses outer walls of the maze
                // Can most likely be refactored to be more efficient and less repetitive
                GameObject edge = BuildEdge(cell.GetEdge(MazeDirection.East));
                if (edge != null)
                {
                    edge.transform.parent = cellGO.transform;
                    edge.transform.localPosition = new Vector3(0.5f, 0, 0);
                    edge.transform.Rotate(0, 90, 0);
                    
                }
  
                edge = BuildEdge(cell.GetEdge(MazeDirection.South));
                if (edge != null)
                {
                    edge.transform.parent = cellGO.transform;
                    edge.transform.localPosition = new Vector3(0, 0, 0.5f);
                }

                if(x == 0)
                {
                    edge = BuildEdge(cell.GetEdge(MazeDirection.West));
                    if (edge != null)
                    {
                        edge.transform.parent = cellGO.transform;
                        edge.transform.localPosition = new Vector3(-0.5f, 0, 0);
                        edge.transform.Rotate(0, -90, 0);
                    }
                }

                if (y == 0)
                {
                    edge = BuildEdge(cell.GetEdge(MazeDirection.North));
                    if (edge != null)
                    {
                        edge.transform.parent = cellGO.transform;
                        edge.transform.localPosition = new Vector3(0, 0, -0.5f);
                    }
                }
            }
        }

        return gameObject;
    }

    // Build the edge of a cell
    // TODO: Add support for different edge types
    private GameObject BuildEdge(MazeEdge edge)
    {
        if (edge.edgeType == MazeEdgeType.Wall)
        {
            return Instantiate(wallPrefab);
        }
        else
        {
            return null;
        }
    }
}
