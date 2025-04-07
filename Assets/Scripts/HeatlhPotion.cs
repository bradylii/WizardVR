using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatlhPotion : MonoBehaviour
{
    public float healAmount = 25f;
    public float drinkDistance = 0.3f;
    public float drinkAngleThreshold = 60f;
    public float destroyDelay = 2f;

    public float distance;
    public float angle;

    private bool used = false;

    public Transform player;
    public Transform playerHead;
    Player playerInfo;



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

        playerInfo = GameObject.Find("Game Manager")?.GetComponent<Player>();
    }

    private void Update()
    {
       
        if (used || player == null || playerInfo == null) return;

        distance = Vector3.Distance(transform.position, playerHead.position);
        angle = Vector3.Angle(transform.up, Vector3.down);

        if (distance < drinkDistance && angle < drinkAngleThreshold)
        {
            ActivateHeal();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ActivateHeal();
        }
    }

    private void ActivateHeal()
    {
        playerInfo.playerHealth = Mathf.Min(playerInfo.playerHealth + healAmount, 100f);
        used = true;
        Debug.Log("[POTION] Potion Used. Player Healed.");

        StartCoroutine(DestroyPotion());
    }

    private IEnumerator DestroyPotion()
    {
        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);
    }
}
