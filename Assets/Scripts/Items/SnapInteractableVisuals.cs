using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SnapInteractableVisuals : MonoBehaviour
{
    [SerializeField] private SnapInteractable snapInteractable;
    [SerializeField] private Material hoverMaterial;

    [SerializeField] private GameObject currentInteractorGameObject;
    [SerializeField] private SnapInteractor currentInteractor;

    [SerializeField] private float imageSize = 0.5f;

    private void OnEnable()
    {
        snapInteractable.WhenInteractorAdded.Action += WhenInteractorAdded_Action;
        snapInteractable.WhenSelectingInteractorViewAdded += SnapInteractable_WhenSelectingInteractorViewAdded;
        snapInteractable.WhenInteractorViewRemoved += SnapInteractable_WhenInteractorViewRemoved;
        snapInteractable.WhenInteractorViewAdded += SnapInteractable_WhenInteractorViewAdded;
    }

    private void WhenInteractorAdded_Action(SnapInteractor obj)
    {
        if (currentInteractor == null)
            currentInteractor = obj;
        else if (currentInteractor != obj)
        {
            currentInteractor = obj;
            var tempGP = currentInteractorGameObject;
            Destroy(tempGP);
            currentInteractorGameObject = null;
        }
        else
            return;

        SetupGhostModel(obj);
    }

    private void SnapInteractable_WhenSelectingInteractorViewAdded(IInteractorView obj)
    {
        if (currentInteractorGameObject != null)
            currentInteractorGameObject.SetActive(false);
            
        if (currentInteractorGameObject == null)
        {
            Debug.LogWarning("[SnapVisual] Tried to set active but ghost model isn't initialized yet.");
            return;
        }
    }

    private void SnapInteractable_WhenInteractorViewAdded(IInteractorView obj)
    {
        if (currentInteractorGameObject != null)
            currentInteractorGameObject.SetActive(true);

        if (currentInteractorGameObject == null)
        {
            Debug.LogWarning("[SnapVisual] Tried to set active but ghost model isn't initialized yet.");
            return;
        }
    }

    private void SnapInteractable_WhenInteractorViewRemoved(IInteractorView obj)
    {
        if (currentInteractorGameObject != null)
            currentInteractorGameObject.SetActive(false);
        
        if (currentInteractorGameObject == null)
        {
            Debug.LogWarning("[SnapVisual] Tried to set active but ghost model isn't initialized yet.");
            return;
        }
    }

    private void SetupGhostModel(SnapInteractor interactor)
    {
        currentInteractorGameObject = new GameObject("GhostModel");

        // 👇 Parent it to the SnapInteractable so it inherits its local position & rotation
        currentInteractorGameObject.transform.SetParent(snapInteractable.transform, false);
        currentInteractorGameObject.transform.localPosition = Vector3.zero;
        currentInteractorGameObject.transform.localRotation = Quaternion.identity;
        currentInteractorGameObject.transform.localScale = Vector3.one * imageSize; // Optional: scale control

        //GameObject visualSource = snapInteractable.gameObject;

        //Debug.Log("[SnapVisual] Using object: " + visualSource.name);


        //var rootMesh = visualSource.GetComponent<MeshFilter>();
        //if (rootMesh != null)
        //{
        //    var rootGO = new GameObject("RootVisual");
        //    rootGO.transform.SetParent(currentInteractorGameObject.transform, false);
        //    rootGO.transform.localPosition = Vector3.zero;
        //    rootGO.transform.localRotation = Quaternion.identity;
        //    rootGO.transform.localScale = Vector3.one;

        //    var mf = rootGO.AddComponent<MeshFilter>();
        //    mf.sharedMesh = rootMesh.sharedMesh;

        //    var mr = rootGO.AddComponent<MeshRenderer>();
        //    mr.material = hoverMaterial;
        //}

        //// Handle child meshes
        //var childMeshes = visualSource.GetComponentsInChildren<MeshFilter>();
        //foreach (var item in childMeshes)
        //{
        //    if (item.gameObject == visualSource) continue; // skip self if already added

        //    var newGo = new GameObject(item.name);
        //    newGo.transform.SetParent(currentInteractorGameObject.transform, false);
        //    newGo.transform.localPosition = item.transform.localPosition;
        //    newGo.transform.localRotation = item.transform.localRotation;
        //    newGo.transform.localScale = item.transform.localScale;

        //    var mf = newGo.AddComponent<MeshFilter>();
        //    mf.sharedMesh = item.sharedMesh;

        //    var mr = newGo.AddComponent<MeshRenderer>();
        //    mr.material = hoverMaterial;
        //}


        // Try to copy the mesh from the interactor's parent
        var parentMesh = interactor.transform.parent.GetComponent<MeshFilter>();
        if (parentMesh != null)
        {
            var mf = currentInteractorGameObject.AddComponent<MeshFilter>();
            mf.mesh = parentMesh.mesh;

            var mr = currentInteractorGameObject.AddComponent<MeshRenderer>();
            mr.material = hoverMaterial;
        }

        // Also clone any children meshes
        var childMesh = interactor.transform.parent.GetComponentsInChildren<MeshFilter>();
        foreach (var item in childMesh)
        {
            if (item == parentMesh) continue; // avoid duplicating the root

            var newGo = new GameObject(item.name);
            newGo.transform.SetParent(currentInteractorGameObject.transform, false);
            newGo.transform.localPosition = item.transform.localPosition;
            newGo.transform.localRotation = item.transform.localRotation;
            newGo.transform.localScale = item.transform.localScale * imageSize; 

            var mf = newGo.AddComponent<MeshFilter>();
            mf.mesh = item.mesh;

            var mr = newGo.AddComponent<MeshRenderer>();
            mr.material = hoverMaterial;
        }
    }
}
