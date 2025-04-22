using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public GameState currentState;

    public static GameStateManager Instance;

    public GameObject gameOverInterface;

    bool renderedUI = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
        // setGameState(GameState.Playing);
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One) || Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("[GAMESTATE] A button pressed");
            if (renderedUI)
            {
                setGameState(GameState.Playing);
                renderedUI = false;
            }
            else
            {
                setGameState(GameState.GameOver);
                renderedUI = true;
            }
        }
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

    private IEnumerator LoadPlaying()
    {
        Debug.Log("[GAMESTATE] LoadPlaying() started");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("CastleScene"); // Load the "Lobby" scene
        while (!asyncLoad.isDone)
        {
            yield return null; // Wait until the scene is fully loaded
        }

        GameObject gameManager = gameObject;
        if (gameManager.GetComponent<Player>() == null)
            gameManager.AddComponent<Player>();
        if (gameManager.GetComponent<Wand>() == null)
            gameManager.AddComponent<Wand>();
        if (gameManager.GetComponent<CustomControllerModels>() == null)
            gameManager.AddComponent<CustomControllerModels>();
        
    }

    // To handle calling actions when game state changes
    public void updateGameState()
    {
        switch (currentState)
        {
            case GameState.Lobby:
                Debug.Log("[GAMESTATE] Game is in loading screen");
                lobby();
                break;
            case GameState.Playing:
                Debug.Log("[GAMESTATE] Game is being played");
                playing();
                break;
            case GameState.GameOver:
                Debug.Log("[GAMESTATE] Game Over");
                gameOver();
                break;
            case GameState.Victory:
                Debug.Log("[GAMESTATE] Victory!");
                victory();
                break;
        }
    }

    // To preform actions and configurations in loading screen/lobby
    public void lobby()
    {
        Debug.Log("[GameState] Lobby");
        if (gameOverInterface != null)
        {
            gameOverInterface.SetActive(false);
        }

        StartCoroutine(LoadLobby());        
    }

    // To preform actions and configurations when playing game
    public void playing()
    {
        if (gameOverInterface != null)
        {
            gameOverInterface.SetActive(false);
        }

        StartCoroutine(LoadPlaying());

        
    }

    // To preform actions and configurations when game is over
    // loss confetti
    // loss display (with stats)
    // option to go back to lobby
    public void gameOver()
    {
        if (gameOverInterface != null)
        {
            gameOverInterface.SetActive(true);
        }
        Debug.Log("[GameState] Game Over");
    }

    // To preform actions and configurations when player wins
    // win confetti
    // win display (with stats)
    // option to go back to lobby
    public void victory()
    {
        if (gameOverInterface != null)
        {
            gameOverInterface.SetActive(true);
        }
    }

}
