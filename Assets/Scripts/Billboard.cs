using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------
     Billboard - Controls behavior of billboarded UI
----------------------------------------------------------------------------------------*/
public class Billboard : MonoBehaviour
{
    void Update()
    {
        if(Camera.main != null)
        {
            //Make object face the camera
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }
}
