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
    public bool collided = false;

    public float damage;

    public float collisionCooldown = 0.5f;
    public bool isSkeleton = true;



    
    private void Start()
    {
        player = GameObject.Find("Game Manager")?.GetComponent<Player>();
        dropItemScript = GetComponent<ItemDrop>();

        if (isSkeleton)
        {
            enemyAi = GetComponent<EnemyAI>();  
        }
        else
        {
            golemAi = GetComponent<Golem>();
        }
        
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("[Collision] Trigger Entered with: " + other.gameObject.name);
        if (other.gameObject.tag == "weapon" && !collided)
        {
            Debug.Log("[Collision] Collided with weapon, processing damage." );
            collided = true;

            WeaponStats weaponStats = other.gameObject.GetComponent<WeaponStats>();

            if (weaponStats != null) 
            {
                if (isSkeleton)
                {
                    enemyAi.wasHit(weaponStats.damage, dropItemScript);
                }
                else
                {
                    golemAi.wasHit(weaponStats.damage, dropItemScript);
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
