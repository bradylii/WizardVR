using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignLayerToChildren : MonoBehaviour
{
    public string obstacleLayerName;
    // Start is called before the first frame update
    void Start()
    {
        SetLayer(transform, LayerMask.NameToLayer(obstacleLayerName));
    }

    void SetLayer(Transform parent, int layer)
    {
        parent.gameObject.layer = layer;

        foreach (Transform child in parent)
        {
            SetLayer(child, layer);
        }
    }
}
