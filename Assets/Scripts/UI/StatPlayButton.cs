using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatPlayButton : MonoBehaviour
{
    private AttributePool pool;
    private Button playButton;

    private SpinControl HPSpin;
    private SpinControl SpeedSpin;
    private SpinControl PowerSpin;
    private SpinControl JumpSpin;

	void Start ()
    {
        pool = GetComponent<AttributePool>();
        playButton = transform.FindDeepChild("Play Button").GetComponent<Button>();

        HPSpin = transform.Find("HP Spin").GetComponent<SpinControl>();
        SpeedSpin = transform.Find("Speed Spin").GetComponent<SpinControl>();
        PowerSpin = transform.Find("Power Spin").GetComponent<SpinControl>();
        JumpSpin = transform.Find("Jump Spin").GetComponent<SpinControl>();
    }
	
	void Update ()
    {
		if(!playButton.interactable)
        {
            if(pool.value == 0)
            {
                PlayerStats.Instance().HP = HPSpin.value;
                PlayerStats.Instance().Speed = SpeedSpin.value;
                PlayerStats.Instance().Power = PowerSpin.value;
                PlayerStats.Instance().Jump = JumpSpin.value;

                playButton.interactable = true;
            }
        }
        else
        {
            if(pool.value != 0)
            {
                playButton.interactable = false;
            }
        }
	}
}
