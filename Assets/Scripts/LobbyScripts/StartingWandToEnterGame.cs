using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Transform))]
public class StartingWandToEnterGame : MonoBehaviour
{
    public GameStateManager gameStateManager;

    private Vector3 objectPos;
    private float changingValue = 1;
    private float maxPercent = 1.2f;
    private float minPercent = 0.8f;
    private float speed = 1.5f;
    private void Start()
    {
        objectPos = this.transform.position;

        if (gameStateManager == null)
        {
            gameStateManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
        }
    }

    private void Update()
    {
        bounceObject();
    }

    private void bounceObject()
    {
        float t = (Mathf.Sin(Time.time * speed) + 1) / 2;
        changingValue = Mathf.Lerp(minPercent, maxPercent, t);
        transform.position = new Vector3(objectPos.x, (objectPos.y * changingValue), objectPos.z);
    }

    // Load with StartCoroutine(LoadSceneAsync());
    public void LoadGame()
    {
        if (gameStateManager != null)
        {
            gameStateManager.setGameState(GameState.Playing);
        }
    }
}
