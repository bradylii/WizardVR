using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    // How much damage enemy does
    public float damage;
    public float attackRate;
    public float health = 10;
    public bool isDamagingPlayer = false; // Flag to track if coroutine is running
    public bool goingToPlayer = false;

    // player positions and navmesh agent init
    public Transform player;
    [SerializeField] private float distanceToPlayer;
    private NavMeshAgent agent;
    private Player playerInfo;

    // Variables for checking if enemy can see player
    public float detectionRange = 10f;
    public float stoppingDistance = 2f;
    public float angle = 80;

    // variables for rotation
    private Vector3 directionToPlayer;
    private Quaternion targetRotation;

    // variables for going to last known location
    public bool playerSeen = false;
    private Vector3 lastKnownPlayerPosition;

    private Animator animator;
    private bool canAttack = true;
    public float attackCoodldown = 1.5f;

    public bool showDebugGizmos = false;

    private bool hasBeenHit;

    void Start()
    {
        // Navmesh init
        agent = GetComponent<NavMeshAgent>();
        playerInfo = GameObject.Find("Game Manager")?.GetComponent<Player>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        animator = GetComponent<Animator>();
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
                goingToPlayer = false;

                
                if (!isDamagingPlayer && animator == null) // Only start coroutine if it's not already running
                {
                    StartCoroutine(DamagePlayerOverTime());
                }
                
                
                if (agent != null && animator != null && canAttack)
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
        canAttack = false;

        animator.SetTrigger("Attack1h1");

        yield return new WaitForSeconds(attackCoodldown);

        canAttack = true;
    }

    private IEnumerator DamagePlayerOverTime()
    {
        if (playerInfo == null)
        {
            Debug.LogError("[ENEMYAI] playerInfo is null! Assign it before starting DamagePlayerOverTime.");
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

    public void DamagePlayer() 
    {
        if (Vector3.Distance(transform.position, player.position) <= stoppingDistance)
        {
            Debug.Log("[ENEMYAI] Damaged Player (animation)");
            playerInfo?.lowerPlayerHealth(damage);
        }
    }

    /* 
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "weapon" && !hasBeenHit)
        {
            hasBeenHit = true;
            animator.SetTrigger("Fall1");
        }
    }
    */

    public void wasHit(float damage, ItemDrop dropItemScript) 
    {
        
        health -= damage;

        if (health <= 0) 
        {
            animator.SetTrigger("Fall1");
            playerInfo.killedBadGuy();
            dropItemScript.dropItem();
            StartCoroutine(DestroyAfterDelay());
        }
        else 
        {
            animator.SetTrigger("Hit1");
        }
    }

    private IEnumerator DestroyAfterDelay() {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
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
