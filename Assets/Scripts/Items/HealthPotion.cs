using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{


    [SerializeField] private Potions potionsScript;
    [SerializeField] private bool used = false;
    [SerializeField] private Player playerInfo;
    [SerializeField] private float destroyDelay = 2f;
    [SerializeField] private float healAmount = 25f;



    private void Start()
    {
        if (potionsScript == null)
            potionsScript = GetComponent<Potions>();

        if (playerInfo == null)
            playerInfo = GameObject.FindGameObjectWithTag("GameManager")?.GetComponent<Player>();
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
