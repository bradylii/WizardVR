using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{


    public Potions potionsScript;
    private bool used = false;
    public Player playerInfo;
    public float destroyDelay = 2f;
    public float healAmount = 25f;



    private void Start()
    {
        if (potionsScript == null)
            potionsScript = GetComponent<Potions>();

        if (playerInfo == null)
            playerInfo = GameObject.Find("Game Manager")?.GetComponent<Player>();
    }

    private void Update()
    {
         if (Input.GetKeyDown(KeyCode.H))
        {
            ActivateHeal();
        }
    }

    public void ActivateHeal()
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
