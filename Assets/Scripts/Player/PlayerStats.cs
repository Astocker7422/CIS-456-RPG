using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
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

    }

}
