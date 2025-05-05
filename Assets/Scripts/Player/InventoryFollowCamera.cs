using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryFollowCamera : MonoBehaviour
{
    [SerializeField] private Transform playerHead;

    [SerializeField] private float forwardOffset = 0.5f;
    [SerializeField] private float rotationSpeed = 2f;

    private float initialY;

    private void Start()
    {
        if (playerHead == null)
            playerHead = GameObject.FindGameObjectWithTag("MainCamera")?.transform;

        initialY = transform.position.y;
    }


    private void LateUpdate()
    {
            Vector3 headEuler = playerHead.rotation.eulerAngles;
            Quaternion targetRotation = Quaternion.Euler(0f, headEuler.y, 0f);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            Vector3 offset = transform.forward * forwardOffset;
            transform.position = new Vector3(playerHead.position.x, initialY, playerHead.position.z) + offset;
    }


}
