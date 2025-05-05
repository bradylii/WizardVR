using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private bool collided = false;
    [SerializeField] private float collisionCooldown = 0.5f;

    [SerializeField] private Player playerInfo;
    // Start is called before the first frame update
    void Start()
    {
        
        if (playerInfo == null)
        {
            playerInfo = GameObject.FindGameObjectWithTag("GameManager")?.GetComponent<Player>();
            if (playerInfo == null)
                Debug.Log("[PlayerCollision] Couldnt find player script in game manager" );
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInfo == null)
        {
            playerInfo = GameObject.FindGameObjectWithTag("GameManager")?.GetComponent<Player>();
            if (playerInfo == null)
                Debug.Log("[PlayerCollision] -UPDATE()- Couldnt find player script in game manager" );
        }
    }

    public void lowerPlayerHealth(float damage)
        {
            playerInfo.playerHealth -= damage;
            Debug.Log("[PlayerCollision] Damage Dealt = " + damage);
            Debug.Log("[PlayerCollision] player health = " + playerInfo.playerHealth);
        }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyWeapon" && !collided)
        {
            Debug.Log("[PlayerCollision] Collided with weapon, processing damage." );
            collided = true;

            WeaponStats weaponStats = other.gameObject.GetComponent<WeaponStats>();
            Debug.Log("[PlayerCollision] Weapon did " + weaponStats.damage + " damage" );

            if (weaponStats != null) 
            {
                lowerPlayerHealth(weaponStats.damage);
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
