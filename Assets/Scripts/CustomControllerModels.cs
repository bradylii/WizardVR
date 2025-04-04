using UnityEngine;

public class CustomControllerModels : MonoBehaviour
{
    public GameObject rightControllerModelPrefab; // Your custom right controller model
    public Transform trackingSpace;
    private GameObject rightControllerInstance;

    void Start()
    {
        if (rightControllerModelPrefab != null)
        {
            rightControllerInstance = Instantiate(rightControllerModelPrefab);
            rightControllerInstance.SetActive(true);
        }
        // Hide the default controller models if needed
        //GameObject defaultRightController = GameObject.Find("RightController");
        //if (defaultRightController != null)
        //    defaultRightController.SetActive(false);
    }

    void Update()
    {
        if (rightControllerInstance != null && OVRInput.IsControllerConnected(OVRInput.Controller.RTouch))
        {
            // Set the position and rotation of the custom right controller model
            rightControllerInstance.transform.position = trackingSpace.TransformPoint(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));
            rightControllerInstance.transform.rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        }
    }
}
