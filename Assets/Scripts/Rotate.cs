using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------
     Rotate - Controls rotating object
----------------------------------------------------------------------------------------*/
public class Rotate : MonoBehaviour
{
    //The rotation values
    public float x;
    public float y;
    public float z;

    void Update ()
    {
        //Rotate the object based on the input
        transform.Rotate(new Vector3(x, y, z) * Time.deltaTime);
	}
}
