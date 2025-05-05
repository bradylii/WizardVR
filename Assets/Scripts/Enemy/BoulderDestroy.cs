using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        destroyAfterTime();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
           Destroy(gameObject);
        }
    }

    IEnumerator destroyAfterTime()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("[BOULDER] Hit the player!");
        }
    }
}
