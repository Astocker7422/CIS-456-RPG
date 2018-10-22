using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls behavior of the player's weapon object
//Attached to the player's weapon object
public class Weapon : MonoBehaviour
{
    //The player holding the weapon
    public Player player;

    //Indication that the weapon is being used
    public bool isSwinging;

    void Start ()
    {
        isSwinging = false;
	}

    void Update()
    {
        if(player.anim.GetCurrentAnimatorStateInfo(0).IsName("Punching"))
        {
            isSwinging = true;
        }
        else
        {
            isSwinging = false;
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        Debug.Log("fist trigger entered enemy");
        //If the weapon is being used,
        if (isSwinging)
        {
            Debug.Log("weapon is swinging");
            //If an enemy enters the weapon's trigger,
            if (coll.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Enemy Hit");
                //Apply damage to the enemy
                coll.gameObject.GetComponent<Enemy>().TakeDamage(player.power);
            }
        }
    }
}
