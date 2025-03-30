using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerHealth;
    public float kills;
    public GameStateManager gameStateManager;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 100.0f;
        kills = 0;
    }

    public void killedBadGuy()
    {
        kills++;
        Debug.Log("kills: " + kills);
    }

    public void lowerPlayerHealth(float damage)
    {
        playerHealth -= damage;
        Debug.Log("player health = " + playerHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth <= 0)
        {
            gameStateManager.setGameState(GameState.GameOver);
        }
    }
}
