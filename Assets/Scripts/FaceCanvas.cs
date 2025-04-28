using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FaceCanvas : MonoBehaviour
{
    public Transform playerHead;
    public float distanceFromPlayer = 2.0f;
    public float verticalOffset = 0.0f;


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
        if (playerHead == null)
        {
            playerHead = GameObject.FindGameObjectWithTag("MainCamera")?.transform;
        }

        Vector3 spawnPosition = playerHead.position + playerHead.forward * distanceFromPlayer; spawnPosition.y += verticalOffset;

        transform.position = spawnPosition;

        Vector3 lookDirection = transform.position - playerHead.position;
        lookDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDirection);
    }

    // private void Start()
    // {
    //     if (playerHead == null)
    //     {
    //         playerHead = GameObject.FindGameObjectWithTag("MainCamera")?.transform;
    //     }

    //     Vector3 forwardDirection = playerHead.rotation * Vector3.forward;
    //     Vector3 spawnPosition = playerHead.position + forwardDirection * distanceFromPlayer; 
    //     spawnPosition.y += verticalOffset;

    //     transform.position = spawnPosition;

    //     Vector3 lookDirection = transform.position - playerHead.position;
    //     lookDirection.y = 0;
    //     transform.rotation = Quaternion.LookRotation(lookDirection);
    // }
}
