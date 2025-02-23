using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float stoppingDistance = 2f;
    private NavMeshAgent agent;
    public LayerMask obstacleMask;
    [SerializeField] private float distanceToPlayer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);


        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer <= stoppingDistance && HaslineOfSight())
            {
                agent.isStopped = true;
                agent.ResetPath();
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }

        }
        else
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
    }

    private bool HaslineOfSight()
    {
        RaycastHit hit;
        Vector3 startRayPosition = transform.position + Vector3.up * 1.5f;
        Vector3 directionToPlayer = (player.position - startRayPosition).normalized;

        Debug.DrawRay(startRayPosition, directionToPlayer * detectionRange, Color.red, 0.1f);


        if (Physics.Raycast(startRayPosition, directionToPlayer, out hit, detectionRange, obstacleMask))
        {
            Debug.Log("Ray hit: " + hit.transform.name);
            return hit.transform == player;
        }
        return false;
    }

}
