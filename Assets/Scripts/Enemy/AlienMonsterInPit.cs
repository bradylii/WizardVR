using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienMonsterInPit : MonoBehaviour
{
    public Transform player;
    public float speed = 1;
    public Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(player.position, transform.position, speed * Time.deltaTime);
        animator.SetFloat("Run", 1);
    }
}
