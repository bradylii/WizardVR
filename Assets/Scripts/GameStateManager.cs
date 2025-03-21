using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        currentState = GameState.Loading;
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
            case GameState.Loading:
                Debug.Log("Game is in loading screen");
                loading();
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
    public void loading()
    {

    }

    // To preform actions and configurations when playing game
    public void playing()
    {

    }

    // To preform actions and configurations when game is over
    public void gameOver()
    {

    }

    // To preform actions and configurations when player wins
    public void victory()
    {

    }

}
