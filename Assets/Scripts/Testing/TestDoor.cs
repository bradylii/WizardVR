using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDoor : MonoBehaviour
{
    public List<GameObject> enemies;
    [SerializeField] private int enemyCount;

    public float moveDownAmount = 10f;
    public float speed = 1f;

    private bool doorOpened = false;

    public Vector3 targetPosition;
    void Start()
    {
        enemyCount = enemies.Count;

        if (targetPosition == null)
            targetPosition = transform.position - new Vector3(0, moveDownAmount, 0);


    }


    public void removeEnemy()
    {
        Debug.Log("[DOOR] Subtractin enemy");
        enemyCount--;

        if (enemyCount <= 0)
        {
            StartCoroutine(OpenDoor());
        }
    }

    private IEnumerator OpenDoor()
    {
        doorOpened = true;

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
