using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
public class EnemyAI : MonoBehaviour
{
    //=========================//
    #region components
    [Header("Components")]
     public Transform player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Player playerInfo;
    [SerializeField] private Animator animator;

    [SerializeField] private DetectPlayer detectionScript;
    #endregion
    //=========================//
    #region Detection
    [Header("Detection")]
    public float stoppingDistance = 2f;
    [SerializeField] private bool playerVisible = false;
    [SerializeField] private bool sawPlayer = false;
    private Vector3 lastKnownPlayerPosition;
    #endregion
    //=========================//
    #region Combat
    [Header("Combat")]
    [SerializeField] private bool canAttack = true;
    [SerializeField] private float attackRate;
    public float health = 10;
    [SerializeField] private float attackCoodldown = 1.5f;
    [SerializeField] private bool isDamagingPlayer = false;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private float remainingDistance; // set initia
    #endregion
    //=========================//
    #region AI State
    [Header("AI State")]
    [SerializeField] private bool goingToPlayer = false;
    [SerializeField] private bool isDead = false;
    #endregion
    //=========================//
    #region Rotation
    [Header("Rotation")]
    private Vector3 directionToPlayer;
    private Quaternion targetRotation;
    #endregion
    //=========================//
    #region Debug
    [Header("Debug")]
    public bool showDebugGizmos = true;
    #endregion
    //=========================//

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

        remainingDistance = stoppingDistance + 0.1f; // iniital set for bugs
    }

    void Update()
    {
        if (isDead) return;
        playerVisible = detectionScript.playerVisible;

        

        if (playerVisible) // check if player is visible
        {
            sawPlayer = true;
            lastKnownPlayerPosition = player.position;
            RotateTowardsPlayer(); // rotate to player 

            if (agent.hasPath)
                remainingDistance = agent.remainingDistance;

            if (remainingDistance <= stoppingDistance && !agent.pathPending) // if enemy is close to player stop movement
            {
                agent.isStopped = true;
                goingToPlayer = false;

                if (canAttack && !isDead && !isAttacking)
                {
                    StartCoroutine(AttackCoolDown());
                }

                agent.ResetPath();
            }
            else // if enemy is not close go to them
            {
                goingToPlayer = true;
                agent.isStopped = false;
                agent.SetDestination(player.position); 
            }
        }
        else if (sawPlayer && !playerVisible) // if player out of vision but enemy has seen them
        {
            goingToPlayer = false;
            agent.isStopped = false;
            agent.SetDestination(lastKnownPlayerPosition);

            if (agent.remainingDistance <= stoppingDistance && !agent.pathPending) // if enemy is close to last known position, stop
            {
                agent.isStopped = true;
                agent.ResetPath();
                sawPlayer = false;
            }

            
        }

        if (agent != null && animator != null) // tells animator if walking or stopped
        {
            float speed = agent.velocity.magnitude;
            animator.SetFloat("speedv", speed);
        }
    }


    private IEnumerator AttackCoolDown()
    {
        if (isDead) yield break;

        canAttack = false;
        isAttacking = true;

        if (!isDead)
            animator.SetTrigger("Attack1h1");
     
        yield return new WaitForSeconds(attackCoodldown);

        if (!isDead)
            canAttack = true;
        
        isAttacking = false;
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

            StartCoroutine(hitCooldown());
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

        Vector3 target = playerVisible ? player.position : lastKnownPlayerPosition;

        directionToPlayer = (target - transform.position).normalized;
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
