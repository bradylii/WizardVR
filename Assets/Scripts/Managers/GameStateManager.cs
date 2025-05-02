using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameStateManager : MonoBehaviour
{
    public GameState currentState;
    public ScenesManager sceneManager;

    public static GameStateManager Instance;

    public GameObject player;

    public GameObject secretText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    // start game state as loading screen
    void Start()
    {

        secretText = GameObject.FindGameObjectWithTag("SecretTextUI");
        if (secretText != null)
            secretText.SetActive(false);
        else    
            Debug.Log("[GameStatemanager] Secret Text Null");

        OVRManager.display.RecenterPose();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        GameObject spawn = GameObject.FindGameObjectWithTag("Spawn");

        if (player != null && spawn != null)
        {
            player.transform.position = spawn.transform.position;
        }
        else 
            Debug.Log("[GameStateManager] Player or spawn null");
    }


    // To set game state and update accordingly
    public void setGameState(GameState newState)
    {
        currentState = newState;
        updateGameState();
    }

    public GameState getGameState()
    {
        return currentState;
    }

    // To handle calling actions when game state changes
    public void updateGameState()
    {
        switch (currentState)
        {
            case GameState.MainMenu:
                Debug.Log("[GAMESTATE] Main Menu is being played");
                sceneManager.MainMenu();
                break;
            case GameState.Lobby:
                Debug.Log("[GAMESTATE] Game is in loading screen");
                sceneManager.Lobby();
                break;
            case GameState.Playing:
                Debug.Log("[GAMESTATE] Game is being played");
                sceneManager.Playing();
                break;
            case GameState.GameOver:
                Debug.Log("[GAMESTATE] Game Over");
                sceneManager.GameOver();
                break;
            case GameState.Victory:
                Debug.Log("[GAMESTATE] Victory!");
                sceneManager.Victory();
                break;
        }
    }
}
