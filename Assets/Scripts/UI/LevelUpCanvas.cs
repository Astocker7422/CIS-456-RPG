using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*----------------------------------------------------------------------------------------
     LevelUpCanvas - Controls behavior of upgrade UI
----------------------------------------------------------------------------------------*/
public class LevelUpCanvas : MonoBehaviour
{
    private SpinControl HPSpin;
    private SpinControl SpeedSpin;
    private SpinControl PowerSpin;
    private SpinControl JumpSpin;

    private AttributePool attributePool;

    private TMPro.TMP_InputField HPInput;
    private TMPro.TMP_InputField SpeedInput;
    private TMPro.TMP_InputField PowerInput;
    private TMPro.TMP_InputField JumpInput;

    void Awake()
    {
        //Access the spin controls
        HPSpin = transform.FindDeepChild("HP Spin").GetComponent<SpinControl>();
        SpeedSpin = transform.FindDeepChild("Speed Spin").GetComponent<SpinControl>();
        PowerSpin = transform.FindDeepChild("Power Spin").GetComponent<SpinControl>();
        JumpSpin = transform.FindDeepChild("Jump Spin").GetComponent<SpinControl>();

        //Access and initialize the attribute point pool
        attributePool = transform.Find("Attribute Pool").GetComponent<AttributePool>();
        if (SceneManager.GetActiveScene().name == "StatMenu")
        {
            attributePool.SetValue(10);
        }
        else
        {
            attributePool.SetValue(5);
        }

        //Access the text elements of the spin controls
        HPInput = HPSpin.transform.Find("Value Text").GetComponent<TMPro.TMP_InputField>();
        SpeedInput = SpeedSpin.transform.Find("Value Text").GetComponent<TMPro.TMP_InputField>();
        PowerInput = PowerSpin.transform.Find("Value Text").GetComponent<TMPro.TMP_InputField>();
        JumpInput = JumpSpin.transform.Find("Value Text").GetComponent<TMPro.TMP_InputField>();
    }

    void OnEnable()
    {
        //Initialize the attribute point pool
        if (SceneManager.GetActiveScene().name == "StatMenu")
        {
            attributePool.SetValue(10);
        }
        else
        {
            attributePool.SetValue(5);
        }
    }

    //Set HP stat when level up screen opened
    public void SetHP(int value)
    {
        HPInput.text = value.ToString();
        HPSpin.minValue = value;
        HPSpin.maxValue = value + 5;
        HPSpin.value = value;
    }

    //Set Speed stat when level up screen opened
    public void SetSpeed(int value)
    {
        SpeedInput.text = value.ToString();
        SpeedSpin.minValue = value;
        SpeedSpin.maxValue = value + 5;
        SpeedSpin.value = value;
    }

    //Set Power stat when level up screen opened
    public void SetPower(int value)
    {
        PowerInput.text = value.ToString();
        PowerSpin.minValue = value;
        PowerSpin.maxValue = value + 5;
        PowerSpin.value = value;
    }

    //Set Jump stat when level up screen opened
    public void SetJump(int value)
    {
        JumpInput.text = value.ToString();
        JumpSpin.minValue = value;
        JumpSpin.maxValue = value + 5;
        JumpSpin.value = value;
    }

    //Get HP stat when level up screen closed
    public int GetHP()
    {
        return HPSpin.value;
    }

    //Get Speed stat when level up screen closed
    public int GetSpeed()
    {
         return SpeedSpin.value;
    }

    //Get Power stat when level up screen closed
    public int GetPower()
    {
        return PowerSpin.value;
    }

    //Get Jump stat when level up screen closed
    public int GetJump()
    {
        return JumpSpin.value;
    }
}
