using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public GameObject itemPrefab;
    public float spawnHeight = 2f;

    public void dropItem()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, spawnHeight, transform.position.z);
        Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
    }
}
