using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpCanvas : MonoBehaviour
{
    private SpinControl HPSpin;
    private SpinControl SpeedSpin;
    private SpinControl PowerSpin;
    private SpinControl JumpSpin;

    void OnEnable()
    {
        HPSpin = transform.FindDeepChild("HP Spin").GetComponent<SpinControl>();
        SpeedSpin = transform.FindDeepChild("Speed Spin").GetComponent<SpinControl>();
        PowerSpin = transform.FindDeepChild("Power Spin").GetComponent<SpinControl>();
        JumpSpin = transform.FindDeepChild("Jump Spin").GetComponent<SpinControl>();
    }
	
	//void Update ()
    //{
		
	//}

    public void SetHP(int value)
    {
        HPSpin.minValue = value;
        HPSpin.value = value;
    }

    public void SetSpeed(int value)
    {
        SpeedSpin.minValue = value;
        SpeedSpin.value = value;
    }

    public void SetPower(int value)
    {
        PowerSpin.minValue = value;
        PowerSpin.value = value;
    }

    public void SetJump(int value)
    {
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
