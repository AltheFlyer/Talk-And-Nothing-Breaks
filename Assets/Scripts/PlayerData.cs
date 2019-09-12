using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{

    public static int score = 0;


    public static void IncreaseScore(int a) {
        score += a;    
    }
}
