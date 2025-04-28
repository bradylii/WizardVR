using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnOffOnUI : MonoBehaviour
{
    public GameObject victoryUI;
    public GameObject retryUI;
    [SerializeField] private bool victoryActive = false;
    [SerializeField] private bool retryActive = false;

    [SerializeField] private GameStateManager stateManager;


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("[TurnOffOn] Scene Loaded: " + scene.name);

        victoryUI = GameObject.FindGameObjectWithTag("VictoryUI");
        if (victoryUI == null)
            Debug.Log("[TurnOffOn] VictoryUI not found");
        victoryUI.SetActive(false);

        retryUI = GameObject.FindGameObjectWithTag("RetryUI");
        if (victoryUI == null)
            Debug.Log("[TurnOffOn] RetryUI not found");
        retryUI.SetActive(false);

        stateManager = GetComponent<GameStateManager>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One) || Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("[TurnOffOnUI] A button pressed");
            if (stateManager.getGameState() == GameState.Playing || stateManager.getGameState() == GameState.GameOver)
            {
                turnOnRetry();
            }
            else if (stateManager.getGameState() == GameState.Victory)
            {
                turnOnVictory();
            }
            else
            {
                Debug.Log("[TurnOffOnUI] Couldn't match gamestate");
            }
        }
    }

    public void turnOnRetry()
    {
        retryUI.SetActive(!retryUI.activeSelf);
    }

    public void turnOnVictory()
    {
        victoryUI.SetActive(!victoryUI.activeSelf);
    }

    public void manualSceneInit()
    {
        Debug.Log("[TurnOffOnUI] Manually initializing UI after scene load.");

        victoryUI = GameObject.FindGameObjectWithTag("VictoryUI");
        if (victoryUI != null)
            victoryUI.SetActive(false);

        retryUI = GameObject.FindGameObjectWithTag("RetryUI");
        if (retryUI != null)
            retryUI.SetActive(false);

        stateManager = GetComponent<GameStateManager>();
    }
}
