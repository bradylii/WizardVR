using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{

    // player positions and navmesh agent init
    [Header("Player Info")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerHead;
    [SerializeField] private float distanceToPlayer;

    // Variables for checking if enemy can see player
    [Header("Detection")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float angle = 80;
    [SerializeField] private Vector3 directionToPlayer;

    // variables for going to last known location
    [Header("Last Known Location of Player")]
    public bool playerVisible = false;

    [Header("Misc")]
    [SerializeField] private bool showDebugGizmos = false;


    [SerializeField] private bool DebuggedPlayerSeen = false;


    void Start()
    {
        // Get Components if Null
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerHead == null)
            playerHead = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }


    void Update()
    {
        bool canSeePlayer = isInRange() && isFacingPlayer() && inLineOfSight();
        if (canSeePlayer) // check if player is visible for the first time
        {
            if (!DebuggedPlayerSeen)
            {
                Debug.Log("[DetectPlayer] Spots Player" );
                DebuggedPlayerSeen = true;
            }

            playerVisible = true;
        }
        else 
        {
            playerVisible = false;
            DebuggedPlayerSeen = false;
        }
    }

    bool isInRange()
    {
        if (player == null) return false;
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return distanceToPlayer <= detectionRange;
    }

    bool inLineOfSight()
    {
        if (player == null) return false;
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 1.5f; // have raycast come from enemy eyes
        Vector3 direction = (playerHead.position - origin).normalized;

        if (Physics.Raycast(origin, direction, out hit, detectionRange)) // check if raycast hits something within the detection range
        {
            return hit.transform.CompareTag("Player"); // return true if it hits player tag
        }
        return false;
    }

    bool isFacingPlayer()
    {
        if (player == null) return false;
        directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        return angleToPlayer < angle;
    }

    // copy pasted code for visualizing detection range, line to player, and stopping distance. does not affect code at all just visual 
    void OnDrawGizmosSelected()
    {
        if (!showDebugGizmos) return;

        // Draw Detection Range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

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
