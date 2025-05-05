using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryFollowCamera : MonoBehaviour
{
    [SerializeField] private Transform playerHead;

    [SerializeField] private float forwardOffset = 0.5f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float initialDelay = 0.5f;
    [Range(0f, 1f)]
    [SerializeField] private float beltHeightPercent = 0.35f;

    private bool initialPositionSet = false;
    private Vector3 lockedPosition;
    [SerializeField] private float delayTimer = 0f;

    private void LateUpdate()
    {
        if (playerHead == null)
        {
            Debug.Log("[Inventory] PlayerHead is still null, trying to find...");
            playerHead = GameObject.FindGameObjectWithTag("MainCamera")?.transform;

            if (playerHead == null)
                Debug.Log("[Inventory] Couldnt find player head");
            return;
        }

        if (!initialPositionSet)
        {
            delayTimer += Time.deltaTime;

            if (delayTimer >= initialDelay)
            {
                Debug.Log("[Inventory] Setting position");

                float playerHeight = playerHead.position.y;
                float beltHeight = playerHeight * (beltHeightPercent);

                lockedPosition = new Vector3(playerHead.position.x, beltHeight, playerHead.position.z);
                transform.position = lockedPosition;

                initialPositionSet = true;
            }
            
        }

        if (initialPositionSet)
        {
            Vector3 headEuler = playerHead.rotation.eulerAngles;
            Quaternion targetRotation = Quaternion.Euler(0f, headEuler.y, 0f);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            Vector3 offset = transform.forward * forwardOffset;

            transform.position = new Vector3(playerHead.position.x, lockedPosition.y, playerHead.position.z) + offset;
        }
    }


}
