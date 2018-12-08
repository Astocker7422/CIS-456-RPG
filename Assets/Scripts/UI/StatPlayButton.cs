using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*----------------------------------------------------------------------------------------
     StatPlayButton - Controls play button on upgrade UI
----------------------------------------------------------------------------------------*/
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
        //Access attribute point pool, play button, and spin controls
        pool = GetComponent<AttributePool>();
        playButton = transform.FindDeepChild("Play Button").GetComponent<Button>();

        HPSpin = transform.Find("HP Spin").GetComponent<SpinControl>();
        SpeedSpin = transform.Find("Speed Spin").GetComponent<SpinControl>();
        PowerSpin = transform.Find("Power Spin").GetComponent<SpinControl>();
        JumpSpin = transform.Find("Jump Spin").GetComponent<SpinControl>();
    }
	
	void Update ()
    {
        //Activate the button if all points used
        //Deactivate if not
		if(!playButton.interactable)
        {
            if(pool.value == 0)
            {
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

    //Update PlayerStats class
    public void ApplyStats()
    {
        PlayerStats.Instance().HP = HPSpin.value;
        PlayerStats.Instance().Speed = SpeedSpin.value;
        PlayerStats.Instance().Power = PowerSpin.value;
        PlayerStats.Instance().Jump = JumpSpin.value;
    }
}
