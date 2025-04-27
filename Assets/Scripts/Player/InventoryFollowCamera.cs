using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryFollowCamera : MonoBehaviour
{
    public Transform playerHead;

    public float minimumHeight = 1f;
    public float verticalOffset = 0.5f;

    private bool initialPositionSet = false;
    private Vector3 lockedPosition;

    private void LateUpdate()
    {
        if (playerHead == null)
        {
            playerHead = GameObject.FindGameObjectWithTag("MainCamera").transform;

            if (playerHead == null)
                Debug.Log("[Inventory] Couldnt find player head");
            return;
        }

        if (!initialPositionSet)
        {
            float desiredY = playerHead.position.y - verticalOffset;
            float finalY = Mathf.Max(desiredY, minimumHeight);

            lockedPosition = new Vector3(playerHead.position.x, finalY, playerHead.position.z);
            transform.position = lockedPosition;

            initialPositionSet = true;
        }

        Vector3 headUeler = playerHead.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, headUeler.y, 0f);

        transform.position = new Vector3(playerHead.position.x, lockedPosition.y, playerHead.position.z);
    }


}
