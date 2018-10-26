using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    public Enemy enemy;
    public SphereCollider lineOfSight;

    private bool playerInSight;

    void Start()
    {
        playerInSight = false;
    }

    void Update()
    {
        if (playerInSight) enemy.MoveToPlayer();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInSight = true;
            enemy.inCombat = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInSight = false;
            enemy.inCombat = false;
            enemy.GetComponent<Animator>().SetBool("isMoving", false);
        }
    }
}
