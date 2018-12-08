using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*----------------------------------------------------------------------------------------
     Player - Handles all input by player and behavior of player character
----------------------------------------------------------------------------------------*/
public class Player : MonoBehaviour
{
    //Player stats
    public float maxHealth;
    public float speed;
    public float turnSpeed;
    public float jumpSpeed;
    public float power;

    //Transform to respawn at
    public Vector3 respawn;
    public Quaternion respawnRotation;

    //Player health bar
    public Slider healthBar;

    //Indicates if player can move
    public bool isPaused;

    //Canvas of pause menu
    public GameObject pauseCanvas;

    //Camera following player
    public GameObject mainCamera;

    //Player's weapon
    public Weapon weapon;

    //Animator component
    public Animator anim;

    //Experience bar
    public Slider expBar;

    //Level up canvas
    public GameObject statCanvas;

    //Current health
    private float currHealth;

    //Non-sprinting speed
    private float defaultSpeed;

    //Indicates if player touching the ground
    private bool isGrounded;

    //Distance from player to ground
    private float floorDist;

    //Weapon's transform
    private Transform weaponTransform;

    //Player's rigidbody
    private Rigidbody rigid;

    //Timer and bool handling invincibility after getting hit
    private float hitTime;
    private float hitTimer;
    private bool canHit;

    //Timer to fix double jumping
    private float jumpTimer;
    private float jumpTime;

    //Indicates if player has died
    private bool isDead;

    //Indicates if player if moving
    private bool isMoving;

    //Variables handling level and experience
    private int level;
    private int exp;
    private int expTilLvl;
    private TMPro.TextMeshProUGUI expText;

    void Start()
    {
        //Initialize stats
        maxHealth = PlayerStats.Instance().HP;
        speed = PlayerStats.Instance().Speed;
        power = PlayerStats.Instance().Power;
        jumpSpeed = PlayerStats.Instance().Jump * 10;

        currHealth = maxHealth;
        defaultSpeed = speed;

        rigid = GetComponent<Rigidbody>();

        hitTime = 2f;
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

        Paused(true);
    }

    void Update()
    {
        //If the player can move and has not died
        if (!isPaused && !isDead)
        {
            //Check for sprint
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = defaultSpeed * 2;
            }
            else
            {
                speed = defaultSpeed;
            }

            //If player is not moving and attack button pressed,
            if(Input.GetMouseButtonDown(0) && !isMoving)
            {
                //Attack
                anim.SetBool("IsAttacking",true);
            }
            else
            {
                anim.SetBool("IsAttacking", false);
            }

            //Get inputs
            float horizontal = Input.GetAxis("Horizontal") * speed;
            float vertical = Input.GetAxis("Vertical") * speed;

            //Apply movement if not attacking
            if(!weapon.isSwinging) Movement(horizontal, vertical);

            //If the player is not attacking and jump button pressed,
            if (Input.GetKeyDown(KeyCode.Space) && !weapon.isSwinging)
            {
                //If the player is on the ground and it has been long enough since last jump,
                if (isGrounded && jumpTimer >= jumpTime)
                {
                    //Start jump
                    isGrounded = false;
                    StartCoroutine(Jump());
                    jumpTimer = 0;
                }
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                ActivatePauseCanvas();
            }
        }
    }

    void LateUpdate()
    {
        //Update invincibility timer and jump timer
        hitTimer += Time.deltaTime;
        if (hitTimer > hitTime) canHit = true;

        jumpTimer += Time.deltaTime;
    }

    //Handles jump animation and physics
    IEnumerator Jump()
    {
        anim.SetBool("IsJumping", true);

        yield return new WaitForSeconds(0.5f);
        
        rigid.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
    }

    //Applies movement to player using rigidbody
    void Movement(float horizontal, float vertical)
    {
        if (horizontal != 0f || vertical != 0f)
        {
            anim.SetBool("IsAttacking", false);

            Vector3 direction = Camera.main.transform.TransformDirection(new Vector3(horizontal, 0, vertical));
            rigid.velocity = new Vector3(direction.x, rigid.velocity.y, direction.z);

            transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

            isMoving = true;

            if (isGrounded)
            {
                anim.SetBool("IsWalking", true);
                anim.SetFloat("Blend", horizontal);
            }
        }
        else
        {
            isMoving = false;

            anim.SetBool("IsWalking", false);
        }
    }

    //Set if the player can move
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
    }

    //Applies damage to player
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
            TakeDamage(other.gameObject.GetComponent<EnemyWeapon>().power);

            float force = 2;
            float height = 2;
            Vector3 enemyDir = other.gameObject.transform.forward;
            Vector3 dir = new Vector3(enemyDir.x * force, height, enemyDir.z * force);
            rigid.AddForce(dir, ForceMode.Impulse);
        }
    }

    //Handles animation and respawn
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

    //Handles healing from health pack
    public void Heal()
    {
        currHealth += maxHealth * 0.1f;

        if(currHealth > maxHealth)
        {
            currHealth = maxHealth;
        }

        healthBar.value = currHealth;
    }

    //Returns true if health is full
    public bool isHealthFull()
    {
        return currHealth == maxHealth;
    }

    //Returns player's level
    public int GetLevel()
    {
        return level;
    }

    //Adds experience to player
    public void IncrementExp(int points)
    {
        exp += points;
        PlayerStats.Instance().CurrExp = exp;

        //If the player reached a new level,
        if(exp >= expTilLvl)
        {
            //Reset experience
            exp = 0;
            PlayerStats.Instance().CurrExp = exp;

            //Increase experience needed to level up
            expTilLvl *= 2;
            PlayerStats.Instance().MaxExp = expTilLvl;

            expBar.maxValue = expTilLvl;

            level++;
            PlayerStats.Instance().Level = level;
            
            //Activate the level up canvas
            ActivateStatCanvas();
        }

        expBar.value = exp;

        expText.text = exp + " / " + expTilLvl;
    }

    //Activates pause menu UI and pauses the game
    private void ActivatePauseCanvas()
    {
        Time.timeScale = 0;
        Paused(true);

        pauseCanvas.SetActive(true);
    }

    //Activates the level up canvas and passes it the player's current stats
    private void ActivateStatCanvas()
    {
        Time.timeScale = 0;
        Paused(true);

        statCanvas.SetActive(true);

        LevelUpCanvas statScript = statCanvas.GetComponent<LevelUpCanvas>();

        statScript.SetHP(PlayerStats.Instance().HP);
        statScript.SetSpeed(PlayerStats.Instance().Speed);
        statScript.SetPower(PlayerStats.Instance().Power);
        statScript.SetJump(PlayerStats.Instance().Jump);
    }

    //Deactivates the level up canvas and applies the chosen upgrades
    public void DeactivateStatCanvas()
    {
        LevelUpCanvas statScript = statCanvas.GetComponent<LevelUpCanvas>();

        PlayerStats.Instance().HP = statScript.GetHP();
        PlayerStats.Instance().Speed = statScript.GetSpeed();
        PlayerStats.Instance().Power = statScript.GetPower();
        PlayerStats.Instance().Jump = statScript.GetJump();

        //If the player's health is full when upgrading health,
        if(isHealthFull())
        {
            //Also increase current health
            maxHealth = PlayerStats.Instance().HP;
            currHealth = maxHealth;
        }
        else
        {
            maxHealth = PlayerStats.Instance().HP;
        }
        healthBar.maxValue = maxHealth;
        healthBar.value = currHealth;

        speed = PlayerStats.Instance().Speed;
        power = PlayerStats.Instance().Power;
        jumpSpeed = PlayerStats.Instance().Jump * 10;

        statCanvas.SetActive(false);

        Time.timeScale = 1;
        Paused(false);
    }

    //Pauses player and turns off main camera
    public void AllowOtherCamera(bool allow)
    {
        mainCamera.SetActive(!allow);
        Paused(allow);
    }
}
