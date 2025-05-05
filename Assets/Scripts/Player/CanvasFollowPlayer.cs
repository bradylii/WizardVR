using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float followSpeed = 5f;

    [SerializeField] private float minimumHeight = 1.5f;
    [SerializeField] private float horizontalOffset = 0.5f;

    [SerializeField] private bool followPlayer = true;

    [SerializeField] private float forwardOffset = 4f;

    [SerializeField] private bool isMenu = false;
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
