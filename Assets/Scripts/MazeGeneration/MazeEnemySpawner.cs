using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Finish enemy spawner
public class MazeEnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private int minSpawnDistance = 2;

    [SerializeField]
    private int maxSpawnDistance = 5;

    public static int maxEnemies = 5;

    private MazeWorld maze;

    private int numEnemies = 0;

    private static bool enableSpawning = false;

    public static void EnableSpawning()
    {
        enableSpawning = true;
    }

    private void Start()
    {
        maze = GetComponent<MazeWorld>();
    }

    private void Update()
    {
        if (enableSpawning)
        {
            SpawnEnemy();
            if (numEnemies >= maxEnemies)
            {
               enableSpawning = false;
            }
        }
    }

    void SpawnEnemy()
    {
        MazeCell spawnLoc = GenerateSpawnLocation();
        if (spawnLoc != null)
        {
            Vector3 spawnGlobal = spawnLoc.GetGameObject().transform.position;
            spawnGlobal = spawnGlobal + Vector3.up;
            GameObject spawned = Instantiate(enemyPrefab, spawnGlobal, Quaternion.Euler(0, 0, 0));
            numEnemies++;
        } 
    }

    void DecrementEnemies()
    {
        numEnemies--;
    }

    MazeCell GenerateSpawnLocation()
    {
        Vector3 currentCameraPosition = Camera.main.transform.position;
        MazeCell mazePos = maze.GetPositionInMaze(currentCameraPosition.x, currentCameraPosition.z);

        MazeCell spawnCell;

        IntVect2 posCoords = mazePos.GetCoordinates();
        List<MazeCell> validSpawnPositions = new List<MazeCell>();

        for (int i = minSpawnDistance; i <= maxSpawnDistance; i++)
        {
            for (int j = minSpawnDistance; j <= maxSpawnDistance; j++)
            {
                int offsetX = i;
                int offsetY = j;

                int spawnCellX = offsetX + posCoords.x;
                int spawnCellY = offsetY + posCoords.y;

                spawnCell = maze.GetCell(spawnCellX, spawnCellY);
                if (spawnCell != null)
                {
                    GameObject cellGO = spawnCell.GetGameObject();
                    if (!IsVisibleToPlayer(cellGO.transform.position, currentCameraPosition))
                    {
                        validSpawnPositions.Add(spawnCell);
                    }
                }
            }
        }

        if (validSpawnPositions.Count > 0)
        {
            int spawnCellIndex = Random.Range(0, validSpawnPositions.Count);
            return validSpawnPositions[spawnCellIndex];
        }

        return null;
    }

    bool IsVisibleToPlayer(Vector3 point, Vector3 player)
    {
        Vector3 dir = point - player;
        Ray ray = new Ray(player, dir);
        if (Physics.Raycast(ray, out RaycastHit hit, dir.magnitude))
        {
            return false;
        }
        return true;
    }
}
