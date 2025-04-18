using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestingPlayerNavMesh : MonoBehaviour
{
    public Transform target;
    private Vector3 startPosition;

    private NavMeshAgent agent;
    private bool hasArrived = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;
        agent.SetDestination(target.position);
    }

    void Update()
    {
        if (!hasArrived && !agent.pathPending && agent.remainingDistance <= 0.1f)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                hasArrived = true;
                Debug.Log("Arrived at target");

                // Optional delay before teleporting back
                StartCoroutine(TeleportBackAfterDelay(0.5f));
            }
        }
    }

    private IEnumerator TeleportBackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Disable NavMeshAgent so we can move the object manually
        agent.enabled = false;

        transform.position = startPosition;

        // Re-enable and set new destination if needed
        agent.enabled = true;
        hasArrived = false;
        agent.SetDestination(target.position);
    }
}
