using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    Player player;
    ItemDrop dropItemScript;
    EnemyAI enemyAi;
    Golem golemAi;
    BoulderHealth BoulderHealth;

    WeaponStats weaponStats;

    public bool collided = false;

    public float collisionCooldown = 0.5f;
    public bool isSkeleton = true;
    public bool isGolem;



    [SerializeField] private EnemyType enemyType;

    [SerializeField] private bool boulderAlreadyHit = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("GameManager")?.GetComponent<Player>();
        dropItemScript = GetComponent<ItemDrop>();


        switch (enemyType)
        {
            case EnemyType.Skeleton:
                enemyAi = GetComponent<EnemyAI>();
                break;
            case EnemyType.Golem:
                golemAi = GetComponent<Golem>();
                break;
            case EnemyType.GolemBoulders:
                BoulderHealth = GetComponent<BoulderHealth>();
                break;
            default:
                defaultEnemyType();
                break;
        }



    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("[Collision] Trigger Entered with: " + other.gameObject.name);
        if (other.gameObject.tag == "weapon" && !collided)
        {
            Debug.Log("[Collision] Collided with weapon, processing damage.");
            collided = true;

            weaponStats = other.gameObject.GetComponent<WeaponStats>();

            if (weaponStats != null)
            {
                switch (enemyType)
                {
                    case EnemyType.Skeleton:
                        enemyAi.wasHit(weaponStats.damage, dropItemScript);
                        break;
                    case EnemyType.Golem:
                        golemAi.wasHit(weaponStats.damage);
                        break;
                    case EnemyType.GolemBoulders:
                        if (!boulderAlreadyHit)
                            BoulderHealth.wasHit();
                        boulderAlreadyHit = true;
                        break;
                    default:
                        defaultEnemyHit();
                        break;
                }
            }

            StartCoroutine(ResetCollisionCooldown());
        }
    }

    private IEnumerator ResetCollisionCooldown()
    {
        yield return new WaitForSeconds(collisionCooldown);
        collided = false;
    }

    private void defaultEnemyType()
    {
        if (isSkeleton)
        {
            enemyAi = GetComponent<EnemyAI>();
        }
        else if (isGolem)
        {
            golemAi = GetComponent<Golem>();
        }
        else
        {
            BoulderHealth = GetComponent<BoulderHealth>();
        }
    }

    private void defaultEnemyHit()
    {
        if (isSkeleton)
        {
            enemyAi.wasHit(weaponStats.damage, dropItemScript);
        }
        else if (isGolem)
        {
            golemAi.wasHit(weaponStats.damage);
        }
        else
        {
            BoulderHealth.wasHit();
        }
    }
}
