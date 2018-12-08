using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------
     HealthPack - Controls behavior of healing packs
----------------------------------------------------------------------------------------*/
public class HealthPack : MonoBehaviour
{
    private GameObject horizontal;
    private GameObject vertical;
    private BoxCollider trigger;

    private bool active;

    private float respawnTimer;
    private float respawnTime;

    void Start()
    {
        horizontal = transform.Find("Horizontal").gameObject;
        vertical = transform.Find("Vertical").gameObject;
        trigger = GetComponent<BoxCollider>();

        active = true;

        respawnTime = 60;
        respawnTimer = 0;
    }

    void Update()
    {
        if (respawnTimer >= respawnTime)
        {
            Activate();
        }
    }

    void LateUpdate()
    {
        if (!active)
        {
            respawnTimer += Time.deltaTime;
        }
    }

    private void Deactivate()
    {
        respawnTimer = 0;

        active = false;

        horizontal.SetActive(false);
        vertical.SetActive(false);
        trigger.enabled = false;
    }

    private void Activate()
    {
        active = true;

        horizontal.SetActive(true);
        vertical.SetActive(true);
        trigger.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if(!player.isHealthFull())
            {
                player.Heal();

                Deactivate();
            }
        }
    }
}
