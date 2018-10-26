using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityStandardAssets.CrossPlatformInput;

public class Console : MonoBehaviour
{
    private ConsoleInputField input;
    private Scrollbar vScroll;
    private TextMeshProUGUI text;

	// Use this for initialization
	void Start ()
    {
        input = transform.FindDeepChild("Console Input").GetComponent<ConsoleInputField>();
        vScroll = transform.FindDeepChild("Scrollbar").GetComponent<Scrollbar>();
        text = transform.FindDeepChild("Console Text").GetComponent<TextMeshProUGUI>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyUp(KeyCode.Slash))
        {
            input.ActivateInputField();
            input.Select();
            input.text = "/";
        }
	}
}
