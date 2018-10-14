using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float maxHealth;
    public float speed;
    public float turnSpeed;
    public float jumpSpeed;
    public float power;
    public Vector3 respawn;
    public Quaternion respawnRotation;
    public Slider healthBar;
    public bool isPaused;


    private float currHealth;
    private bool isGrounded;
    private float floorDist;
    private Transform weaponTransform;
    private Rigidbody rigid;
    private float hitTime;
    private float hitTimer;
    private bool canHit;
    private bool isRunning;
    private Animator anim;
    private bool isDead;
    private float deathTimer;
    private float deathTime;

    void Start()
    {
        currHealth = maxHealth;
        rigid = GetComponent<Rigidbody>();
        hitTime = 0.4f;
        hitTimer = 0;
        canHit = true;
        isRunning = false;
        anim = GetComponentInChildren<Animator>();
        isDead = false;
        deathTime = 5;
        deathTimer = 0;
    }

    void Update()
    {
        if (!isPaused && !isDead)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isRunning)
            {
                speed *= 2;
                isRunning = true;
            }
            if (Input.GetKeyUp(KeyCode.CapsLock) && isRunning)
            {
                speed /= 2;
                isRunning = false;
            }

            if(Input.GetMouseButtonDown(0))
            {
                anim.SetBool("IsAttacking",true);
            }
            else
            {
                anim.SetBool("IsAttacking", false);
            }

            float horizontal = Input.GetAxis("Horizontal") * speed;
            float vertical = Input.GetAxis("Vertical") * speed;

            Movement(horizontal, vertical);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded)
                {
                    isGrounded = false;
                    StartCoroutine(Jump());
                }
            }
        }
        if (deathTimer >= deathTime)
        {
            transform.position = respawn;
            transform.rotation = respawnRotation;
            currHealth = maxHealth;
            //healthBar.value = maxHealth;

            isDead = false;
            deathTimer = 0;
        }
    }

    void LateUpdate()
    {
        hitTimer += Time.deltaTime;
        if (hitTimer > hitTime) canHit = true;

        if (isDead)
        {
            deathTimer += Time.deltaTime;
        }
    }

    IEnumerator Jump()
    {
        anim.SetBool("IsJumping", true);

        yield return new WaitForSeconds(0.5f);

        rigid.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
    }

    void Movement(float horizontal, float vertical)
    {
        if (horizontal != 0f || vertical != 0f)
        {
            Vector3 direction = Camera.main.transform.TransformDirection(new Vector3(horizontal, 0, vertical));
            rigid.velocity = new Vector3(direction.x, rigid.velocity.y, direction.z);

            transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

            if (isGrounded)
            {
                anim.SetBool("IsWalking", true);
                anim.SetFloat("Blend", horizontal);
            }

        }
        else
        {
            anim.SetBool("IsWalking", false);
        }
    }

    public void Paused(bool paused)
    {
        isPaused = paused;

        if (isPaused)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        rigid.angularVelocity = new Vector3(0, 0, 0);
        if (coll.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(coll.collider.gameObject.GetComponent<Enemy>().power);
        }
    }

    void TakeDamage(float damage)
    {
        if (!canHit) return;

        if (isDead || isPaused) return;

        currHealth -= damage;
        //healthBar.value = currHealth;

        hitTimer = 0;
        canHit = false;

        if (currHealth <= 0)
        {
            isDead = true;
        }
    }

    void OnCollisionStay(Collision coll)
    {
        rigid.angularVelocity = new Vector3(0, 0, 0);

        if (coll.gameObject.CompareTag("Wall"))
        {
            isGrounded = false;
        }
        else if(coll.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("IsJumping", false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy Weapon"))
        {
            GameObject enemy = other.gameObject.transform.parent.gameObject;
            if (enemy.CompareTag("Enemy")) TakeDamage(other.gameObject.transform.parent.GetComponent<Enemy>().power);

            float force = 2;
            float height = 2;
            Vector3 enemyDir = other.gameObject.transform.forward;
            Vector3 dir = new Vector3(enemyDir.x * force, height, enemyDir.z * force);
            rigid.AddForce(dir, ForceMode.Impulse);
        }
    }
}
