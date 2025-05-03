using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
public class _TestGolem : MonoBehaviour
{
    // How much damage enemy does
    public float health = 100;
    public bool isDamagingPlayer = false; // Flag to track if coroutine is running

    private bool isDead = false;

    // player positions and navmesh agent init
    public Transform player;
    [SerializeField] private float distanceToPlayer;
    private Player playerInfo;
    public GameStateManager gameStateManager;

    // Variables for checking if enemy can see player
    public float detectionRange = 10f;
    public float angle = 80;

    // variables for rotation
    private Vector3 directionToPlayer;
    private Quaternion targetRotation;

    // variables for going to last known location
    [SerializeField] private bool playerSeen = false;

    private bool canAttack = true;
    public float attackCoodldown = 1.5f;

    public GameObject boulderPrefab;
    public float chargeTime = 5f;
    [SerializeField] private bool isCharging = false;

    public float boulderSpawnHeight = 2f;
    public float boulderSpawnOffset = 2f;
    public float throwSpeed = 15f;
    public float targetPositionHeight = 0.5f;
    public bool useGravity = false;
    public float arcDirectionHeight = 0.5f;

    public ItemDrop dropItemScript;

    void Start()
    {
        playerInfo = GameObject.FindGameObjectWithTag("GameManager")?.GetComponent<Player>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (dropItemScript == null)
        {
            dropItemScript = GetComponent<ItemDrop>();
        }

        if (gameStateManager == null)
        {
            gameStateManager = GameObject.FindGameObjectWithTag("GameManager")?.GetComponent<GameStateManager>();
        }
    }


    void Update()
    {
        if (isDead) return;

        if (playerSeen) // check if player is visible
        {
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

    public void OnPlayerSpotted()
    {
        playerSeen = true;
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
        if (boulderPrefab != null)
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

    public void wasHit(float damage)
    {

        health -= damage;

        if (health <= 0)
        {
            isDead = true;
            canAttack = false;

            playerInfo.killedBadGuy();
            dropItemScript.dropItem();
            StartCoroutine(DestroyAfterDelay());
            gameStateManager.setGameState(GameState.GameOver);
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
    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    void RotateTowardsPlayer()
    {
        directionToPlayer.y = 0; // make sure the enemy doesnt rotate head up and down (can change this if we have verticality in game)

        targetRotation = Quaternion.LookRotation(directionToPlayer); // figure out where to rotate to
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // smoothly rotate to player 
    }
}
