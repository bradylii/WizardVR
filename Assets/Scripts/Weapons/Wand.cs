using System.Collections.Generic;
using UnityEngine;

public class Wand : MonoBehaviour
{
    [SerializeField] List<Spell> spells;
    [SerializeField] GameObject wandTipTransform;


    [Header("Audio")]
    [SerializeField] AudioClip castSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D soun


        if (castSound == null)
        {
            Debug.Log("[WAND] castSound is null... trying to find now");
            castSound = Resources.Load<AudioClip>("Audio/PewSound");
        }

        if (wandTipTransform == null)
        {
            Debug.Log("[WAND] wandTipTransform is null... trying to find now");
            wandTipTransform = GameObject.Find("[BuildingBlock] Camera Rig");
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Cast(0);
        }
    }

    public void Cast(int spellNum)
    {
        Spell spell = Instantiate(spells[(int)spellNum], wandTipTransform.transform.position, wandTipTransform.transform.rotation);
        spell.transform.forward = wandTipTransform.transform.forward;

        if (castSound != null && audioSource != null)
        {
            Debug.Log("[SOUND] Pew played");
            audioSource.PlayOneShot(castSound);
        }
        else
        {
            if (castSound == null)
                Debug.Log("[SOUND] Sound not played - castSound null");
            else
                Debug.Log("[SOUND] Sound not played - audioSource null");
        }
    }



}

