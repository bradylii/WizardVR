using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public float x;
    public float y;
    public float z;

    public bool teleport = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (teleport)
        {
            transform.position = new Vector3(x, y, z);
            teleport = false;
        }
    }
}
