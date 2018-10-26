using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ConsoleInputField : TMP_InputField
{
    protected override void LateUpdate()
    {
        base.LateUpdate();

        if(EventSystem.current != null)
        {
            if(this.gameObject.Equals(EventSystem.current.currentSelectedGameObject))
            {
                MoveTextEnd(false);
            }
        }
    }
}
