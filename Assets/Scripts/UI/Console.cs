using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityStandardAssets.CrossPlatformInput;
using System;

public class Console : MonoBehaviour
{
    public delegate void ConsoleInputHandler(object source, ConsoleArguments args);
    public static event ConsoleInputHandler listeners;

    private TMP_InputField input;
    private Scrollbar vScroll;
    private TextMeshProUGUI consoleText;
    private ScrollRect scrollRect;

	// Use this for initialization
	void Start ()
    {
        input = transform.FindDeepChild("Console Input").GetComponent<TMP_InputField>();
        vScroll = transform.FindDeepChild("Scrollbar").GetComponent<Scrollbar>();
        consoleText = transform.FindDeepChild("Console Text").GetComponent<TextMeshProUGUI>();
        scrollRect = transform.FindDeepChild("Text Panel").GetComponent<ScrollRect>();

        input.onEndEdit.AddListener(delegate { InputEntered(); });
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

    private void InputEntered()
    {
        AddString(input.text);

        if(listeners != null)
        {
            listeners(this, new ConsoleArguments(input.text));
        }
    }

    public void AddString(string s)
    {
        consoleText.text += "\n" + s;
        StartCoroutine(ScrollToBottom());
    }

    void LateUpdate()
    {
        input.MoveTextEnd(false);
    }

    IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        scrollRect.verticalNormalizedPosition = 0;
    }

    public static void AddListener(ConsoleInputHandler eh)
    {
        listeners += eh;
    }

    public static void RemoveListener(ConsoleInputHandler eh)
    {
        listeners -= eh;
    }
}

public class ConsoleArguments : EventArgs
{
    char[] delimiters = { ' ', '\t', ',' };
    private string[] _args;

    public ConsoleArguments(string str)
    {
        _args = str.Split(delimiters);
    }

    public string[] args
    {
        get { return _args; }
    }
}
