using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    bool collided = false;
    public float collisionCooldown = 0.5f;

    Player playerInfo;
    // Start is called before the first frame update
    void Start()
    {
        playerInfo = GameObject.Find("Game Manager")?.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void lowerPlayerHealth(float damage)
        {
            playerInfo.playerHealth -= damage;
            Debug.Log("[PLAYER] Damage Dealt = " + damage);
            Debug.Log("[PLAYER] player health = " + playerInfo.playerHealth);
        }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyWeapon" && !collided)
        {
            Debug.Log("[PLAYER] Collided with weapon, processing damage." );
            collided = true;

            WeaponStats weaponStats = other.gameObject.GetComponent<WeaponStats>();

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
