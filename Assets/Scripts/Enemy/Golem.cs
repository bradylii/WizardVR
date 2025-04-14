using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
public class Golem : MonoBehaviour
{
    // How much damage enemy does
    public float health = 100;
    public bool isDamagingPlayer = false; // Flag to track if coroutine is running

    private bool isDead = false;

    // player positions and navmesh agent init
    public Transform player;
    [SerializeField] private float distanceToPlayer;
    private Player playerInfo;

    // Variables for checking if enemy can see player
    public float detectionRange = 10f;
    public float angle = 80;

    // variables for rotation
    private Vector3 directionToPlayer;
    private Quaternion targetRotation;

    // variables for going to last known location
    public bool playerSeen = false;

    private bool canAttack = true;
    public float attackCoodldown = 1.5f;

    public bool showDebugGizmos = false;

    private bool hasBeenHit;

    public GameObject boulderPrefab;
    public float chargeTime = 5f;
    private bool isCharging = false;

    public float boulderSpawnHeight = 2f;
    public float boulderSpawnOffset = 2f;
    public float throwSpeed = 15f;
    public float targetPositionHeight = 0.5f;
    public bool useGravity = false;
    public float arcDirectionHeight = 0.5f;

    void Start()
    {
        playerInfo = GameObject.Find("Game Manager")?.GetComponent<Player>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }


    void Update()
    {
        if (isDead) return;

        if (!isDead && isInRange() && isFacingPlayer() && inLineOfSight()) // check if player is visible
        {

            playerSeen = true;
            RotateTowardsPlayer(); // rotate to player 

            if (canAttack && !isDead && !isCharging)
            {
                StartCoroutine(ChargeAndThrowBoulder());
            }

        }

         if (Input.GetKeyDown(KeyCode.B))
         {
            Debug.Log("[GOLEM] Manula Throw");
            ThrowBoulder();
         }
    }


    private IEnumerator ChargeAndThrowBoulder()
    {
        isCharging = true;

        Debug.Log("[GOLEM] Charging Boulder!");

        yield return new WaitForSeconds(chargeTime);

        ThrowBoulder();

        canAttack = true;

        isCharging = false;
    }

    private void ThrowBoulder()
    {
        if(boulderPrefab != null)
        {
            if (!useGravity)
            {
                Vector3 spawnPosition = transform.position + transform.forward * boulderSpawnOffset + Vector3.up * boulderSpawnHeight;
                GameObject boulder = Instantiate(boulderPrefab, spawnPosition, Quaternion.identity);

                Vector3 targetPosition = player.position + Vector3.up * targetPositionHeight;
                Vector3 direction = (targetPosition - transform.position).normalized;

                Rigidbody boulderRb = boulder.GetComponent<Rigidbody>();
                if (boulderRb != null)
                {
                    boulderRb.velocity = direction * throwSpeed;
                    boulderRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                }

                Debug.Log("[GOLEM] Boulder thrown towards the player");

                
            }
            else
            {
                Vector3 spawnPosition = transform.position + transform.forward * boulderSpawnOffset + Vector3.up * boulderSpawnHeight;
                GameObject boulder = Instantiate(boulderPrefab, spawnPosition, Quaternion.identity);

                Vector3 targetPosition = player.position + Vector3.up * targetPositionHeight;
                Vector3 direction = (targetPosition - transform.position).normalized;

                Vector3 arcDirection = direction + Vector3.up * arcDirectionHeight;

                Rigidbody boulderRb = boulder.GetComponent<Rigidbody>();
                if (boulderRb != null)
                {
                    boulderRb.useGravity = true;
                    boulderRb.AddForce(arcDirection.normalized * throwSpeed, ForceMode.VelocityChange);
                }

                Debug.Log("[GOLEM] Boulder thrown towards the player");

               
            }
        }
    }
   


    public void wasHit(float damage, ItemDrop dropItemScript) 
    {
        
        health -= damage;

        if (health <= 0) 
        {
            isDead = true;
            canAttack = false;

            playerInfo.killedBadGuy();
            dropItemScript.dropItem();
            StartCoroutine(DestroyAfterDelay());
        }
        else 
        {
            hitCooldown();
        }
    }

    private IEnumerator hitCooldown() 
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCoodldown);
        canAttack = true;
    }
    private IEnumerator DestroyAfterDelay() {
        yield return new WaitForSeconds(0.5f);
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
