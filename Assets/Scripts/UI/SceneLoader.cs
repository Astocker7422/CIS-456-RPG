using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Controls which scene is loaded
//Attached to empty object
public class SceneLoader : MonoBehaviour
{
    //Load the demo scene
    public void LoadOutskirts()
    {
        SceneManager.LoadScene("Outskirts");
    }

    //Load the main menu scene
    public void LoadStatMenu()
    {
        SceneManager.LoadScene("StatMenu");
    }

    //Exit the game
    public void Quit()
    {
        Application.Quit();
    }
}
