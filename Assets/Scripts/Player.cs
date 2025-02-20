using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float playerHealth;
    public GameStateManager gameStateManager;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 100.0f;
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
