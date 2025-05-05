using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
public class OldEnemyAI : MonoBehaviour
{
    // How much damage enemy does
    [Header("Stats")]
    public float damage;
    public float attackRate;
    public float health = 10;
    public float attackCoodldown = 1.5f;
    [SerializeField] private bool isDamagingPlayer = false; // Flag to track if coroutine is running
    [SerializeField] private bool goingToPlayer = false;

    [SerializeField] private bool isDead = false;


    // player positions and navmesh agent init
    [Header("Player Info")]
    public Transform player;
    public Transform playerHead;
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private NavMeshAgent agent;
    public Player playerInfo;

    // Variables for checking if enemy can see player
    [Header("Detection")]
    public float detectionRange = 10f;
    public float stoppingDistance = 2f;
    public float angle = 80;

    // variables for rotation
    [Header("Rotation")]
    private Vector3 directionToPlayer;
    private Quaternion targetRotation;

    // variables for going to last known location
    [Header("Last Known Location of Player")]
    public bool playerSeen = false;
    private Vector3 lastKnownPlayerPosition;

    [Header("Misc")]
    [SerializeField] private Animator animator;
    private bool canAttack = true;
    public bool showDebugGizmos = false;
    private bool hasBeenHit;


    //public RoomEventManager roomManager;
    //public int roomNumber = 1;

    void Start()
    {
        // Get Components if Null
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerInfo == null)
            playerInfo = GameObject.Find("Game Manager")?.GetComponent<Player>();
        if (playerHead == null)
            playerHead = GameObject.FindGameObjectWithTag("MainCamera").transform;
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (isDead) return;

        if (!isDead && isInRange() && isFacingPlayer() && inLineOfSight()) // check if player is visible
        {
            // Debug.Log("[ENEMYAI] Spots Player" );
            playerSeen = true;
            lastKnownPlayerPosition = player.position;
            RotateTowardsPlayer(); // rotate to player 

            if (distanceToPlayer <= stoppingDistance) // if enemy is close to player stop movement
            {
                agent.isStopped = true;
                goingToPlayer = false;

                if (agent != null && animator != null && canAttack && !isDead)
                {
                    StartCoroutine(AttackCoolDown());
                }

                agent.ResetPath();
            }
            else
            {
                goingToPlayer = true;
                agent.isStopped = false;
                agent.SetDestination(player.position); // if not go to player
            }
        }
        else if (playerSeen) // if player out of vision but enemy has seen them
        {
            goingToPlayer = false;
            agent.isStopped = false;
            agent.SetDestination(lastKnownPlayerPosition); // go to last known position

            if (Vector3.Distance(transform.position, lastKnownPlayerPosition) <= stoppingDistance) // if enemy is close to last known position, stop
            {
                agent.isStopped = true;
                agent.ResetPath();
                playerSeen = false;
            }
        }

        if (agent != null && animator != null)
        {
            float speed = agent.velocity.magnitude;
            animator.SetFloat("speedv", speed);
        }
    }

    private IEnumerator AttackCoolDown()
    {
        if (isDead) yield break;

        canAttack = false;

        if (!isDead)
            animator.SetTrigger("Attack1h1");
        
        yield return new WaitForSeconds(0.4f);
     
        yield return new WaitForSeconds(attackCoodldown);

        if (!isDead)
            canAttack = true;
        
    }

    public void wasHit(float damage, ItemDrop dropItemScript)
    {
        health -= damage;

        if (health <= 0)
        {
            Debug.Log("[ENEMYAI] Enemy Died");
            isDead = true;
            canAttack = false;

            StopAllCoroutines();
            animator.ResetTrigger("Attack1h1");
            animator.ResetTrigger("Hit1");
            animator.SetFloat("speedv", 0);

            if (agent != null)
            {
                agent.isStopped = true;
                agent.ResetPath();
                agent.enabled = false;
            }


            animator.SetTrigger("Fall1");
            animator.applyRootMotion = false;
            
            if (playerInfo != null)
                playerInfo.killedBadGuy();
            else   
                Debug.Log("[ENEMYAI] Playerinfo null");

            dropItemScript.dropItem();
            StartCoroutine(DestroyAfterDelay());
        }
        else
        {
            if (agent != null)
            {
                agent.isStopped = true;
                agent.ResetPath();
            }
            hitCooldown();
            animator.SetTrigger("Hit1");
        }
    }

    private IEnumerator hitCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCoodldown);
        if (agent != null && health > 0)
        {
            agent.isStopped = false;
        }
        canAttack = true;
    }
    private IEnumerator DestroyAfterDelay()
    {
        Debug.Log("[ENEMYAI] Going to destroy");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    bool isInRange()
    {
        if (player == null) return false;
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return (distanceToPlayer <= detectionRange);
    }

    bool inLineOfSight()
    {
        if (player == null) return false;
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 1.5f; // have raycast come from enemy eyes
        Vector3 direction = (playerHead.position - origin).normalized;

        if (Physics.Raycast(origin, direction, out hit, detectionRange)) // check if raycast hits something within the detection range
        {
            return (hit.transform.CompareTag("Player")); // return true if it hits player tag
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

    void RotateTowardsPlayer()
    {
        if (isDead) return;
        directionToPlayer.y = 0; // make sure the enemy doesnt rotate head up and down (can change this if we have verticality in game)

        targetRotation = Quaternion.LookRotation(directionToPlayer); // figure out where to rotate to
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // smoothly rotate to player 
    }


    // copy pasted code for visualizing detection range, line to player, and stopping distance. does not affect code at all just visual 
    void OnDrawGizmosSelected()
    {

        if (!showDebugGizmos) return;

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
