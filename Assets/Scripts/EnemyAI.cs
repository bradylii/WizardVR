using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Wander = 0,
    Pursue = 1,
    Search = 2
}


public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float stoppingDistance = 2f;
    private NavMeshAgent agent;
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
            if (distanceToPlayer <= stoppingDistance)
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


}
