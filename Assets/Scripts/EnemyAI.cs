using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    // How much damage enemy does
    public float damage;
    public float attackRate;
    private bool isDamagingPlayer = false; // Flag to track if coroutine is running

    // player positions and navmesh agent init
    public Transform player;
    [SerializeField] private float distanceToPlayer;
    private NavMeshAgent agent;
    private Player playerInfo;

    // Variables for checking if enemy can see player
    public float detectionRange = 10f;
    public float stoppingDistance = 2f;
    public float angle;

    // variables for rotation
    private Vector3 directionToPlayer;
    private Quaternion targetRotation;

    // variables for going to last known location
    private bool playerSeen = false;
    private Vector3 lastKnownPlayerPosition;

    void Start()
    {
        // Navmesh init
        agent = GetComponent<NavMeshAgent>();
        playerInfo = GameObject.Find("Game Manager")?.GetComponent<Player>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }


    void Update()
    {
        if (isInRange() && isFacingPlayer() && inLineOfSight()) // check if player is visible
        {
            playerSeen = true;
            lastKnownPlayerPosition = player.position;
            RotateTowardsPlayer(); // rotate to player 

            if (distanceToPlayer <= stoppingDistance) // if enemy is close to player stop movement
            {
                agent.isStopped = true;
                if (!isDamagingPlayer) // Only start coroutine if it's not already running
                {
                    StartCoroutine(DamagePlayerOverTime());
                }
                agent.ResetPath();
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(player.position); // if not go to player
            }
        }
        else if (playerSeen) // if player out of vision but enemy has seen them
        {
            agent.isStopped = false;
            agent.SetDestination(lastKnownPlayerPosition); // go to last known position

            if (Vector3.Distance(transform.position, lastKnownPlayerPosition) <= stoppingDistance) // if enemy is close to last known position, stop
            {
                agent.isStopped = true;
                agent.ResetPath();
                playerSeen = false;
            }
        }
    }

    private IEnumerator DamagePlayerOverTime()
    {
        if (playerInfo == null)
        {
            Debug.LogError("playerInfo is null! Assign it before starting DamagePlayerOverTime.");
            yield break; // Exit coroutine to prevent errors
        }

        isDamagingPlayer = true;

        while (Vector3.Distance(transform.position, lastKnownPlayerPosition) <= stoppingDistance)
        {
            playerInfo.lowerPlayerHealth(damage);
            yield return new WaitForSeconds(attackRate); // Wait for 1 second before dealing damage again
        }

        isDamagingPlayer = false; // Reset flag when enemy moves away
    }



    bool isInRange()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return (distanceToPlayer <= detectionRange);
    }

    bool inLineOfSight()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 1.5f; // have raycast come from enemy eyes
        Vector3 direction = (player.position - origin).normalized; 

        if(Physics.Raycast(origin, direction, out hit, detectionRange)) // check if raycast hits something within the detection range
        {
            return (hit.transform.CompareTag("Player")); // return true if it hits player tag
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
        directionToPlayer.y = 0; // make sure the enemy doesnt rotate head up and down (can change this if we have verticality in game)

        targetRotation = Quaternion.LookRotation(directionToPlayer); // figure out where to rotate to
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // smoothly rotate to player 
    }


    // copy pasted code for visualizing detection range, line to player, and stopping distance. does not affect code at all just visual 
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
