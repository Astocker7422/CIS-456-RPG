using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*----------------------------------------------------------------------------------------
     Bomb - Controls behavior of bombs in City level
----------------------------------------------------------------------------------------*/
public class Bomb : MonoBehaviour
{
    //Bomb's maximum health
    public float maxHealth;

    //Experience given to player
    public int expValue;

    //The player
    public GameObject player;

    //The bomb's health bar
    public GameObject healthBar;

    //UI counting bombs in level
    public BombCount bombCount;

    //The body object of the bomb
    private GameObject body;

    //Bomb's exploding effect
    private GameObject explosion;

    //Current health
    private float currHealth;

    //Timer controlling invincibility after hit
    private float hitTime;
    private float hitTimer;
    private bool canHit;

    void Start()
    {
        body = transform.Find("Body").gameObject;

        explosion = transform.Find("Explosion").gameObject;

        currHealth = maxHealth;
        healthBar.GetComponent<Slider>().maxValue = maxHealth;
        healthBar.GetComponent<Slider>().value = currHealth;

        hitTime = 0.5f;
        hitTimer = 0;
        canHit = true;

        bombCount.AddBomb(transform.gameObject);
    }

    void LateUpdate()
    {
        hitTimer += Time.deltaTime;
        if (hitTimer > hitTime) canHit = true;
    }

    //Applies damage to bomb
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

    //Handles behavior when bomb destroyed
    IEnumerator Die()
    {
        bombCount.RemoveBomb(this.gameObject);

        body.SetActive(false);

        explosion.SetActive(true);
        explosion.GetComponent<ParticleSystem>().Play();

        player.GetComponent<Player>().IncrementExp(expValue);

        yield return new WaitForSeconds(2);

        Destroy(this.gameObject);
    }
}
