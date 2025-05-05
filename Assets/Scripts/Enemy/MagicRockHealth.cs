using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicRockHealth : MonoBehaviour
{
    [SerializeField] private Golem golemAi;

    [SerializeField] private float damage = 30;

    private void Start()
    {
        if (golemAi == null)
        {
            golemAi = GameObject.FindGameObjectWithTag("Golem").GetComponent<Golem>();
        }
    }

    public void wasHit()
    {
        Debug.Log("[Boulder] was hit");

        Rigidbody rb = GetComponent<Rigidbody>();

        rb.useGravity = true;

        golemAi.wasHit(damage);
    }   
}
