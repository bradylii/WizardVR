using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
public class Golem : MonoBehaviour
{
    #region Combat Stats
    [Header("Combat Stats")]
    public float health = 100f;
    [SerializeField] private float attackCoodldown = 1.5f;
    private bool isDead = false;
    private bool canAttack = true;
    [SerializeField] private bool isDamagingPlayer = false; // Tracks if coroutine is running
    #endregion

    #region References
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Player playerInfo;
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private DetectPlayer detectionScript;
    [SerializeField] private ItemDrop dropItemScript;
    #endregion

    #region Detection
    [Header("Detection")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float angle = 80f;
    [SerializeField] private bool playerVisible = false;
    #endregion

    #region Rotation
    [Header("Rotation")]
    private Vector3 directionToPlayer;
    private Quaternion targetRotation;
    #endregion

    #region Boulder Attack
    [Header("Boulder Attack")]
    [SerializeField] private GameObject boulderPrefab;
    [SerializeField] private float chargeTime = 5f;
    [SerializeField] private bool isCharging = false;
    [SerializeField] private float boulderSpawnHeight = 2f;
    [SerializeField] private float boulderSpawnOffset = 2f;
    [SerializeField] private float throwSpeed = 15f;
    [SerializeField] private float targetPositionHeight = 0.5f;
    [SerializeField] private bool useGravity = false;
    [SerializeField] private float arcDirectionHeight = 0.5f;
    #endregion

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

        playerVisible = detectionScript.playerVisible;

        if (playerVisible)
        {
            directionToPlayer = (player.position - transform.position).normalized;
            RotateTowardsPlayer();

            if (canAttack && !isDead && !isCharging)
            {
                StartCoroutine(ChargeAndThrowBoulder());
            }
        }

        if (Input.GetKeyDown(KeyCode.B)) // Manual testing 
        {
            Debug.Log("[GOLEM] Manula Throw");
            StartCoroutine(ChargeAndThrowBoulder());
        }
    }

    private IEnumerator ChargeAndThrowBoulder()
    {
        isCharging = true;

        Debug.Log("[GOLEM] Charging Boulder!");

        Vector3 spawnPosition = transform.position + transform.forward * boulderSpawnOffset + Vector3.up * boulderSpawnHeight;
        GameObject boulder = Instantiate(boulderPrefab, spawnPosition, Quaternion.identity);


        yield return new WaitForSeconds(chargeTime);

       

        ThrowBoulder(boulder);

        canAttack = true;
        isCharging = false;
    }

    private void ThrowBoulder(GameObject boulder)
    {
        if (boulderPrefab != null)
        {
            if (!useGravity)
            {
                Vector3 spawnPosition = transform.position + transform.forward * boulderSpawnOffset + Vector3.up * boulderSpawnHeight;

                Vector3 targetPosition = player.position + Vector3.up * targetPositionHeight;
                Vector3 direction = (targetPosition - spawnPosition).normalized;

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

                Vector3 targetPosition = player.position + Vector3.up * targetPositionHeight;

                Rigidbody boulderRb = boulder.GetComponent<Rigidbody>();
                if (boulderRb != null)
                {
                    boulderRb.useGravity = true;
                    boulderRb.velocity = CalculateProjectileVelocity(spawnPosition, targetPosition, 1.2f);
                }

                Debug.Log("[GOLEM] Boulder thrown towards the player");
            }
        }
    }

    Vector3 CalculateProjectileVelocity(Vector3 origin, Vector3 target, float timeToTarget)
    {
        Vector3 toTarget = target - origin;
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0, toTarget.z);

        float y = toTarget.y;
        float xz = toTargetXZ.magnitude;

        float vxz = xz / timeToTarget;
        float vy = y / timeToTarget + 0.5f * Mathf.Abs(Physics.gravity.y) * timeToTarget;

        Vector3 result = toTargetXZ.normalized * vxz;
        result.y = vy;
        return result;
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
