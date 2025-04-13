using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerHealth;
    public float kills;
    public GameStateManager gameStateManager;
    public Boolean playing;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 100.0f;
        kills = 0;
        playing = true;
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
        if (playing && playerHealth <= 0)
        {
            Debug.Log("Player Died!");
            gameStateManager.setGameState(GameState.GameOver);
            playing = false;
        }
    }
}
