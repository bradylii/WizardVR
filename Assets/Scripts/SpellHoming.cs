using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpellHoming : MonoBehaviour
{
    public float speed = 10f;
    public float homingStrength = 0.2f;
    public float maxHomingAngle = 45f;
    public float detectionRange = 20f;

    private Transform targetEnemy;
    private Vector3 moveDirection;

    private void Start()
    {
        moveDirection = transform.forward;
        targetEnemy = FindClosestEnemeyInFront();
    }

    private void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;

        if (targetEnemy != null)
        {
       
            Vector3 targetDirection = (targetEnemy.position - transform.position).normalized;

            moveDirection = Vector3.Lerp(moveDirection, targetDirection, homingStrength * Time.deltaTime).normalized;

            transform.forward = moveDirection;

            Debug.DrawLine(transform.position, targetEnemy.position, Color.red);
        }

        Debug.DrawRay(transform.position, moveDirection * 2f, Color.green);
    }

    Transform FindClosestEnemeyInFront()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform bestTarget = null;
        float closestDistance = detectionRange;
        Vector3 playerForward = transform.forward;

        

        foreach (GameObject enemy in enemies)
        {
            Vector3 enemyDirection = (enemy.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(playerForward, enemyDirection);
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            Debug.DrawLine(transform.position, enemy.transform.position, Color.yellow, 1.0f);

            if (angle < maxHomingAngle && distance < closestDistance)
            {
                closestDistance = distance;
                bestTarget = enemy.transform;

                Debug.DrawLine(transform.position, enemy.transform.position, Color.blue, 1.0f);
            }
        }

        return bestTarget;
    }

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
        Vector3 forward = transform.forward;

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
