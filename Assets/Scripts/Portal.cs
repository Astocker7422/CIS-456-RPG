using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    //Indicates if the portal to the next level is passable
    public bool isPassable;

	// Use this for initialization
	void Start ()
    {
        isPassable = false;
	}

    public void OnCollisionEnter()
    {
        if(isPassable)
        {
            GetComponent<SceneLoader>().LoadCity();
        }
    }
}
