using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public GameState currentState;

    public static GameStateManager Instance;

    public GameObject gameOverInterface;

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
       setGameState(GameState.Playing);
    }

    // To set game state and update accordingly
    public void setGameState(GameState newState)
    {
        currentState = newState;
        updateGameState();
    }

    private IEnumerator LoadLobby()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Lobby"); // Load the "Lobby" scene
        while (!asyncLoad.isDone)
        {
            yield return null; // Wait until the scene is fully loaded
        }
    }

    // To handle calling actions when game state changes
    public void updateGameState()
    {
        switch (currentState)
        {
            case GameState.Lobby:
                Debug.Log("Game is in loading screen");
                lobby();
                break;
            case GameState.Playing:
                Debug.Log("Game is being played");
                playing();
                break;
            case GameState.GameOver:
                Debug.Log("Game Over");
                gameOver();
                break;
            case GameState.Victory:
                Debug.Log("Victory!");
                victory();
                break;
        }
    }

    // To preform actions and configurations in loading screen/lobby
    public void lobby()
    {
        StartCoroutine(LoadLobby());
        gameOverInterface.SetActive(false);
    }

    // To preform actions and configurations when playing game
    public void playing()
    {
        gameOverInterface.SetActive(false);
    }

    // To preform actions and configurations when game is over
    // loss confetti
    // loss display (with stats)
    // option to go back to lobby
    public void gameOver()
    {
        gameOverInterface.SetActive(true);
    }

    // To preform actions and configurations when player wins
    // win confetti
    // win display (with stats)
    // option to go back to lobby
    public void victory()
    {
        gameOverInterface.SetActive(true);
    }

}
