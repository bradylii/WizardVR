using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;




public enum EnemyState
{
    Wander_init = 0,
    Wander_to_Path = 1,
    Pursue = 2,
    Search_init = 3,
    Search_to_Path = 4
}
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

    //variables for Search_init and Search_To_Path timers
    private float timerDuration = 10f;
    public float currentTime = 0f;

    //manages the current state in the state machine
    public EnemyState currentState = EnemyState.Wander_init;
    

    //variables for the different wanderpoints and destination points
    public Vector3 originalWanderPoint;
    public Vector3 newWanderPoint;
    public Vector3 destinationPoint;
    int dRadius = 4;


    //variable to judge how far enemy can be from a destination point
    public float margin_of_difference;


    void Start()
    {
        // Navmesh init
        agent = GetComponent<NavMeshAgent>();


        originalWanderPoint = transform.position;//add a raycast to snap to the ground
        originalWanderPoint.y = (float)0.58;


        playerInfo = GameObject.Find("Game Manager")?.GetComponent<Player>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }


    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        //this section modifies the enemy state
        if (PlayerDetected())
        {
            currentState = EnemyState.Pursue;
        }
        else if (currentState == EnemyState.Pursue)
        {
            newWanderPoint = path.corners[path.corners.Length - 1];
            currentState = EnemyState.Search_init;//this is supposed to go EnemyState.Search_init but for now we will go to Wander_init
        }


        //this section determines what to do in any given state;

        switch (currentState)
        {
            case EnemyState.Wander_init:
                currentTime = 0f;

                //randomly select a point in a radius around the wanderpoint
                System.Random random = new System.Random();
                int random_number_x = random.Next(((int)originalWanderPoint.x) - dRadius, ((int)originalWanderPoint.x) + 1 + dRadius);
                int random_number_z = random.Next(((int)originalWanderPoint.z) - dRadius, ((int)originalWanderPoint.z) + 1 + dRadius);
                Vector3 generated_point = new Vector3(random_number_x, originalWanderPoint.y, random_number_z);

                path = new NavMeshPath();
                if (Vector3.Distance(generated_point, originalWanderPoint) <= dRadius && agent.CalculatePath(generated_point, path))
                {
                    destinationPoint = generated_point;
                    //currentState = EnemyState.Wander_to_Path;
                    //transform.position = generated_point;
                    currentState = EnemyState.Wander_to_Path;
                    agent.SetPath(path);
                }
                break;
            case EnemyState.Wander_to_Path:
                if (agent.remainingDistance == 0 || agent.velocity == new Vector3(0, 0, 0))
                {
                    currentState = EnemyState.Wander_init;
                }
                break;
            case EnemyState.Pursue:
                currentTime = 0f;
                if (Vector3.Distance(destinationPoint, player.position) > margin_of_difference)
                {
                    path = new NavMeshPath();
                    agent.CalculatePath(player.position, path);
                    agent.SetPath(path);
                }
                break;
            case EnemyState.Search_init:
                if (addDeltaTime() == true)
                {
                    currentState = EnemyState.Wander_init;
                }
                //newWanderPoint = path.corners[path.corners.Length - 1];//Create a new temporary wanderpoint to circle around.
                System.Random random1 = new System.Random();
                int random_number_x1 = random1.Next(((int)newWanderPoint.x) - dRadius, ((int)newWanderPoint.x) + 1 + dRadius);
                int random_number_z1 = random1.Next(((int)newWanderPoint.z) - dRadius, ((int)newWanderPoint.z) + 1 + dRadius);
                Vector3 generated_point1 = new Vector3(random_number_x1, newWanderPoint.y, random_number_z1);

                path = new NavMeshPath();


                if (Vector3.Distance(generated_point1, newWanderPoint) <= dRadius && agent.CalculatePath(generated_point1, path))
                {
                    destinationPoint = generated_point1;
                    //currentState = EnemyState.Wander_to_Path;
                    //transform.position = generated_point;
                    currentState = EnemyState.Search_to_Path;
                    agent.SetPath(path);
                }
                break;
            case EnemyState.Search_to_Path:
                if (addDeltaTime() == true)
                {
                    currentState = EnemyState.Wander_init;
                }
                if (agent.remainingDistance == 0 || agent.velocity == new Vector3(0, 0, 0))
                {
                    currentState = EnemyState.Search_init;
                }
                break;
            default:

                break;

        }
    }

    bool addDeltaTime()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= timerDuration)
        {
            currentTime = 0f;
            return true;
        }
        return false;
    }

    bool PlayerDetected()
    {
        if (isInRange() && isFacingPlayer() && inLineOfSight()) // check if player is visible
        {
            return true;
        }
        return false;
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
