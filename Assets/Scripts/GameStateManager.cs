using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public GameState currentState;

    public static GameStateManager Instance;

    bool renderedUI = false;

    public GameObject player;

    public GameObject secretText;

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
        // setGameState(GameState.MainMenu);
        currentState = GameState.Playing;

        secretText = GameObject.FindGameObjectWithTag("SecretTextUI");
        secretText.SetActive(false);

        OVRManager.display.RecenterPose();


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            setGameState(GameState.Playing);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            setGameState(GameState.Lobby);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            setGameState(GameState.Victory);
        }


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
            case GameState.MainMenu:
                Debug.Log("[GAMESTATE] Main Menu is being played");
                mainMenu();
                break;

        }
    }


    // To preform actions and configurations in loading screen/lobby
    public void lobby()
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

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        GameObject spawn = GameObject.FindGameObjectWithTag("Spawn");

        if (player != null && spawn != null)
        {
            player.transform.position = spawn.transform.position;
        }
    }

    public void mainMenu()
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

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        GameObject spawn = GameObject.FindGameObjectWithTag("Spawn");

        if (player != null && spawn != null)
        {
            player.transform.position = spawn.transform.position;
        }
    }


    // To preform actions and configurations when playing game
    public void playing()
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

        GameObject gameManager = gameObject;
        if (gameManager.GetComponent<Player>() == null)
            gameManager.AddComponent<Player>();
        if (gameManager.GetComponent<Wand>() == null)
            gameManager.AddComponent<Wand>();
        if (gameManager.GetComponent<CustomControllerModels>() == null)
        {
            CustomControllerModels customControllers = gameManager.AddComponent<CustomControllerModels>();
            customControllers.manualSceneInit();
        }
        if (gameManager.GetComponent<TurnOffOnUI>() == null)
        {
            TurnOffOnUI uiManager =  gameManager.AddComponent<TurnOffOnUI>();
            uiManager.manualSceneInit();
        }

    }





    // To preform actions and configurations when game is over
    // loss confetti
    // loss display (with stats)
    // option to go back to lobby
    public void gameOver()
    {
        Debug.Log("[GameState] gameOver()");
    }

    // To preform actions and configurations when player wins
    // win confetti
    // win display (with stats)
    // option to go back to lobby
    public void victory()
    {
        Debug.Log("[GAMESTATE] victory()");

        secretText.SetActive(true);
    }


}
