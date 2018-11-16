using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpCanvas : MonoBehaviour
{
    private SpinControl HPSpin;
    private SpinControl SpeedSpin;
    private SpinControl PowerSpin;
    private SpinControl JumpSpin;

    private TMPro.TMP_InputField HPInput;
    private TMPro.TMP_InputField SpeedInput;
    private TMPro.TMP_InputField PowerInput;
    private TMPro.TMP_InputField JumpInput;

    void OnEnable()
    {
        HPSpin = transform.FindDeepChild("HP Spin").GetComponent<SpinControl>();
        SpeedSpin = transform.FindDeepChild("Speed Spin").GetComponent<SpinControl>();
        PowerSpin = transform.FindDeepChild("Power Spin").GetComponent<SpinControl>();
        JumpSpin = transform.FindDeepChild("Jump Spin").GetComponent<SpinControl>();

        transform.Find("Attribute Pool").GetComponent<AttributePool>().value = 5;

        HPInput = HPSpin.transform.Find("Value Text").GetComponent<TMPro.TMP_InputField>();
        SpeedInput = SpeedSpin.transform.Find("Value Text").GetComponent<TMPro.TMP_InputField>();
        PowerInput = PowerSpin.transform.Find("Value Text").GetComponent<TMPro.TMP_InputField>();
        JumpInput = JumpSpin.transform.Find("Value Text").GetComponent<TMPro.TMP_InputField>();
    }
	
	//void Update ()
    //{
		
	//}

    public void SetHP(int value)
    {
        HPInput.text = value.ToString();
        HPSpin.minValue = value;
        HPSpin.value = value;
    }

    public void SetSpeed(int value)
    {
        SpeedInput.text = value.ToString();
        SpeedSpin.minValue = value;
        SpeedSpin.value = value;
    }

    public void SetPower(int value)
    {
        PowerInput.text = value.ToString();
        PowerSpin.minValue = value;
        PowerSpin.value = value;
    }

    public void SetJump(int value)
    {
        JumpInput.text = value.ToString();
        JumpSpin.minValue = value;
        JumpSpin.value = value;
    }

    public int GetHP()
    {
        return HPSpin.value;
    }

    public int GetSpeed()
    {
         return SpeedSpin.value;
    }

    public int GetPower()
    {
        return PowerSpin.value;
    }

    public int GetJump()
    {
        return JumpSpin.value;
    }
}
