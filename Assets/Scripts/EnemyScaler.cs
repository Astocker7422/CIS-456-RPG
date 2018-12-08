using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------
     EnemyScaler - Scales all enemies' stats on player level up
----------------------------------------------------------------------------------------*/
public class EnemyScaler : MonoBehaviour
{
    //The player
    private Player player;

    //List of all enemies in scene
    private List<GameObject> enemyList;

    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        enemyList = new List<GameObject>();

        foreach(Transform child in transform)
        {
            enemyList.Add(child.gameObject);
        }
    }

    //Increases all enemy stats when player levels up
    public void ScaleEnemies()
    {
        foreach (GameObject enemy in enemyList)
        {
            enemy.GetComponent<Enemy>().ScaleStats();
        }
    }
}
