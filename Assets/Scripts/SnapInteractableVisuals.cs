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

    private GameObject currentInteractorGameObject;
    private SnapInteractor currentInteractor;

    public float imageSize;

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
        currentInteractorGameObject?.SetActive(false);
    }

    private void SnapInteractable_WhenInteractorViewAdded(IInteractorView obj)
    {
        currentInteractorGameObject?.SetActive(true);
    }

    private void SnapInteractable_WhenInteractorViewRemoved(IInteractorView obj)
    {
        currentInteractorGameObject.SetActive(false);
    }

    private void SetupGhostModel(SnapInteractor interactor)
    {
        currentInteractorGameObject = new GameObject("GhostModel");

        // 👇 Parent it to the SnapInteractable so it inherits its local position & rotation
        currentInteractorGameObject.transform.SetParent(snapInteractable.transform, false);
        currentInteractorGameObject.transform.localPosition = Vector3.zero;
        currentInteractorGameObject.transform.localRotation = Quaternion.identity;
        currentInteractorGameObject.transform.localScale = Vector3.one; // Optional: scale control

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
            newGo.transform.localScale = item.transform.localScale;

            var mf = newGo.AddComponent<MeshFilter>();
            mf.mesh = item.mesh;

            var mr = newGo.AddComponent<MeshRenderer>();
            mr.material = hoverMaterial;
        }
    }
}
