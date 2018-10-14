using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float maxHealth;
    public float power;
    public Vector3 respawn;
    //public Text countText;
    //public float collectGoal;
    //public GameObject WinCanvas;
    //public Slider healthBar;
    public bool isPaused;

    private float currHealth;
    //private bool isGrounded;
    //private int count;
    //private AudioSource audioSource;
    //private float floorDist;
    //private Transform weaponTransform;
    //private Rigidbody rigid;
    private float hitTime;
    private float hitTimer;
    private bool canHit;
    //private bool isRunning;

    void Start()
    {
        currHealth = maxHealth;
        hitTime = 0.5f;
        hitTimer = 0;
        canHit = true;

        Debug.Log(currHealth);
    }

    void Update()
    {
        
    }

    void LateUpdate()
    {
        hitTimer += Time.deltaTime;
        if (hitTimer > hitTime) canHit = true;
    }
    
    //Control cursor visibility on pause
    //public void Paused(bool paused)
    //{
    //    isPaused = paused;

    //    if(isPaused)
    //    {
    //        Cursor.visible = true;
    //    }
    //    else
    //    {
    //        Cursor.visible = false;
    //    }
    //}

    //Take damage on touch enemy
    //void OnCollisionEnter(Collision coll)
    //{
    //    rigid.angularVelocity = new Vector3(0, 0, 0);
    //    if (coll.gameObject.CompareTag("Enemy"))
    //    {
    //        TakeDamage(coll.collider.gameObject.GetComponent<Enemy>().power);
    //    }
    //}

    void TakeDamage(float damage)
    {
        if (!canHit) return;

        currHealth -= damage;
        //healthBar.value = currHealth;

        Debug.Log(currHealth);

        hitTimer = 0;
        canHit = false;

        if (currHealth <= 0)
        {
            transform.position = respawn;
            currHealth = maxHealth;
            //healthBar.value = maxHealth;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy Weapon"))
        {
            TakeDamage(other.gameObject.GetComponent<EnemyWeapon>().power);

            //float force = 2;
            //float height = 2;
            //Vector3 enemyDir = other.gameObject.transform.forward;
            //Vector3 dir = new Vector3(enemyDir.x * force, height, enemyDir.z * force);
            //rigid.AddForce(dir, ForceMode.Impulse);
        }
    }
}
