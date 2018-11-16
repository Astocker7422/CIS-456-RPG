using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Level = 1;
        CurrExp = 0;
        MaxExp = 1000;
    }

}
