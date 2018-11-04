using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnNew()
    {
        StartCoroutine(LoadCharacterCreate());
    }

    IEnumerator LoadCharacterCreate()
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync("");

        while(!loading.isDone)
        {
            yield return null;
        }
    }
}
