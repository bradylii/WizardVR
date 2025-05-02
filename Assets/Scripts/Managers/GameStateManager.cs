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

        // if (currentState == GameState.MainMenu)
        // {
        //     UnityEngine.UI.Button playButton = GameObject.Find("PlayButton")?.GetComponent<UnityEngine.UI.Button>();
        //     if (playButton != null)
        //     {
        //         playButton.onClick.RemoveAllListeners();
        //         playButton.onClick.AddListener(() => setGameState(GameState.Playing));
        //     }    
        // }

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

    void Update()
    {
        keyboardInput();
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

        yield return null;

        currentState = GameState.MainMenu; 
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

        Transform playerHead = GameObject.FindGameObjectWithTag("MainCamera").transform;
        Transform headTarget = GameObject.Find("HeadPosition").transform;
        playerHead.position = headTarget.position;
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


        // Set Game Manager Components if null
        
        GameObject gameManager = gameObject;

        Player playerInfo = GetComponent<Player>();
        if (playerInfo == null)
            playerInfo = gameManager.AddComponent<Player>();

        if (gameManager.GetComponent<Wand>() == null)
            gameManager.AddComponent<Wand>();

        if (gameManager.GetComponent<CustomControllerModels>() == null)
        {
            CustomControllerModels customControllers = gameManager.AddComponent<CustomControllerModels>();
            customControllers.manualSceneInit();
        }

        TurnOffOnUI uiManager =  gameManager.GetComponent<TurnOffOnUI>();
        if (uiManager == null)
        {
            uiManager = gameManager.AddComponent<TurnOffOnUI>();
        }

        // Reset 
        playerInfo.resetStats();
        uiManager.manualSceneInit();
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

    private void keyboardInput()
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

}
