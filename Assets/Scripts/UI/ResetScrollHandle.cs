using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetScrollHandle : MonoBehaviour
{
    private void Start()
    {
        transform.FindDeepChild("Scrollbar").GetComponent<Scrollbar>().size = 1;
        transform.FindDeepChild("Scrollbar").GetComponent<Scrollbar>().value = 0;
    }
}
