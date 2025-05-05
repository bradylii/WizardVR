using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerHealth;
    public float kills;
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private bool playing;

    [SerializeField] private bool collided = false;
    [SerializeField] private float collisionCooldown = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 100.0f;
        kills = 0;
        playing = true;

        if (gameStateManager == null)
            gameStateManager = GetComponent<GameStateManager>();

    }

    public void killedBadGuy()
    {
        kills++;
        Debug.Log("[PLAYER] kills: " + kills);
    }


    public void lowerPlayerHealth(float damage)
    {
        playerHealth -= damage;
        Debug.Log("[PLAYER] Damage Dealt = " + damage);
        Debug.Log("[PLAYER] player health = " + playerHealth);
    }


    // Update is called once per frame
    void Update()
    {
        if (playing && playerHealth <= 0)
        {
            Debug.Log("[PLAYER] Player Died!");
            gameStateManager.setGameState(GameState.GameOver);
            playing = false;
        }
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

    public void resetStats()
    {
        Debug.Log("[Player] Resetting Stats");
        this.playerHealth = 100;
        this.kills = 0;
        Debug.Log("[Player] PlayerHealth: " + playerHealth);
        Debug.Log("[Player] PlayerKills: " + kills);
    }
}
