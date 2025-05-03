using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
public class _TestEnemyAI : MonoBehaviour
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
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private NavMeshAgent agent;
    public Player playerInfo;

    // Variables for checking if enemy can see player
    [Header("Detection")]
    public float stoppingDistance = 2f;

    // variables for rotation
    [Header("Rotation")]
    private Vector3 directionToPlayer;
    private Quaternion targetRotation;

    // variables for going to last known location
    [Header("Last Known Location of Player")]
    public bool playerSeen = false;
    [SerializeField]private bool playerCurrentlyVisible = false;
    private Vector3 lastKnownPlayerPosition;

    [Header("Misc")]
    [SerializeField] private Animator animator;
    private bool canAttack = true;

    public bool showDebugGizmos = true;


    //public RoomEventManager roomManager;
    //public int roomNumber = 1;

    void Start()
    {
        // Get Components if Null
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerInfo == null)
            playerInfo = GameObject.Find("Game Manager")?.GetComponent<Player>();
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void OnPlayerSpotted(Vector3 playerPosition)
    {
        playerSeen = true;
        lastKnownPlayerPosition = playerPosition;
    }

    void Update()
    {
        if (isDead) return;
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (playerSeen) // check if player is visible
        {
            playerCurrentlyVisible = true;
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
        else if (playerCurrentlyVisible) // if player out of vision but enemy has seen them
        {
            playerCurrentlyVisible = false;
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
     
        yield return new WaitForSeconds(attackCoodldown);

        if (!isDead)
            canAttack = true;
    }

    public void wasHit(float damage, ItemDrop dropItemScript)
    {
        health -= damage;

        if (health <= 0)
        {
            Debug.Log("[ENEMYAI] Skeleton Died");
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

            dropItemScript.dropItem();
            StartCoroutine(DestroyAfterDelay());
        }
        else // if alive
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
        Debug.Log("[ENEMYAI] Skeleton going to be destroyed");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    void RotateTowardsPlayer()
    {
        if (isDead) return;
        directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0; // make sure the enemy doesnt rotate head up and down (can change this if we have verticality in game)

        targetRotation = Quaternion.LookRotation(directionToPlayer); // figure out where to rotate to
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // smoothly rotate to player 
    }

    void OnDrawGizmosSelected()
    {
        if (showDebugGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, stoppingDistance);
        }
    }
}
