using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : MonoBehaviour
{
    [SerializeField] private float speedBoost = 2f;


    [SerializeField] private float deactivateDelay = 2f;
    [SerializeField] private float resetSpeedDelay = 5f;
    [SerializeField] private float currentSpeed;

    [SerializeField] private JoyStickMove playerMovement;

    [SerializeField] private string manualButton = "S";

    private void Start()
    {
        if (playerMovement == null)
            playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<JoyStickMove>();
    }

    private void Update()
    {
        if (Enum.TryParse(manualButton, out KeyCode key))
        {
            if (Input.GetKeyDown(key))
            {
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
