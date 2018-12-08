using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*----------------------------------------------------------------------------------------
     BombCount - Controls bomb count UI in City level
----------------------------------------------------------------------------------------*/
public class BombCount : MonoBehaviour
{
    public Player player;

    public GameObject winScreen;

    //List of all enemies in scene
    private List<GameObject> bombList;

    //The text displaying the number of bombs
    private TextMeshProUGUI text;

    // Use this for initialization
    void Awake()
    {
        bombList = new List<GameObject>();

        text = GetComponent<TextMeshProUGUI>();

        text.text = "Bombs Remaining: ";
    }

    //Add bomb in level to list and update UI
    public void AddBomb(GameObject bomb)
    {
        bombList.Add(bomb);

        text.text = "Bombs Remaining: " + bombList.Count;
    }

    //Add bomb in level to list and update UI
    public void RemoveBomb(GameObject bomb)
    {
        bombList.Remove(bomb);

        text.text = "Bombs Remaining: " + bombList.Count;

        if (bombList.Count == 0)
        {
            ActivateWinScreen();
        }
    }

    //Activates City level win UI
    private void ActivateWinScreen()
    {
        player.Paused(true);

        winScreen.SetActive(true);

        Time.timeScale = 0;
    }
}
