using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*----------------------------------------------------------------------------------------
     BombTimer - Controls timer UI in City level
----------------------------------------------------------------------------------------*/
public class BombTimer : MonoBehaviour
{
    //The player
    public Player player;

    //Out of time UI
    public GameObject loseScreen;

    //Time to start timer at
    private float startTime;

    //The value of the timer
    private float timer;

    //Indicates if the timer should run
    private bool canCount;

    //The UI element
    private TextMeshProUGUI text;

    void Start()
    {
        canCount = false;

        //10 minutes
        startTime = 600;

        timer = 0;
        
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (canCount)
        {
            timer += Time.deltaTime;

            float time = startTime - timer;

            if (time <= 0)
            {
                text.text = "0.00";

                ActivateLoseScreen();
            }
            else
            {
                string minutes = ((int)time / 60).ToString();
                string seconds = (time % 60).ToString("f2");

                text.text = minutes + ":" + seconds;
            }
        }
    }

    public void StartTimer()
    {
        canCount = true;
    }

    //Activates City level losing UI
    private void ActivateLoseScreen()
    {
        player.Paused(true);

        loseScreen.SetActive(true);

        Time.timeScale = 0;
    }
}
