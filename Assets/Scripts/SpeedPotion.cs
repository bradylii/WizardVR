using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : MonoBehaviour
{
    public float speedBoost = 2f;
    public float drinkDistance = 0.3f;
    public float drinkAngleThreshold = 60f;

    public float deactivateDelay = 2f;
    public float resetSpeedDelay = 5f;
    public float currentSpeed;

    public float distance;
    public float angle;

    private bool used = false;

    public Transform player;
    public Transform playerHead;
    JoyStickMove playerMovement;

    public string manualButton;

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.Log("[POTION] Player object not found in the scene");
            }

        }
        if (playerHead == null)
        {
            playerHead = player?.Find("CenterEyeAnchor");
        }
        if (playerHead == null)
        {
            Debug.Log("[POTION] No playerhead found");
        }

        playerMovement = player?.GetComponent<JoyStickMove>();
        if (playerMovement == null)
        {
            Debug.Log("[POTION] JoystickMove component not found");
        }
    }

    private void Update()
    {

        if (used || player == null || playerMovement == null) return;

        distance = Vector3.Distance(transform.position, playerHead.position);
        angle = Vector3.Angle(transform.up, Vector3.down);

        if (distance < drinkDistance && angle < drinkAngleThreshold)
        {
            ActivateSpeed();
        }

        if (Enum.TryParse(manualButton, out KeyCode key))
        {
            if (Input.GetKeyDown(key))
            {
                Debug.Log($"[{manualButton}] key pressed");
                ActivateSpeed();
            }
        }
    }

    private void ActivateSpeed()
    {
        currentSpeed = playerMovement.speed;
        playerMovement.speed += speedBoost;
        used = true;
        Debug.Log("[POTION] Speed Potion Used. Player Faster.");

        StartCoroutine(ResetSpeed());
        StartCoroutine(DestroyPotion());
    }

    private IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(resetSpeedDelay);
        Debug.Log("[POTION] Speed reset");
        playerMovement.speed -= speedBoost;

        Destroy(gameObject);


    }

    private IEnumerator DestroyPotion()
    {
        yield return new WaitForSeconds(deactivateDelay);
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        //gameObject.SetActive(false);
        Debug.Log("[POTION] Speed Potion Dissapear");

        
    }
}
