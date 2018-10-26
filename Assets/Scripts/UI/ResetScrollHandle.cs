using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetScrollHandle : MonoBehaviour
{
    private void Start()
    {
        transform.GetComponent<Scrollbar>().size = 1;
        transform.GetComponent<Scrollbar>().value = 0;
    }
}
