using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

public class GrabTracker : MonoBehaviour
{
    public void grabbed()
    {
        Debug.Log("[GRABTRACKER] Grabbed");
    }
}