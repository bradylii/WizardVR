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
        GameState stateToLoad;

        if (System.Enum.TryParse(sceneName, out stateToLoad))
        {
            gameStateManager.setGameState(stateToLoad);
        }
        else
        {
            Debug.Log("[ReloadScene] Invalid gamestate: " + sceneName);
        }
    }

    public void ReloadCurrentScene()
    {
        gameStateManager.setGameState(GameState.Playing);
    }

    public void goToMainMenu()
    {
        gameStateManager.setGameState(GameState.MainMenu);
    }
}
