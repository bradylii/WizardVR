using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public GameState currentState;

    private void Update()
    {
        KeyboardInput();
    }

    public void MainMenu()
    {
        Debug.Log("[GameState] mainMenu()");
        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator LoadMainMenu()
    {
        Debug.Log("[GAMESTATE] LoadMainMenu()");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu"); // Load the "Lobby" scene
        while (!asyncLoad.isDone)
        {
            yield return null; // Wait until the scene is fully loaded
        }

        yield return null;

        currentState = GameState.MainMenu; 
    }


    public void Lobby()
    {
        Debug.Log("[GAMESTATE] Lobby()");
        StartCoroutine(LoadLobby());
    }
    private IEnumerator LoadLobby()
    {
        Debug.Log("[GAMESTATE] LoadLobby()");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Lobby"); // Load the "Lobby" scene
        while (!asyncLoad.isDone)
        {
            yield return null; // Wait until the scene is fully loaded
        }

        currentState = GameState.Lobby;

        Transform playerHead = GameObject.FindGameObjectWithTag("MainCamera").transform;
        Transform headTarget = GameObject.Find("HeadPosition").transform;
        playerHead.position = headTarget.position;
    }


    public void Playing()
    {
        Debug.Log("[GAMESTATE] playing()");
        StartCoroutine(LoadPlaying());
    }
    private IEnumerator LoadPlaying()
    {
        Debug.Log("[GAMESTATE] LoadPlaying()");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("CastleScene"); // Load the "Lobby" scene
        while (!asyncLoad.isDone)
        {
            yield return null; // Wait until the scene is fully loaded
        }

        currentState = GameState.Playing;
    }

    public void GameOver()
    {
        Debug.Log("[GameState] gameOver()");
    }


    public void Victory()
    {
        Debug.Log("[GAMESTATE] victory()");

        
    }

    private void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Playing();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Lobby();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Victory();
        }
    }
}
