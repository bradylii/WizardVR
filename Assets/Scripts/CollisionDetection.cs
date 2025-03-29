using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    Player player;
    private void Start()
    {
        player = GetComponent<Player>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "weapon")
        {
            Destroy(gameObject);
            player.killedBadGuy();
        }
    }
}
