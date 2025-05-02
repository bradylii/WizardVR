using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Potions : MonoBehaviour
{   
    public PotionTypes potionType;

    public float drinkDistance = 0.3f; 
    public float drinkAngleThreshold = 60f;

    public float distance;
    public float angle;

    public bool used = false;
    
    public Transform playerHead;
    public Transform player;

    [SerializeField] private AudioClip drinkSound;
    private AudioSource audioSource;



    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            if (player == null)
            {
                Debug.LogError("[POTION] Player object not found in the scene");
            }
        }
        if (playerHead == null)
        {
            playerHead = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }


        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            if (drinkSound == null)
                drinkSound = Resources.Load<AudioClip>("Audio/DrinkingMP3");
        }

        
    }

    private void Update()
    {
        if (used) return;

        distance = Vector3.Distance(transform.position, playerHead.position);
        angle = Vector3.Angle(transform.up, Vector3.down);

        if (distance < drinkDistance && angle < drinkAngleThreshold)
        {
            activatePotion();
        }
    }

    private void activatePotion()
    {
        if (drinkSound != null)
        {
            audioSource.PlayOneShot(drinkSound);
            used = true;
        }

        if (potionType == PotionTypes.Healing)
            GetComponent<HealthPotion>().ActivateHeal();
        else if (potionType == PotionTypes.Speed)
            if (potionType == PotionTypes.Healing)
            GetComponent<SpeedPotion>().ActivateSpeed();
    }
}
