using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretText : MonoBehaviour
{
    // public RoomEventManager roomManager;

    public GameStateManager stateManager;


    private void Start()
    {
        // if (roomManager == null)
        // {
        //     GameObject[] allManagers = GameObject.FindGameObjectsWithTag("RoomManager");

        //     foreach (GameObject manager in allManagers)
        //     {
        //         if (manager.name.Contains("RoomBoss"))
        //             roomManager = manager.GetComponent<RoomEventManager>();

        //         if (roomManager == null)
        //             Debug.Log("[SecretText] couldn't find room manager");
        //     }
        // }

        if (stateManager == null)
        {
            stateManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();

            if (stateManager == null)
                Debug.Log("[SecretText] stateManager null");
        }

        gameObject.SetActive(false);


    }

    private void Update()
    {
        if (stateManager != null)
        {
            if (stateManager.getGameState() == GameState.Victory)
            {
                gameObject.SetActive(true);
            }
        }
    }
}
