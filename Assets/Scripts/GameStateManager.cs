using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public GameState currentState;

    public static GameStateManager Instance;

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
        currentState = GameState.Lobby;
    }

    // To set game state and update accordingly
    public void setGameState(GameState newState)
    {
        currentState = newState;
        updateGameState();
    }

    // To handle calling actions when game state changes
    public void updateGameState()
    {
        switch (currentState)
        {
            case GameState.Lobby:
                Debug.Log("Game is in lobby");
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

    public void LoadLobbyScene()
    {
        StartCoroutine(LoadLobby());
    }

    private IEnumerator LoadLobby()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Lobby");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    void Update()
    {
        // TO DO: MOVE TO SEPARATE SCRIPT (MANAGING WIN/LOSS DISPLAY) AND REPLACE WITH XR INTERFACE INTERACTION
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadLobbyScene();
        }
    }

    // To preform actions and configurations in loading screen/lobby
    public void lobby()
    {

    }

    // To preform actions and configurations when playing game
    public void playing()
    {

    }

    // To preform actions and configurations when game is over
    // loss confetti
    // loss display (with stats)
    // option to go back to lobby
    public void gameOver()
    {

    }

    // To preform actions and configurations when player wins
    // win confetti
    // win display (with stats)
    // option to go back to lobby
    public void victory()
    {

    }

}
