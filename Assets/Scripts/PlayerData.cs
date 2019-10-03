using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{

    public static int totalScore = 0;
    public static int currentScore = 0;
    public static float time = 0;
    public static string death = "";
    public static bool win = false;
    
    public static void IncreaseScore(int a) {
        currentScore += a;    
    }
    
}
