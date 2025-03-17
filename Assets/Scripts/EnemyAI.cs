using System;
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


    public EnemyState currentState = EnemyState.Wander;
    public int dRadius = 500;
    public Vector3 originalWanderPoint;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalWanderPoint = transform.position;//add a raycast to snap to the ground
    }


    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        //this section modifies the enemy state
        if (PlayerDetected()) currentState = EnemyState.Pursue;


        //this section determines what to do in any given state;

        switch (currentState)
        {
            case == EnemyState.Wander:
                System.Random random = new System.Random();
                int random_number_x = random.Next( ( (int) originalWanderPoint.x) - dRadius,  ((int) originalWanderPoint.x) + 1 + dRadius);
                int random_number_z = random.Next(((int)originalWanderPoint.z) - dRadius, ((int)originalWanderPoint.z) + 1 + dRadius);
                Vector3 generated_point = new Vector3(random_number_x, originalWanderPoint.y, random_number_z);

                if (Vector3.Distance(generated_point,transform.position) <= dRadius && agent.CalculatePath(generated_point,)
                //agent.CalculatePath will return true if there is a path, false if no path exists.
                //Physics.SphereCast() returns every object in that sphere.
        }




        //if (distanceToPlayer <= detectionRange)
        //{
        //    if (distanceToPlayer <= stoppingDistance)
        //    {
        //        agent.isStopped = true;
        //        agent.ResetPath();
        //    }
        //    else
        //    {
        //        agent.isStopped = false;
        //        agent.SetDestination(player.position);
        //    }

        //}
        //else
        //{
        //    agent.isStopped = true;
        //    agent.ResetPath();
        }

    Boolean PlayerDetected()
    {
        

        if (distanceToPlayer <= detectionRange)
        {
            return true;

        }
        return false;
}


}
