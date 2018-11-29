using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    public Enemy enemy;
    public Boss boss;
    public SphereCollider lineOfSight;

    private bool playerInSight;

    void Start()
    {
        playerInSight = false;
    }

    void Update()
    {
        if (playerInSight)
        {
            if (enemy != null)
            {
                enemy.MoveToPlayer();
            }
            else
            {
                boss.MoveToPlayer();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInSight = true;
            if (enemy != null)
            {
                enemy.inCombat = true;
            }
            else
            {
                boss.inCombat = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInSight = false;
            if (enemy != null)
            {
                enemy.inCombat = true;
                enemy.GetComponent<Animator>().SetBool("isMoving", false);
            }
            else
            {
                boss.inCombat = true;
                boss.GetComponent<Animator>().SetBool("isMoving", false);
            }
        }
    }
}
