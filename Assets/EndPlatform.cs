using UnityEngine;

public class EndPlatform : MonoBehaviour
{
    public GameStateManager gameStateManager;
    public Renderer platformRenderer; // Assign this in the Inspector

    private void Start()
    {
        if (platformRenderer == null)
            platformRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player (XR Camera Rig) enters the trigger
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the end of the maze!");
            platformRenderer.material.color = Color.green; // Make platform green
            gameStateManager.setGameState(GameState.Victory); // Set state to Victory
        }
    }
}
