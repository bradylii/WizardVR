using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    [SerializeField] private GameState currentState;
    public GameStateManager gameStateManager;

    private void Start()
    {
        if (gameStateManager == null)
            gameStateManager = GetComponent<GameStateManager>();
    }


    public void MainMenu()
    {
        Debug.Log("[ScenesManager] mainMenu()");
        StartCoroutine(LoadMainMenu());
    }

    public IEnumerator LoadMainMenu()
    {
        Debug.Log("[ScenesManager] LoadMainMenu()");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu"); // Load the "Lobby" scene
        while (!asyncLoad.isDone)
        {
            yield return null; // Wait until the scene is fully loaded
        }

        yield return null;

        currentState = GameState.MainMenu; 

        gameStateManager.MainMenuScene();
        
    }


    public void Lobby()
    {
        Debug.Log("[ScenesManager] Lobby()");
        StartCoroutine(LoadLobby());
        
    }
    private IEnumerator LoadLobby()
    {
        Debug.Log("[ScenesManager] LoadLobby()");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Lobby"); // Load the "Lobby" scene
        while (!asyncLoad.isDone)
        {
            yield return null; // Wait until the scene is fully loaded
        }

        currentState = GameState.Lobby;

        gameStateManager.LobbyScene();
    }


    public void Playing()
    {
        Debug.Log("[ScenesManager] playing()");
        StartCoroutine(LoadPlaying());
    }
    private IEnumerator LoadPlaying()
    {
        Debug.Log("[ScenesManager] LoadPlaying()");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("CastleScene"); // Load the "Lobby" scene
        while (!asyncLoad.isDone)
        {
            yield return null; // Wait until the scene is fully loaded
        }

        currentState = GameState.Playing;
    }

    public void GameOver()
    {
        Debug.Log("[ScenesManager] gameOver()");
        currentState = GameState.GameOver;

        gameStateManager.GameOverScene();
    }


    public void Victory()
    {
        Debug.Log("[ScenesManager] victory()");
        currentState = GameState.Victory;

        gameStateManager.VictoryScene();
        
    }
}
