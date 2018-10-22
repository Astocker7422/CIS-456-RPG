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
    public Weapon weapon;
    public Animator anim;


    private float currHealth;
    private bool isGrounded;
    private float floorDist;
    private Transform weaponTransform;
    private Rigidbody rigid;
    private float hitTime;
    private float hitTimer;
    private bool canHit;
    private bool isRunning;
    private bool isDead;
    private bool isMoving;

    void Start()
    {
        currHealth = maxHealth;
        rigid = GetComponent<Rigidbody>();
        hitTime = 1f;
        hitTimer = 0;
        canHit = true;
        isRunning = false;
        anim = GetComponentInChildren<Animator>();
        isDead = false;
        isMoving = false;
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

            if(Input.GetMouseButtonDown(0) && !isMoving)
            {
                anim.SetBool("IsAttacking",true);
            }
            else
            {
                anim.SetBool("IsAttacking", false);
            }

            float horizontal = Input.GetAxis("Horizontal") * speed;
            float vertical = Input.GetAxis("Vertical") * speed;

            if(!weapon.isSwinging) Movement(horizontal, vertical);

            if (Input.GetKeyDown(KeyCode.Space) && !weapon.isSwinging)
            {
                if (isGrounded)
                {
                    isGrounded = false;
                    StartCoroutine(Jump());
                }
            }
        }
    }

    void LateUpdate()
    {
        hitTimer += Time.deltaTime;
        if (hitTimer > hitTime) canHit = true;
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
        //if (coll.gameObject.CompareTag("Enemy"))
        //{
        //    TakeDamage(coll.collider.gameObject.GetComponent<Enemy>().power);
        //}
    }

    void TakeDamage(float damage)
    {
        if (!canHit) return;

        if (isDead || isPaused) return;

        currHealth -= damage;
        healthBar.value = currHealth;

        hitTimer = 0;
        canHit = false;

        if (currHealth <= 0)
        {
            StartCoroutine(Die());
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
            TakeDamage(other.gameObject.GetComponent<EnemyWeapon>().enemy.power);

            float force = 2;
            float height = 2;
            Vector3 enemyDir = other.gameObject.transform.forward;
            Vector3 dir = new Vector3(enemyDir.x * force, height, enemyDir.z * force);
            rigid.AddForce(dir, ForceMode.Impulse);
        }
    }

    IEnumerator Die()
    {
        isDead = true;
        anim.SetBool("IsDying", true);

        if (anim.GetBool("IsWalking"))
        {
            anim.SetBool("IsWalking", false);
        }

        if (anim.GetBool("IsAttacking"))
        {
            anim.SetBool("IsAttacking", false);
        }

        yield return new WaitForSeconds(5);

        transform.position = respawn;
        transform.rotation = respawnRotation;
        currHealth = maxHealth;
        healthBar.value = maxHealth;
        anim.SetBool("IsDying", false);

        isDead = false;
    }
}
