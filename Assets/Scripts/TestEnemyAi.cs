using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class TestEnemyAi : MonoBehaviour
{
    private NavMeshAgent agent;
    private FieldOfView fieldOfView;

    public float stoppingDistance = 2f;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        fieldOfView = GetComponent<FieldOfView>();
        Debug.Log("Isn't stopped");
    }


    void Update()
    {
        if (fieldOfView.canSeePlayer)
        {
            float distanceToplayer = Vector3.Distance(transform.position, fieldOfView.playerRef.transform.position);

            if (distanceToplayer <= stoppingDistance)
            {
                agent.isStopped = true;
                Debug.Log("Is stopped");
                agent.ResetPath();
            }
            else
            {
                agent.isStopped = false;
                Debug.Log("Isnt stopped");
                agent.SetDestination(fieldOfView.playerRef.transform.position);
            }
        }
        else
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
    }


}
