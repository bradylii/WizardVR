using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEditor.Build.Content;

public class GrabTracker : MonoBehaviour
{
    public GameStateManager gameManager;

    private void Start()
    {
        if (gameManager == null)
        {
            Debug.Log("[GRABTRACKER] GameStateManager not set... trying to find now");
            gameManager = FindObjectOfType<GameStateManager>();
            if (gameManager == null)
            {
                Debug.Log("[GRABTRACKER] No GameStateManager Found");
            }
        }
    }
    public void grabbed()
    {
        Debug.Log("[GRABTRACKER] Grabbed");
        
        gameManager.setGameState(GameState.Playing);
    }
}