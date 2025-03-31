using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Finish enemy spawner
public class MazeEnemySpawner : MonoBehaviour
{
    [SerializeField]
    private MazeWorld maze;

    [SerializeField]
    private GameObject enemyPrefab;

    public static int maxEnemies = 5;

    private int numEnemies = 0;

    private static bool enableSpawning = false;

    public static void EnableSpawning()
    {
        enableSpawning = true;
    }

    private void Update()
    {
        if (enableSpawning)
        {
            System.Random rand;
            rand = new System.Random();

            MazeCell mc = maze.GetCell(Random.Range(0, maze.GetWidth() - 1), Random.Range(0, maze.GetHeight() - 1));
            IntVect2 coords = mc.GetCoordinates();
            

        }
    }

    void SpawnEnemyAtPosition()
    {

    }

    void DecrementEnemies()
    {
        numEnemies--;
    }


}
