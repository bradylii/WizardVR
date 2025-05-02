using UnityEngine;
using Oculus.Interaction;

public class GrabbableEvents : MonoBehaviour
{
    [SerializeField] private Grabbable _grabbable;
    [SerializeField] private GrabTracker _grabTracker;

    private void OnEnable()
    {
        if (_grabbable != null)
            _grabbable.WhenPointerEventRaised += HandlePointerEvent;
        else 
            Debug.LogError("[GrabbableEvents] grabbable null");
    }

    private void OnDisable()
    {
         if (_grabbable != null)
            _grabbable.WhenPointerEventRaised -= HandlePointerEvent;
        else 
            Debug.LogError("[GrabbableEvents] grabbable null");
    }

    private void HandlePointerEvent(PointerEvent evt)
    {
        if (evt.Type == PointerEventType.Select && _grabTracker != null)
        {
            _grabTracker.grabbed();
        }
        else 
            Debug.LogError("[GrabbableEvents] grabTracker null");
    }
}