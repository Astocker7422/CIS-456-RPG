using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float maxHealth;
    public float speed;
    public float turnSpeed;
    public float power;

    public GameObject player;

    public float attackDistance;

    public float wanderDistance;

    public EnemyWeapon weapon;

    public GameObject healthBar;

    public EnemyCount enemyCount;

    public bool inCombat;

    private float currHealth;

    private float hitTime;
    private float hitTimer;

    private float attackTime;
    private float attackTimer;

    private bool canHit;
    private bool isAttacking;

    private bool isDead;

    private float destTimer;
    private float destTime;

    private float minX;
    private float maxX;
    private float minZ;
    private float maxZ;

    private Vector3 wanderDestination;

    void Start()
    {
        currHealth = maxHealth;
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
        wanderDestination = transform.position;

        minX = transform.position.x - wanderDistance;
        maxX = transform.position.x + wanderDistance;
        minZ = transform.position.z - wanderDistance;
        maxZ = transform.position.z + wanderDistance;

        enemyCount.IncrementCount();

        weapon.power = 0;
    }

    void Update()
    {
        if (!isDead)
        {
            if (inCombat)
            {
                if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
                {
                    if (isAttacking == false && attackTimer >= attackTime)
                    {
                        GetComponentInChildren<Animator>().SetBool("isAttacking", true);
                        weapon.power = power;
                        isAttacking = true;
                        attackTimer = 0;
                    }
                    else
                    {
                        if (attackTimer > 1)
                        {
                            GetComponentInChildren<Animator>().SetBool("isAttacking", false);
                            weapon.power = 0;
                            isAttacking = false;
                        }
                    }
                }
                else
                {
                    GetComponentInChildren<Animator>().SetBool("isAttacking", false);
                    weapon.power = 0;
                    isAttacking = false;
                }
            }
            else
            {
                Patrol();
            }
        }
    }

    void LateUpdate()
    {
        hitTimer += Time.deltaTime;
        if (hitTimer > hitTime) canHit = true;

        if (inCombat)
        {
            attackTimer += Time.deltaTime;
        }
        else
        {
            destTimer += Time.deltaTime;
        }
    }

    public void TakeDamage(float damage)
    {
        if (!canHit) return;

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

    IEnumerator Die()
    {
        GetComponentInChildren<Animator>().SetBool("isDying", true);
        isDead = true;

        enemyCount.DecrementCount();

        yield return new WaitForSeconds(5);

        Destroy(this.gameObject);
    }
}
