using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    public float moveDownAmount = 10f;
    public float speed = 1f;

    private List<GameObject> enemiesInRoom = new List<GameObject>();
    private bool doorOpened = false;
    private Vector3 targetPosition;

    void Start()
    {
        // Define the final position of the door after it moves down
        targetPosition = transform.position - new Vector3(0, moveDownAmount, 0);
    }

    void Update()
    {
        // Remove any enemies that have been destroyed (null)
        enemiesInRoom.RemoveAll(enemy => enemy == null);

        // Check if all enemies are defeated and door hasn't already opened
        if (!doorOpened && enemiesInRoom.Count == 0)
        {
            doorOpened = true;
            StartCoroutine(OpenDoor());
        }
    }

    // Manually register enemies you want to track
    public void RegisterEnemy(GameObject enemy)
    {
        if (enemy != null && !enemiesInRoom.Contains(enemy))
        {
            enemiesInRoom.Add(enemy);
        }
    }

    private IEnumerator OpenDoor()
    {
        Debug.Log("[DOOR] Opening door...");
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        Debug.Log("[DOOR] Door opened!");
    }
}
