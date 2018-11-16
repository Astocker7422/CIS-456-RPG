using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCount : MonoBehaviour
{
    //The number of enemies in the level
    private int count;

    //List of all enemies in scene
    private List<GameObject> enemyList;

    //The text displaying the number of enemies
    private TextMeshProUGUI text;

    void Awake()
    {
        enemyList = new List<GameObject>();

        text = GetComponent<TextMeshProUGUI>();
    }
    
    //Increment the enemy count on the UI
	public void IncrementCount(GameObject enemy)
    {
        if(text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        count++;
        
        text.text = "Enemies: " + count.ToString();

        enemyList.Add(enemy);
    }

    //Decrement the enemy count on the UI
    public void DecrementCount(GameObject enemy)
    {
        count--;

        text.text = "Enemies: " + count.ToString();

        enemyList.Remove(enemy);
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
