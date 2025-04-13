using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public float spawnHeight = 2f;

    public void dropItem()
    {
        if (itemPrefabs.Length == 0) return;

        int randomIndex = Random.Range(0, itemPrefabs.Length);
        GameObject selectedItem = itemPrefabs[randomIndex];

        Vector3 spawnPosition = new Vector3(transform.position.x, spawnHeight, transform.position.z);
        Instantiate(selectedItem, spawnPosition, Quaternion.identity);
    }
}
