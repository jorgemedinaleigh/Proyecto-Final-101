using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils
{
    public static bool IsGamePaused
    {
        get 
        {
            return Time.timeScale == 0;
        }
    }
}
