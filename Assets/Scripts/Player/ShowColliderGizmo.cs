using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowColliderGizmo : MonoBehaviour
{
    public bool showCollider;
    void OnDrawGizmos()
    {
        if (!showCollider) return;

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.color = Color.cyan; // or any color you want
            Gizmos.matrix = transform.localToWorldMatrix;

            if (col is BoxCollider box)
                Gizmos.DrawWireCube(box.center, box.size);
            else if (col is SphereCollider sphere)
                Gizmos.DrawWireSphere(sphere.center, sphere.radius);
            else if (col is CapsuleCollider capsule)
                DrawCapsule(capsule);
        }
    }

    void DrawCapsule(CapsuleCollider capsule)
    {
        Vector3 center = capsule.center;
        float radius = capsule.radius;
        float height = capsule.height;
        int direction = capsule.direction;

        float cylinderHeight = Mathf.Max(0, height - 2 * radius);

        Vector3 up = Vector3.up;
        if (direction == 0) up = Vector3.right;
        else if (direction == 2) up = Vector3.forward;

        Vector3 top = center + up * (cylinderHeight / 2);
        Vector3 bottom = center - up * (cylinderHeight / 2);

        // Draw cylinder part
        Gizmos.DrawLine(top + Vector3.forward * radius, bottom + Vector3.forward * radius);
        Gizmos.DrawLine(top - Vector3.forward * radius, bottom - Vector3.forward * radius);
        Gizmos.DrawLine(top + Vector3.right * radius, bottom + Vector3.right * radius);
        Gizmos.DrawLine(top - Vector3.right * radius, bottom - Vector3.right * radius);

        // Draw sphere ends
        Gizmos.DrawWireSphere(top, radius);
        Gizmos.DrawWireSphere(bottom, radius);
    }
}
