using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlienMonsterInPit : MonoBehaviour
{
    #region References
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;

    [SerializeField] private NavMeshAgent agent;
    #endregion

    #region Movement and Combat
    [Header("Movement and Combat")]
    [SerializeField] private float offset = 2f;
    [SerializeField] private float velocity;
    [SerializeField] private bool chasingPlayer = false;
    #endregion

    private void Start()
    {
        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (chasingPlayer)
        {
            // Update animation based on speed
            velocity = agent.velocity.magnitude;
            animator.SetFloat("speed", velocity);

            // if close enough to target, attack
            if (!agent.pathPending)
            {
                float distanceToTarget = Vector3.Distance(agent.destination, agent.transform.position);

                if (distanceToTarget <= 0.1f)
                    animator.SetTrigger("Punch");
            }
        }
    }


    public void runToPlayer()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Destination is offset from player 
        if (player != null)
        {
            Vector3 targetPosition = new Vector3(player.position.x, player.position.y, player.position.z + offset);
            agent.SetDestination(targetPosition);

            chasingPlayer = true;
        }
    }
}
