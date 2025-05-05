using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Meta.XR.BuildingBlocks;
using UnityEngine;

public class SyncColliderToHead : MonoBehaviour
{
    [SerializeField] private Transform cameraRigRoot;
    [SerializeField] private Transform head;

    [SerializeField] private CapsuleCollider bodyCollider;
    void Start()
    {
        if (cameraRigRoot == null)
        {
            cameraRigRoot =  GameObject.Find("CenterEyeAnchor").transform;
        }
        if (head == null) 
        {
            head = GameObject.Find("[BuildingBlock] Camera Rig").transform;
        }
        if (bodyCollider == null) 
        {
            bodyCollider = GetComponent<CapsuleCollider>();
        }

        if (cameraRigRoot == null || head == null || bodyCollider == null)
        {
            Debug.Log("[SYNCCOLLIDER] Player component not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraRigRoot == null || head == null || bodyCollider == null)
            return;

        Vector3 headPositionLocal = cameraRigRoot.InverseTransformPoint(head.position);

        Vector3 newCenter = new Vector3(headPositionLocal.x, bodyCollider.height / 2f, headPositionLocal.z);

        bodyCollider.center = newCenter;


    }
}
