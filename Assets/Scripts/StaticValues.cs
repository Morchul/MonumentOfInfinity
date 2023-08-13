using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticValues
{
    //1 = easy, 2 = medium, 3 = hard
    public static int difficulty;

    public static float GetStrikeThreshold()
    {
        switch (difficulty)
        {
            case 1: return 20;
            case 2: return 25;
            case 3: return 30;
            default: return 20;
        }
    }

    public static float GetAverageAge()
    {

        switch (difficulty)
        {
            case 1: return 40 * 60;
            case 2: return 30 * 60;
            case 3: return 20 * 60;
            default: return 40 * 60;
        }
    }
}
