using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*----------------------------------------------------------------------------------------
     Enemy - Controls behavior of basic enemy in all levels
----------------------------------------------------------------------------------------*/
public class Enemy : MonoBehaviour
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

    //Distance to wander while idle patrolling
    public float wanderDistance;

    //Enemy's weapon
    public EnemyWeapon weapon;

    //Enemy's health bar
    public GameObject healthBar;

    //UI enemy count
    public EnemyCount enemyCount;

    //Indicates if enemy in combat
    public bool inCombat;

    //Enemy's current health
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

    //Timer to control when to patrol to a new destination
    private float destTimer;
    private float destTime;

    //Timer controlling respawn
    private float respawnTimer;
    private float respawnTime;

    //Defines circle to patrol in
    private float minX;
    private float maxX;
    private float minZ;
    private float maxZ;

    //Destination during idle patrolling
    private Vector3 wanderDestination;

    void Start()
    {
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

        destTime = 3;
        destTimer = 0;

        respawnTime = 60;
        respawnTimer = 0;

        wanderDestination = transform.position;

        minX = transform.position.x - wanderDistance;
        maxX = transform.position.x + wanderDistance;
        minZ = transform.position.z - wanderDistance;
        maxZ = transform.position.z + wanderDistance;

        if(enemyCount != null) enemyCount.AddEnemy(transform.gameObject);

        weapon.power = 0;
    }

    void Update()
    {
        //If the enemy is not dead,
        if (!isDead)
        {
            //If the enemy is in combat,
            if (inCombat)
            {
                //If the enemy is close enough to the player,
                if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
                {
                    //If the enemy is not attacking and it is time to attack,
                    if (isAttacking == false && attackTimer >= attackTime)
                    {
                        //Attack animation and indicate attack
                        GetComponentInChildren<Animator>().SetBool("isAttacking", true);
                        weapon.power = power;
                        isAttacking = true;
                        attackTimer = 0;
                    }
                    else
                    {
                        //If a second has passed since the attack,
                        if (attackTimer > 1)
                        {
                            //Stop animation and indicate attack over
                            GetComponentInChildren<Animator>().SetBool("isAttacking", false);
                            weapon.power = 0;
                            isAttacking = false;
                        }
                    }
                }
                else
                {
                    //Stop animation and indicate attack over
                    GetComponentInChildren<Animator>().SetBool("isAttacking", false);
                    weapon.power = 0;
                    isAttacking = false;
                }
            }
            else
            {
                //Patrol if not in combat
                Patrol();
            }
        }
        else
        {
            //Respawn if time passed
            if(respawnTimer >= respawnTime)
            {
                isDead = false;
                Activate();
                respawnTimer = 0;
                GetComponentInChildren<Animator>().SetBool("isDying", false);

                currHealth = maxHealth;
                healthBar.GetComponent<Slider>().value = currHealth;
            }
        }
    }

    void LateUpdate()
    {
        //Update invincibility timer
        hitTimer += Time.deltaTime;
        if (hitTimer > hitTime) canHit = true;

        //If the enemy is in combat
        if (inCombat)
        {
            //Update attacking timer
            attackTimer += Time.deltaTime;
        }
        else
        {
            //Update patrol timer
            destTimer += Time.deltaTime;
        }

        //If the enemy is dead,
        if(isDead)
        {
            //Update respawn timer
            respawnTimer += Time.deltaTime;
        }
    }

    //Applies damage to enemy
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

    //Controls idle patrol behavior
    private void Patrol()
    {
        if (destTimer >= destTime)
        {
            destTimer = 0;

            float xDest = Random.Range(minX, maxX);
            float zDest = Random.Range(minZ, maxZ);

            wanderDestination = new Vector3(xDest, transform.position.y, zDest);

            //Look at target
            Vector3 v3 = wanderDestination - transform.position;
            v3.y = 0.0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(v3), turnSpeed * Time.deltaTime);
        }
        
        if (Vector3.Distance(transform.position, wanderDestination) > 0.5)
        {
            GetComponentInChildren<Animator>().SetBool("isWalking", true);

            //Move towards target
            transform.position += transform.forward * (speed / 2) * Time.deltaTime;
        }
        else
        {
            GetComponentInChildren<Animator>().SetBool("isWalking", false);
        }
    }

    //Controls pursuit of player behavior
    public void MoveToPlayer()
    {
        if (isAttacking || Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
        {
            GetComponentInChildren<Animator>().SetBool("isMoving", false);
            return;
        }

        if(isDead)
        {
            return;
        }

        GetComponentInChildren<Animator>().SetBool("isWalking", false);
        GetComponentInChildren<Animator>().SetBool("isMoving", true);

        //Look at target
        Vector3 v3 = player.transform.position - transform.position;
        v3.y = 0.0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(v3), turnSpeed * Time.deltaTime);

        //Move towards target
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    //Increment all stats
    public void ScaleStats()
    {
        bool healthFull = false;
        if(currHealth == maxHealth)
        {
            healthFull = true;
        }

        maxHealth++;

        if(healthFull)
        {
            currHealth = maxHealth;
        }

        healthBar.GetComponent<Slider>().maxValue = maxHealth;
        healthBar.GetComponent<Slider>().value = currHealth;
        
        speed++;

        power++;

        expValue += 50;
    }

    //Handles behavior on death
    IEnumerator Die()
    {
        GetComponentInChildren<Animator>().SetBool("isDying", true);
        isDead = true;

        if (enemyCount != null) enemyCount.KillCount(transform.gameObject);

        player.GetComponent<Player>().IncrementExp(expValue);

        yield return new WaitForSeconds(5);

        Deactivate();
    }

    //Deactivates all components of enemy without deactivating this script
    private void Deactivate()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        GetComponent<CapsuleCollider>().enabled = false;
    }

    //Activates all components of enemy
    private void Activate()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        GetComponent<CapsuleCollider>().enabled = true;

        healthBar.SetActive(false);
    }
}
