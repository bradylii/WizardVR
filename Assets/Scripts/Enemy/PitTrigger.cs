using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitTrigger : MonoBehaviour
{
    public AlienMonsterInPit monsterScript;

    [SerializeField] private bool playerInPit = false;

    public JoyStickMove joyStickMove;

    private void Start()
    {
        if (monsterScript == null)
        {
            monsterScript = GameObject.FindGameObjectWithTag("PitEnemy").GetComponent<AlienMonsterInPit>();
        }

        if (joyStickMove == null)
        {
            joyStickMove = GameObject.FindGameObjectWithTag("Player").GetComponent<JoyStickMove>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("[PitTrigger] Trigger collided with something");
        if (other.CompareTag("Player") && !playerInPit)
        {
            Debug.Log("[PitTrigger] Trigger collided with player");
            playerInPit = true;

            monsterScript.runToPlayer();

            if (joyStickMove != null)
                joyStickMove.lockMovement = true;
        }
    }
}
