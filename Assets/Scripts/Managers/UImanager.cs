using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImanager : MonoBehaviour
{
    public GameStateManager gameStateManager;
    public TurnOffOnUI turnOffOnUI;

    private void Start()
    {
        if (turnOffOnUI == null)
            turnOffOnUI = GameObject.FindGameObjectWithTag("UI").transform.Find("UIManagers").GetComponent<TurnOffOnUI>();
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One) || Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("[TurnOffOnUI] A button pressed");
            if (gameStateManager.getGameState() == GameState.Playing || gameStateManager.getGameState() == GameState.GameOver)
            {
                turnOffOnUI.SwitchRetryUI();
            }
            else if (gameStateManager.getGameState() == GameState.Victory)
            {
                turnOffOnUI.SwitchVictoryUI();
            }
            else
            {
                Debug.Log("[UIManager] Couldn't match gamestate");
            }
        }
    }

    public void VictoryUI()
    {
        turnOffOnUI.SwitchSecretTextUI();
    }


}
