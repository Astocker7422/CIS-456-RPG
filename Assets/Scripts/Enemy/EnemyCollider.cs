using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------
     EnemyCollider - Controls enemy's aggro trigger collider
----------------------------------------------------------------------------------------*/
public class EnemyCollider : MonoBehaviour
{
    //The enemy or boss this is attached to
    public Enemy enemy;
    public Boss boss;

    //The sphere collider attached to this object
    public SphereCollider lineOfSight;

    //Indicates if the player is in range
    private bool playerInSight;

    void Start()
    {
        playerInSight = false;
    }

    void Update()
    {
        //If the player is in range
        if (playerInSight)
        {
            //Call player pursuit function in appropriate script
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
