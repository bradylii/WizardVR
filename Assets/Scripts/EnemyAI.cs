using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    public Transform player;
    [SerializeField] private float distanceToPlayer;
    private NavMeshAgent agent;

    public float detectionRange = 10f;
    public float stoppingDistance = 2f;
    public float angle;

    private Vector3 directionToPlayer;
    private Quaternion targetRotation;

    private bool playerSeen = false;
    private Vector3 lastKnownPlayerPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }


    void Update()
    {
        if (isInRange() && isFacingPlayer() && inLineOfSight())
        {
            playerSeen = true;
            lastKnownPlayerPosition = player.position;
            RotateTowardsPlayer();

            if (distanceToPlayer <= stoppingDistance)
            {
                agent.isStopped = true;
                agent.ResetPath();
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
        }
        else if (playerSeen)
        {
            agent.isStopped = false;
            agent.SetDestination(lastKnownPlayerPosition);

            if (Vector3.Distance(transform.position, lastKnownPlayerPosition) <= stoppingDistance)
            {
                agent.isStopped = true;
                agent.ResetPath();
                playerSeen = false;
            }
        }
    }



    bool isInRange()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return (distanceToPlayer <= detectionRange);
    }

    bool inLineOfSight()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 direction = (player.position - origin).normalized;

        if(Physics.Raycast(origin, direction, out hit, detectionRange))
        {
            return (hit.transform.CompareTag("Player"));
        }
        return false;
    }

    bool isFacingPlayer()
    {
        directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        return angleToPlayer < angle;
    }

    void RotateTowardsPlayer()
    {
        directionToPlayer.y = 0;

        targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }


    void OnDrawGizmosSelected()
    {
        // Draw Detection Range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw Stopping Distance
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);

        // Draw Raycast Line
        if (player != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, player.position);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit, detectionRange))
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, hit.point);
            }
        }

        Gizmos.color = Color.cyan;
        Vector3 forward = transform.forward * detectionRange;
        Vector3 leftBoundary = Quaternion.Euler(0, -angle, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, angle, 0) * forward;

        Vector3 startPos = transform.position + Vector3.up * 1.5f;

        Gizmos.DrawLine(startPos, startPos + leftBoundary);
        Gizmos.DrawLine(startPos, startPos + rightBoundary);
        Gizmos.DrawLine(startPos + leftBoundary, startPos + rightBoundary);

    }


}
