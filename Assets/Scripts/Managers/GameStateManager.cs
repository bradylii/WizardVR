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
                sceneManager.MainMenu();
                break;
            case GameState.Lobby:
                sceneManager.Lobby();
                break;
            case GameState.Playing:
                sceneManager.Playing();
                break;
            case GameState.GameOver:
                sceneManager.GameOver();
                break;
            case GameState.Victory:
                sceneManager.Victory();
                break;
        }
    }

    public void MainMenuScene()
    {
        Debug.Log("[GAMESTATE] Main Menu is being played");
        currentState = GameState.MainMenu; 
    }

    public void LobbyScene()
    {
        Debug.Log("[GAMESTATE] Game is in loading screen");
        currentState = GameState.Lobby;
    }

    public void PlayingScene()
    {
        Debug.Log("[GAMESTATE] Game is being played");
        currentState = GameState.Playing;
    }

    public void GameOverScene()
    {
        Debug.Log("[GAMESTATE] Game Over");
        currentState = GameState.GameOver;
    }

    public void VictoryScene()
    {
        Debug.Log("[GAMESTATE] Victory!");
        currentState = GameState.Victory;
    }

   

}
