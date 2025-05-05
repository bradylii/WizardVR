using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
     [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private ItemDrop dropItemScript;
    [SerializeField] private EnemyAI enemyAi;
    [SerializeField] private Golem golemAi;
    [SerializeField] private MagicRockHealth magicRockHealth;
    [SerializeField] private EnemyType enemyType;

    [Header("Weapon")]
    [SerializeField] private WeaponStats weaponStats;

    [Header("Settings")]
    [SerializeField] private float collisionCooldown = 0.5f;
    [SerializeField] private bool collided = false;
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
                magicRockHealth = GetComponent<MagicRockHealth>();
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
                            magicRockHealth.wasHit();
                        boulderAlreadyHit = true;
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

}
