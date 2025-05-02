using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : MonoBehaviour
{
    public float speedBoost = 2f;


    public float deactivateDelay = 2f;
    public float resetSpeedDelay = 5f;
    public float currentSpeed;


    public Transform player;
    public Transform playerHead;
    JoyStickMove playerMovement;

    public string manualButton;

    [SerializeField] private AudioClip drinkSound;
    private AudioSource audioSource;

    private void Start()
    {
        if (playerMovement == null)
            playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<JoyStickMove>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (Enum.TryParse(manualButton, out KeyCode key))
        {
            if (Input.GetKeyDown(key))
            {
                Debug.Log($"[{manualButton}] key pressed");
                ActivateSpeed();
            }
        }
    }

    public void ActivateSpeed()
    {
        currentSpeed = playerMovement.speed;
        playerMovement.speed += speedBoost;
        Debug.Log("[POTION] Speed Potion Used. Player Faster.");

        StartCoroutine(HandlePotion());
        
    }

    private IEnumerator HandlePotion()
    {
        yield return new WaitForSeconds(deactivateDelay);

        MeshRenderer[] childrenRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in childrenRenderers) 
        {
            renderer.enabled = false;
        }

        GetComponent<Collider>().enabled = false;
        Debug.Log("[POTION] Speed Potion Dissapear");




        yield return new WaitForSeconds(resetSpeedDelay - deactivateDelay);
        playerMovement.speed -= speedBoost;
        Debug.Log("[POTION] Speed reset");

        Destroy(gameObject);
    }
}
