using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpellHoming : MonoBehaviour
{

    public float homingStrength = 0.2f;
    public float maxHomingAngle = 45f;
    public float detectionRange = 20f;

    private Transform targetEnemy;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        targetEnemy = FindClosestEnemeyInFront();
    }

    private void Update()
    {
        if (targetEnemy == null || rb == null) return;

        // Calculate where enemy is compared to spell
        Vector3 targetDirection = (targetEnemy.position - transform.position).normalized; 
        float angle = Vector3.Angle(transform.right, targetDirection);

        Debug.DrawLine(transform.position, targetEnemy.position, Color.red); // line to enemy
        Debug.DrawRay(transform.position, rb.velocity.normalized * 2f, Color.green); // spell direction

        if (angle < maxHomingAngle)
        {
            Vector3 newDirection = Vector3.Lerp(rb.velocity.normalized, targetDirection, homingStrength * Time.deltaTime).normalized; // smoothly turn object
            rb.velocity = newDirection * rb.velocity.magnitude; // keep speed 

            transform.right = rb.velocity.normalized; // change spell's forward direction to match velocity
        }
    }

    Transform FindClosestEnemeyInFront()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform bestTarget = null;
        float closestDistance = detectionRange;
        Vector3 forward = transform.right;

        foreach (GameObject enemy in enemies)
        {
            Vector3 enemyDirection = (enemy.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(forward, enemyDirection);
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            Debug.DrawLine(transform.position, enemy.transform.position, Color.yellow, 1.0f);

            if (angle < maxHomingAngle && distance < closestDistance) // check to see if in view of spell and closer than previous target
            {
                closestDistance = distance;
                bestTarget = enemy.transform;
                
                Debug.DrawLine(transform.position, enemy.transform.position, Color.blue, 1.0f); // line to selected enemy
            }
        }

        if (bestTarget != null) 
        {
            Debug.Log("Target found");
        }
        else 
        {
            Debug.Log("No Target Found in angle range");
        }

        return bestTarget;

        
    }

    // Visual representations don't ask me how this works
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (targetEnemy != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, targetEnemy.position);
        }

        // Use the spell's own forward direction
        Vector3 forward = transform.right;

        // Correctly rotate based on spell's own up axis
        Quaternion leftRot = Quaternion.AngleAxis(-maxHomingAngle, transform.up);
        Quaternion rightRot = Quaternion.AngleAxis(maxHomingAngle, transform.up);

        Vector3 leftDir = leftRot * forward;
        Vector3 rightDir = rightRot * forward;

        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, leftDir.normalized * detectionRange);
        Gizmos.DrawRay(transform.position, rightDir.normalized * detectionRange);
    }


}
