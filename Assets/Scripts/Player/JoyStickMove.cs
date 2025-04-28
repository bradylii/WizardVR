using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickMove : MonoBehaviour
{
    public GameObject head;
    public GameObject controller;
    public float speed = 2.0f;

    private Rigidbody rb;

    public bool lockMovement = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        
    }

    void Update()
    {
        // Get joystick input
        Vector2 joystickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);

        if (joystickInput.magnitude > 0.1f && !lockMovement) // Deadzone threshold
        {
            // Get movement direction based on controller's forward direction
            Vector3 moveDirection = controller.transform.forward * joystickInput.y + controller.transform.right * joystickInput.x;
            moveDirection.y = 0; // Prevent vertical movement


            rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
            // Apply movement
            //head.transform.position += moveDirection * speed * Time.deltaTime;
        }
    }
}

