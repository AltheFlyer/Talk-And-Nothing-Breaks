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
    public static string playerID;


    static PlayerData()
    {
        string chars = "1234567890";
        for (int i = 0; i < 16; i++) {
            int rand = StaticRandom.NextInt(chars.Length);
            playerID += chars[rand];
        }
        Debug.Log("PLAYER ID");
        Debug.Log(playerID);
    }

    public static void IncreaseScore(int a) {
        currentScore += a;    
    }
    
}
