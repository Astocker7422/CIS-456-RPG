using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*----------------------------------------------------------------------------------------
     EnemyCount - Controls enemy counter UI in Outskirts level
----------------------------------------------------------------------------------------*/
public class EnemyCount : MonoBehaviour
{
    //The boss of the level
    public GameObject boss;

    //The camera to show the boss and the gate
    public GameObject extraCamera;

    //The player
    private Player player;

    //The gate to exit the level
    private GameObject portal;

    //Position and rotation of camera when showing portal
    private Vector3 cameraPortalPos;
    private Quaternion cameraPortalRot;

    //The number of enemies in the level
    private int count;

    //The number of enemies killed by the player
    private int enemiesKilled;

    //List of all enemies in scene
    private List<GameObject> enemyList;

    //The text displaying the number of enemies
    private TextMeshProUGUI text;

    //Indicates if the boss has been activated
    private bool bossActive;

    //Indicates if the portal has been activated
    private bool portalActive;

    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        portal = GameObject.Find("Gate").transform.Find("Portal").gameObject;

        cameraPortalPos = new Vector3(220, 15.5f, 400);
        cameraPortalRot = new Quaternion(0, 0, 0, 0);

        enemyList = new List<GameObject>();

        text = GetComponent<TextMeshProUGUI>();

        text.text = "Enemies Defeated: 0";

        bossActive = false;
        portalActive = false;
    }

    void Update()
    {
        //If the boss has not been activated,
        if(!bossActive)
        {
            //If the appropriate amount of enemies have been killed,
            if(enemiesKilled >= 10)
            {
                //Activate the boss
                boss.SetActive(true);

                //Show boss with extra camera
                StartCoroutine(ShowBoss());

                //Indicate the boss has been activated
                bossActive = true;
            }
        }
        //If the boss has been activated,
        else
        {
            //If the boss object is not active,
            if(boss == null && !portalActive)
            {
                //Show the portal with extra camera
                StartCoroutine(ShowPortal());
            }
        }
    }
    
    //Add enemy to list
	public void AddEnemy(GameObject enemy)
    {
        count++;

        enemyList.Add(enemy);
    }

    //Increment the enemy kill count on the UI
    public void KillCount(GameObject enemy)
    {
        enemiesKilled++;

        text.text = "Enemies Defeated: " + enemiesKilled.ToString();
    }

    //Increases all enemy stats when player levels up
    public void ScaleEnemies()
    {
        foreach(GameObject enemy in enemyList)
        {
            enemy.GetComponent<Enemy>().ScaleStats();
        }
    }

    //Show the boss with the extra camera
    private IEnumerator ShowBoss()
    {
        extraCamera.SetActive(true);
        player.AllowOtherCamera(true);

        yield return new WaitForSeconds(4);

        player.AllowOtherCamera(false);
        extraCamera.SetActive(false);
    }

    //Show the portal with the extra camera
    private IEnumerator ShowPortal()
    {
        portal.GetComponent<Renderer>().material = Resources.Load("Portal") as Material;
        portal.GetComponent<Portal>().isPassable = true;

        extraCamera.transform.position = cameraPortalPos;
        extraCamera.transform.rotation = cameraPortalRot;

        player.AllowOtherCamera(true);
        extraCamera.SetActive(true);

        portalActive = true;

        yield return new WaitForSeconds(4);

        player.AllowOtherCamera(false);
        extraCamera.SetActive(false);
    }
}
