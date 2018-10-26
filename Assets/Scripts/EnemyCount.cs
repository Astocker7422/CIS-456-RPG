using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCount : MonoBehaviour
{
    //The number of enemies in the level
    private int count;

    //The text displaying the number of enemies
    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    
    //Increment the enemy count on the UI
	public void IncrementCount()
    {
        count++;

        text.text = "Enemies: " + count.ToString();
    }

    //Decrement the enemy count on the UI
    public void DecrementCount()
    {
        count--;

        text.text = "Enemies: " + count.ToString();
    }
}
