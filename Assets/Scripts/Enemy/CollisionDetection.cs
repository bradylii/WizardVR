using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    Player player;
    ItemDrop dropItemScript;
    EnemyAI enemyAi;
    private bool collided = false;

    public float damage;

    public float collisionCooldown = 0.5f;



    
    private void Start()
    {
        player = GameObject.Find("Game Manager")?.GetComponent<Player>();
        dropItemScript = GetComponent<ItemDrop>();
        enemyAi = GetComponent<EnemyAI>();
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
                enemyAi.wasHit(weaponStats.damage, dropItemScript);
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
