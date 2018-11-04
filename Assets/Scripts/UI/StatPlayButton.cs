using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatPlayButton : MonoBehaviour
{
    private AttributePool pool;
    private Button playButton;

	void Start ()
    {
        pool = GetComponent<AttributePool>();
        playButton = transform.FindDeepChild("Play Button").GetComponent<Button>();
	}
	
	void Update ()
    {
		if(!playButton.interactable)
        {
            if(pool.value == 0)
            {
                playButton.interactable = true;
            }
        }
        else
        {
            if(pool.value != 0)
            {
                playButton.interactable = false;
            }
        }
	}
}
