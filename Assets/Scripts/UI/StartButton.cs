using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------
     StartButton - Provides method to unpause the game
----------------------------------------------------------------------------------------*/
public class StartButton : MonoBehaviour
{
    public void Unpause()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }
}
