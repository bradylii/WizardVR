using UnityEngine;
using Oculus.Interaction;

public class GrabbableEvents : MonoBehaviour
{
    [SerializeField] private Grabbable _grabbable;
    [SerializeField] private GrabTracker _grabTracker;

    private void OnEnable()
    {
        _grabbable.WhenPointerEventRaised += HandlePointerEvent;
    }

    private void OnDisable()
    {
        _grabbable.WhenPointerEventRaised -= HandlePointerEvent;
    }

    private void HandlePointerEvent(PointerEvent evt)
    {
        if (evt.Type == PointerEventType.Select && _grabTracker != null)
        {
            _grabTracker.grabbed();
        }
    }
}