using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomControllerModels : MonoBehaviour
{
    public GameObject rightControllerModelPrefab; // Your custom right controller model
    public Transform trackingSpace;
    private GameObject rightControllerInstance;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    

    void Update()
    {
        if (rightControllerInstance != null && trackingSpace != null && OVRInput.IsControllerConnected(OVRInput.Controller.RTouch))
        {
            // Set the position and rotation of the custom right controller model
            rightControllerInstance.transform.position = trackingSpace.TransformPoint(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));
            rightControllerInstance.transform.rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("[CustomController] Scene Loaded: " + scene.name);

        if (trackingSpace == null)
        {
            Debug.Log("[CustomController] trackingSpace is null... trying to find now");
            trackingSpace = GameObject.Find("TrackingSpace").transform;

            if (trackingSpace == null)
                Debug.Log("[CustomController] trackingSpace couldnt' be assigned");
        }

        if (rightControllerModelPrefab == null)
        {
            Debug.Log("[CustomController] Rightcontrollermodel null... tyring to find");
            rightControllerModelPrefab = Resources.Load<GameObject>("Prefabs/Wand Pivot Variant");

            if (rightControllerModelPrefab == null)
                Debug.Log("[CustomController] RightControllerModel Couldn't be assigned");
        }


        if (rightControllerModelPrefab != null)
        {
            rightControllerInstance = Instantiate(rightControllerModelPrefab);
            rightControllerInstance.SetActive(true);
            Debug.Log("[CustomController] Instantiated model");
        }

        // Hide the default controller models if needed
        GameObject defaultRightController = GameObject.Find("RightController");
        if (defaultRightController != null)
            defaultRightController.SetActive(false);
        else
            Debug.Log("[CustomController] DefaultRightController null");
    }
}
