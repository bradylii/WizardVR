using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasFollowPlayer : MonoBehaviour
{
    public Transform playerCamera;
    public float followSpeed = 5f;

    public float minimumHeight = 1.5f;
    public float horizontalOffset = 0.5f;

    public bool followPlayer = true;

    public float forwardOffset = 4f;

    public bool isMenu = false;
    private void Start()
    {
        if (!followPlayer)
        {
            Vector3 targetPosition = playerCamera.position + playerCamera.forward * 2.0f;

            targetPosition += playerCamera.right * horizontalOffset;
            targetPosition.y = Mathf.Max(targetPosition.y, minimumHeight);
        }
        if (playerCamera == null)
        {
           playerCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
    }

    private void Update()
    {
        if (followPlayer)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - playerCamera.position);
            Vector3 targetPosition = playerCamera.position + playerCamera.forward * 2.0f;

            if (isMenu)
                targetPosition = playerCamera.position + playerCamera.forward * forwardOffset;

            targetPosition += playerCamera.right * horizontalOffset;

            if (playerCamera.position.y > minimumHeight)
            {
                targetPosition.y = Mathf.Max(targetPosition.y, minimumHeight);
            }
            else
            {
                targetPosition.y = playerCamera.position.y;
            }
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }

        

    }
}
