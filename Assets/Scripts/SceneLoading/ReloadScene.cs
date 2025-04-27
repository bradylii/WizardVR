using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    public GameStateManager gameStateManager;

    private void Start()
    {
        if (gameStateManager == null)
        {
            gameStateManager = GameObject.Find("Game Manager")?.GetComponent<GameStateManager>();
        }
    }

    public void LoadSceneUsingName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void goToLobby()
    {
        gameStateManager.setGameState(GameState.MainMenu);
    }
}
