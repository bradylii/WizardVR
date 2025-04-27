using Oculus.Voice.Windows;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RoomEventManager : MonoBehaviour
{
    public GameObject[] doors;

    public GameObject[] enemies;

    public int roomNumber = 1;

    [SerializeField] private int enemyCount;

    public float doorMoveDownAmount = 10f;
    public float speed = 1f;

    public bool doorsOpened = false;

    // Start is called before the first frame update
    void Start()
    {
        if (doors == null)
        {
            List<GameObject> matchedDoors = new List<GameObject>();
            GameObject[] allDoors = GameObject.FindGameObjectsWithTag("RoomDoor");

            foreach (GameObject door in allDoors)
            {
                if (door.name.Contains($"Room{roomNumber}"))
                    matchedDoors.Add(door);
            }

            doors = matchedDoors.ToArray();
        }

        if (enemies == null)
        {
            List<GameObject> matchedEnemies = new List<GameObject>();
            GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in allEnemies)
            {
                if (enemy.name.Contains($"Room{roomNumber}"))
                    matchedEnemies.Add(enemy);
            }

            enemies = matchedEnemies.ToArray();
        }

        if (enemies != null)
            enemyCount = enemies.Length;

    }

    public void removeEnemy()
    {
        enemyCount--;

        if (enemyCount == 0)
            StartCoroutine(openDoors());
    }

    private IEnumerator openDoors()
    {
        Debug.Log("[RoomManager] Opening doors...");
        doorsOpened = true;

        foreach (GameObject door in doors)
        {
            Vector3 start = door.transform.position;
            Vector3 targetPosition = start - new Vector3(0, doorMoveDownAmount, 0);
            while (Vector3.Distance(door.transform.position, targetPosition) > 0.01f)
            {
                door.transform.position = Vector3.MoveTowards(door.transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            door.transform.position = targetPosition;

            Debug.Log("[RoomManager] Doors opened!");
        }
    }

    private bool allEnemiesDefeated()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
                return false;
        }

        Debug.Log("[RoomManager] All Enemies Defeated");
        return true;
    }

    private void Update()
    {
        if (!doorsOpened && allEnemiesDefeated())
        {
            StartCoroutine(openDoors());

            if (gameObject.name.Contains("BossRoom"))
            {
                GameStateManager stateManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
                stateManager.setGameState(GameState.Victory);
            }
        }
    }
}
