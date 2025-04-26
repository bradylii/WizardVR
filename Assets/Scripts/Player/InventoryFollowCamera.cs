using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryFollowCamera : MonoBehaviour
{
    public Transform playerHead;

    private void LateUpdate()
    {
        if (playerHead == null)
        {
            playerHead = GameObject.FindGameObjectWithTag("MainCamera").transform;

            if (playerHead == null)
                Debug.Log("[Inventory] Couldnt find player head");
            return;
        }

        Vector3 headUeler = playerHead.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, headUeler.y, 0f);
    }


}
