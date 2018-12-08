using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------
     PlayerStats - Stores stats and experience of player
----------------------------------------------------------------------------------------*/
public class PlayerStats
{
    public int HP;
    public int Speed;
    public int Power;
    public int Jump;

    public int Level;
    public int CurrExp;
    public int MaxExp;

    private static PlayerStats _instance = null;

    public static PlayerStats Instance()
    {
        if(_instance == null)
        {
            _instance = new PlayerStats();
        }

        return _instance;
    }

    private PlayerStats()
    {
        HP = 10;
        Speed = 8;
        Power = 2;
        Jump = 12;

        Level = 1;
        CurrExp = 0;
        MaxExp = 1000;
    }

}
