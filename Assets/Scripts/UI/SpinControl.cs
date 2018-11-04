using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinControl : MonoBehaviour
{
    public int value;
    public int minValue;
    public int maxValue;

    public delegate bool ChangeEventRequest(object source, int delta);
    public delegate void ChangeEventCommit(object source, int delta);
    public event ChangeEventCommit commListeners;
    public event ChangeEventRequest reqListeners;

    private int preValue;

    private Button up_button;
    private Button down_button;
    private TMPro.TMP_InputField input;

	void Start ()
    {
        up_button = transform.Find("Up Button").GetComponent<Button>();
        down_button = transform.Find("Down Button").GetComponent<Button>();
        input = transform.Find("Value Text").GetComponent<TMPro.TMP_InputField>();

        preValue = value;

        up_button.onClick.AddListener( delegate { ButtonHandler(1); } );
        down_button.onClick.AddListener(delegate { ButtonHandler(-1); });

        input.onEndEdit.AddListener( delegate { InputHandler(); } );
    }

    private void ButtonHandler(int delta)
    {
        if(input != null)
        {
            preValue = value;
            value = int.Parse(input.text);
            value += delta;
            input.text = value.ToString();
            RangeCheck();
        }
    }

    private void InputHandler()
    {
        preValue = value;
        value = int.Parse(input.text);
        RangeCheck();
    }

    private void RangeCheck()
    {
        if (value > maxValue)
        {
            value = maxValue;
        }
        else if(value < minValue)
        {
            value = minValue;
        }
        input.text = value.ToString();

        if(reqListeners != null)
        {
            foreach(ChangeEventRequest req in reqListeners.GetInvocationList())
            {
                if(req(this, value - preValue) == false)
                {
                    value = preValue;
                    input.text = value.ToString();
                    return;
                }
            }
            
            if((commListeners != null) && (value != preValue))
            {
                commListeners(this, value - preValue);
            }
        }
    }

    //Changes value on object when changed in editor
    private void OnValidate()
    {
        input = transform.Find("Value Text").GetComponent<TMPro.TMP_InputField>();
        input.text = value.ToString();
    }

    public void AddReqListener(ChangeEventRequest eh)
    {
        reqListeners += eh;
    }

    public void RemoveReqListener(ChangeEventRequest eh)
    {
        reqListeners -= eh;
    }

    public void AddCommListener(ChangeEventCommit eh)
    {
        commListeners += eh;
    }

    public void RemoveCommListener(ChangeEventCommit eh)
    {
        commListeners -= eh;
    }
}
