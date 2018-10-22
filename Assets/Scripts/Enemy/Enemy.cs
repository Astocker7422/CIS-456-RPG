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
    public EnemyWeapon weapon;
    public GameObject healthBar;

    private float currHealth;
    private float hitTime;
    private float hitTimer;
    private float attackTime;
    private float attackTimer;
    private bool canHit;
    private bool isAttacking;
    private bool isDead;

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

        weapon.power = 0;
    }

    void Update()
    {
        if (!isDead)
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
    }

    void LateUpdate()
    {
        hitTimer += Time.deltaTime;
        if (hitTimer > hitTime) canHit = true;

        attackTimer += Time.deltaTime;
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

    public void moveToPlayer()
    {
        if (isAttacking || Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
        {
            GetComponentInChildren<Animator>().SetBool("isMoving", false);
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

    IEnumerator Die()
    {
        GetComponentInChildren<Animator>().SetBool("isDying", true);
        isDead = true;

        yield return new WaitForSeconds(5);

        Destroy(this.gameObject);
    }
}
