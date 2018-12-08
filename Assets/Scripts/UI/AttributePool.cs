using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*----------------------------------------------------------------------------------------
     AttributePool - Controls behavior of attribute point pool UI on upgrade screens
----------------------------------------------------------------------------------------*/
public class AttributePool : MonoBehaviour
{
    public int value;

    private TextMeshProUGUI text;

	void Awake()
    {
        text = transform.Find("Text").GetComponent<TextMeshProUGUI>();

        foreach(Transform child in transform)
        {
            SpinControl spinner = child.GetComponent<SpinControl>();
            if(spinner != null)
            {
                spinner.AddReqListener(OnChangeRequest);
                spinner.AddCommListener(OnChangeCommit);
                spinner.minValue = spinner.value;
            }
        }
	}
	
	private void OnValidate()
    {
        text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        if (text)
        {
            text.text = value.ToString();
        }
    }

    private bool OnChangeRequest(object source, int delta)
    {
        SpinControl spinner = source as SpinControl;

        if(value - delta < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void OnChangeCommit(object source, int delta)
    {
        value -= delta;

        if(text)
        {
            text.text = value.ToString();
        }
    }

    public void SetValue(int newValue)
    {
        value = newValue;
        text.text = value.ToString();
    }
}
