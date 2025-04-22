using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    public float damage;
    Collider swordCollider;
    Rigidbody swordRigidbody;

    private void Start()
    {
        if (swordCollider == null)
        {
            swordCollider = GetComponent<Collider>();
        }

        if (swordRigidbody == null)
        {
            swordRigidbody = GetComponent<Rigidbody>();
        }
    }

    public void OnGrabbed()
    {
        swordCollider.isTrigger = true;
        swordRigidbody.isKinematic = true;
    }

    public void OnReleased()
    {
        swordCollider.isTrigger = false;
        swordRigidbody.isKinematic = false;
    }
}
