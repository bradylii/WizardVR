using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
#if UNITY_EDITOR
using UnityEditor.Build;
#endif
using System.Collections;

public class GrabTracker : MonoBehaviour
{
    [SerializeField] private StartingWandToEnterGame wandScript;

    [SerializeField] private bool grabbedTriggered = false;

    private void Start()
    {
        if (wandScript == null)
        {
            wandScript = GetComponent<StartingWandToEnterGame>();
        }
    }
    public void grabbed()
    {
        Debug.Log("[GRABTRACKER] Grabbed");

        if (!grabbedTriggered)
        {
            grabbedTriggered = true;
            
            if (wandScript != null)
            {
                wandScript.LoadGame();
            }
            else
                Debug.LogError("[GrabTracker] wandScript null");
        }
    }

}