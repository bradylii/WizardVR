using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    Player player;
    ItemDrop dropItemScript;
    private void Start()
    {
        player = GameObject.Find("Game Manager")?.GetComponent<Player>();
        dropItemScript = GetComponent<ItemDrop>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "weapon")
        {
            Destroy(gameObject);
            player.killedBadGuy();
            dropItemScript.dropItem();
        }
    }
}
