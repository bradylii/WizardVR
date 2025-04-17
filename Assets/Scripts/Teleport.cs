using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public float x;
    public float y;
    public float z;

    public bool teleport = false;

    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (teleport)
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            Debug.Log("[TELEPORT] Navmesh disabled");
            transform.position = new Vector3(x, y, z);
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;
            Debug.Log("[TELEPORT] Rigidbody altered");
            Debug.Log("[TELEPORT] Teleported");
            teleport = false;

            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
            Debug.Log("[TELEPORT] Navmesh enabled");

        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
            Debug.Log("[TELEPORT] Navmesh enabled");
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = false;
            Debug.Log("[TELEPORT] Rigidbody altered");

        }
    }
}


