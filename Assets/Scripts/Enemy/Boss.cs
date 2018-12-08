using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*----------------------------------------------------------------------------------------
     Boss - Controls behavior of boss enemy in Outskirts level
----------------------------------------------------------------------------------------*/
public class Boss : MonoBehaviour
{
    //Stats
    public float maxHealth;
    public float speed;
    public float turnSpeed;
    public float power;

    //Amount of experience to player
    public int expValue;

    //The player
    public GameObject player;

    //Distance from player to stop and attack
    public float attackDistance;

    //The boss's weapon
    public EnemyWeapon weapon;

    //Boss's health bar
    public GameObject healthBar;

    //UI enemy count
    public EnemyCount enemyCount;

    //Indicates if boss in combat
    public bool inCombat;

    //Animator component
    private Animator animator;

    //Boss's current health
    private float currHealth;

    //Timer to handle invincibility after hit
    private float hitTime;
    private float hitTimer;

    //Timer to handle attack speed
    private float attackTime;
    private float attackTimer;

    //Indicates if boss can be hit and attack, respectively
    private bool canHit;
    private bool isAttacking;

    //Indicates if boss has died
    private bool isDead;

    void Start()
    {
        animator = GetComponent<Animator>();

        currHealth = maxHealth;
        healthBar.GetComponent<Slider>().maxValue = maxHealth;
        healthBar.GetComponent<Slider>().value = currHealth;

        hitTime = 0.5f;
        hitTimer = 0;

        attackTime = 2f;
        attackTimer = attackTime;

        canHit = true;

        isAttacking = false;

        isDead = false;

        inCombat = false;

        weapon.power = power;
    }

    void Update()
    {
        //If the boss is not dead,
        if (!isDead)
        {
            //If the boss is in combat,
            if (inCombat)
            {
                //If the boss is close enough to the player,
                if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
                {
                    //If the boss is not attacking and it is time to attack,
                    if (isAttacking == false && attackTimer >= attackTime)
                    {
                        //Attack animation and indicate attack
                        animator.SetBool("isAttacking", true);
                        isAttacking = true;
                        attackTimer = 0;
                    }
                    else
                    {
                        //If a second has passed since the attack,
                        if (attackTimer > 1)
                        {
                            //Stop animation and indicate attack over
                            animator.SetBool("isAttacking", false);
                            isAttacking = false;
                        }
                    }
                }
                else
                {
                    //Stop animation and indicate attack over
                    animator.SetBool("isAttacking", false);
                    isAttacking = false;
                }
            }

            //If boss attacking,
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("mutant_Punching"))
            {
                //Activate weapon's collider
                weapon.GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                weapon.GetComponent<BoxCollider>().enabled = false;
            }
        }
        //If the boss is out of combat,
        else
        {
            //Reduce weapon's power
            weapon.power = 0;
        }
    }

    void LateUpdate()
    {
        //Update invincibility timer
        hitTimer += Time.deltaTime;
        if (hitTimer > hitTime) canHit = true;

        //If the boss is in combat
        if (inCombat)
        {
            //Update attacking timer
            attackTimer += Time.deltaTime;
        }
    }

    //Applies damage to boss
    public void TakeDamage(float damage)
    {
        if (!canHit || isDead) return;

        if (!healthBar.activeInHierarchy) healthBar.SetActive(true);

        currHealth -= damage;
        healthBar.GetComponent<Slider>().value = currHealth;

        hitTimer = 0;
        canHit = false;

        if (currHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }
    
    //Controls movement toward player
    public void MoveToPlayer()
    {
        //If the boss is attacking or is close enough to the player
        if (isAttacking || Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
        {
            //Stop animation and stop moving
            GetComponentInChildren<Animator>().SetBool("isMoving", false);
            return;
        }

        if (isDead)
        {
            return;
        }

        GetComponentInChildren<Animator>().SetBool("isMoving", true);

        //Look at target
        Vector3 v3 = player.transform.position - transform.position;
        v3.y = 0.0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(v3), turnSpeed * Time.deltaTime);

        //Move towards target
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    //Handles dying animation and destorys object
    IEnumerator Die()
    {
        GetComponentInChildren<Animator>().SetBool("isDying", true);
        isDead = true;

        enemyCount.KillCount(transform.gameObject);

        player.GetComponent<Player>().IncrementExp(expValue);

        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }
}
