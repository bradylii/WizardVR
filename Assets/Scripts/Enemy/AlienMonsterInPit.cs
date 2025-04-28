using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienMonsterInPit : MonoBehaviour
{
    public Transform player;
    public Animator animator;

    private UnityEngine.AI.NavMeshAgent agent;

    public float offset = 2f;

    [SerializeField] private float velocity;

    [SerializeField] private bool chasingPlayer = false;


    private void Start()
    {
        animator = GetComponent<Animator>();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void Update()
    {
        if (chasingPlayer)
        {
            velocity = agent.velocity.magnitude;
            animator.SetFloat("speed", velocity);

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

        if (player != null)
        {
            Vector3 targetPosition = new Vector3(player.position.x, player.position.y, player.position.z + offset);
            agent.SetDestination(targetPosition);

            chasingPlayer = true;
        }
    }
}
