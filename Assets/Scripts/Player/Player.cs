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
    public Slider expBar;
    public GameObject statCanvas;


    private float currHealth;
    private float defaultSpeed;
    private bool isGrounded;
    private float floorDist;
    private Transform weaponTransform;
    private Rigidbody rigid;
    private float hitTime;
    private float hitTimer;
    private bool canHit;
    private float jumpTimer;
    private float jumpTime;
    private bool isDead;
    private bool isMoving;
    private int level;
    private int exp;
    private int expTilLvl;
    private TMPro.TextMeshProUGUI expText;

    void Start()
    {
        maxHealth = PlayerStats.Instance().HP;
        speed = PlayerStats.Instance().Speed;
        power = PlayerStats.Instance().Power;
        jumpSpeed = PlayerStats.Instance().Jump * 10;

        currHealth = maxHealth;
        defaultSpeed = speed;
        rigid = GetComponent<Rigidbody>();
        hitTime = 1f;
        hitTimer = 0;
        canHit = true;
        anim = GetComponentInChildren<Animator>();
        jumpTimer = 0.6f;
        jumpTime = jumpTimer;
        isDead = false;
        isMoving = false;

        expText = expBar.gameObject.transform.Find("Value Text").GetComponent<TMPro.TextMeshProUGUI>();

        level = PlayerStats.Instance().Level;

        exp = PlayerStats.Instance().CurrExp;
        expTilLvl = PlayerStats.Instance().MaxExp;

        expBar.maxValue = expTilLvl;
        expBar.value = exp;

        expText.text = exp + " / " + expTilLvl;
    }

    void Update()
    {
        if (!isPaused && !isDead)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = defaultSpeed * 2;
            }
            else
            {
                speed = defaultSpeed;
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
                if (isGrounded && jumpTimer >= jumpTime)
                {
                    isGrounded = false;
                    StartCoroutine(Jump());
                    jumpTimer = 0;
                }
            }
        }
    }

    void LateUpdate()
    {
        hitTimer += Time.deltaTime;
        if (hitTimer > hitTime) canHit = true;

        jumpTimer += Time.deltaTime;
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

    void OnCollisionExit(Collision coll)
    {
        rigid.angularVelocity = new Vector3(0, 0, 0);

        if (coll.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
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

    public int GetLevel()
    {
        return level;
    }

    public void IncrementExp(int points)
    {
        exp += points;
        PlayerStats.Instance().CurrExp = exp;

        if(exp >= expTilLvl)
        {
            exp = 0;
            PlayerStats.Instance().CurrExp = exp;

            expTilLvl *= 2;
            PlayerStats.Instance().MaxExp = expTilLvl;

            expBar.maxValue = expTilLvl;

            level++;
            PlayerStats.Instance().Level = level;

            ActivateStatCanvas();
        }

        expBar.value = exp;

        expText.text = exp + " / " + expTilLvl;
    }

    private void ActivateStatCanvas()
    {
        Time.timeScale = 0;
        isPaused = true;

        statCanvas.SetActive(true);

        LevelUpCanvas statScript = statCanvas.GetComponent<LevelUpCanvas>();

        statScript.SetHP((int) maxHealth);
        statScript.SetSpeed((int) speed);
        statScript.SetPower((int) power);
        statScript.SetJump((int) (jumpSpeed / 10));
    }

    public void DeactivateStatCanvas()
    {
        LevelUpCanvas statScript = statCanvas.GetComponent<LevelUpCanvas>();

        PlayerStats.Instance().HP = statScript.GetHP();
        PlayerStats.Instance().Speed = statScript.GetSpeed();
        PlayerStats.Instance().Power = statScript.GetPower();
        PlayerStats.Instance().Jump = statScript.GetJump();


        maxHealth = PlayerStats.Instance().HP;
        healthBar.maxValue = maxHealth;
        healthBar.value = currHealth;

        speed = PlayerStats.Instance().Speed;
        power = PlayerStats.Instance().Power;
        jumpSpeed = PlayerStats.Instance().Jump * 10;

        statCanvas.SetActive(false);

        Time.timeScale = 1;
        isPaused = false;
    }
}
