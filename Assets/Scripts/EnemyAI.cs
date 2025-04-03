using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Wander_init = 0,
    Wander_to_Path = 1,
    Pursue = 2,
    Search_init = 3,
    Search_to_Path = 4
}


public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 2f;
    public float stoppingDistance = 2f;
    
    private NavMeshAgent agent;
    [SerializeField] private float distanceToPlayer;


    public EnemyState currentState = EnemyState.Wander_init;
    int dRadius = 4;
    public Vector3 originalWanderPoint;
    public Vector3 newWanderPoint;
    public Vector3 destinationPoint;
    public float margin_of_difference = 1;
    NavMeshPath path = new NavMeshPath();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalWanderPoint = transform.position;//add a raycast to snap to the ground
        originalWanderPoint.y = (float) 0.58;
    }


    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        //this section modifies the enemy state
        if (PlayerDetected())
        {
            currentState = EnemyState.Pursue;
        }else if (currentState == EnemyState.Pursue)
        {
            currentState = EnemyState.Search_init;//this is supposed to go EnemyState.Search_init but for now we will go to Wander_init
        }


        //this section determines what to do in any given state;

        switch (currentState)
        {
            case EnemyState.Wander_init:
                
                //randomly select a point in a radius around the wanderpoint
                System.Random random = new System.Random();
                int random_number_x = random.Next( ( (int) originalWanderPoint.x) - dRadius,  ((int) originalWanderPoint.x) + 1 + dRadius);
                int random_number_z = random.Next(((int)originalWanderPoint.z) - dRadius, ((int)originalWanderPoint.z) + 1 + dRadius);
                Vector3 generated_point = new Vector3(random_number_x, originalWanderPoint.y, random_number_z);

                path = new NavMeshPath();
                if (Vector3.Distance(generated_point,originalWanderPoint) <= dRadius && agent.CalculatePath(generated_point, path))
                {
                    destinationPoint = generated_point;
                    //currentState = EnemyState.Wander_to_Path;
                    //transform.position = generated_point;
                    currentState = EnemyState.Wander_to_Path;
                    agent.SetPath(path);
                }
                break;
            case EnemyState.Wander_to_Path:
                if (agent.remainingDistance == 0 || agent.velocity == new Vector3(0, 0, 0))
                {
                    currentState = EnemyState.Wander_init;
                }
                break;
            case EnemyState.Pursue:
                if (Vector3.Distance(destinationPoint, player.position) > margin_of_difference)
                {
                    path = new NavMeshPath();
                    agent.CalculatePath(player.position,path);
                    agent.SetPath(path);
                }
                break;
            case EnemyState.Search_init:
                newWanderPoint = path.corners[path.corners.Length - 1];//Create a new temporary wanderpoint to circle around.
                System.Random random1 = new System.Random();
                int random_number_x1 = random1.Next(((int)newWanderPoint.x) - dRadius, ((int)newWanderPoint.x) + 1 + dRadius);
                int random_number_z1 = random1.Next(((int)newWanderPoint.z) - dRadius, ((int)newWanderPoint.z) + 1 + dRadius);
                Vector3 generated_point1 = new Vector3(random_number_x1, newWanderPoint.y, random_number_z1);

                path = new NavMeshPath();


                if (Vector3.Distance(generated_point1, newWanderPoint) <= dRadius && agent.CalculatePath(generated_point1, path))
                {
                    destinationPoint = generated_point1;
                    //currentState = EnemyState.Wander_to_Path;
                    //transform.position = generated_point;
                    currentState = EnemyState.Search_to_Path;
                    agent.SetPath(path);
                }
                break;
            case EnemyState.Search_to_Path:
                if (agent.remainingDistance == 0 || agent.velocity == new Vector3(0, 0, 0))
                {
                    currentState = EnemyState.Search_init;
                }
                break;
            default:
                
                break;
                
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
