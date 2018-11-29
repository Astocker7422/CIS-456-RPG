using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCount : MonoBehaviour
{
    public GameObject boss;

    //The number of enemies in the level
    private int count;

    private int enemiesKilled;

    //List of all enemies in scene
    private List<GameObject> enemyList;

    //The text displaying the number of enemies
    private TextMeshProUGUI text;

    void Awake()
    {
        enemyList = new List<GameObject>();

        text = GetComponent<TextMeshProUGUI>();

        text.text = "Enemies Defeated: 0";
    }

    void Update()
    {
        if(!boss.activeInHierarchy)
        {
            if(enemiesKilled >= 1)
            {
                boss.SetActive(true);
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
}
