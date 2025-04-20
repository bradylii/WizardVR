using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualTesting : MonoBehaviour
{
    public GameStateManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null)
        {
            Debug.Log("[MANUALTESTING] No GameStateManager set...trying to find it now");
            gameManager = FindObjectOfType<GameStateManager>();
            if (gameManager == null)
            {
                Debug.Log("[MANUALTESTING] No GameStateManager Found");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            gameManager.setGameState(GameState.Playing);
            Debug.Log("[MANUALTESTING] Set game state to playing");
        }
    }
}
