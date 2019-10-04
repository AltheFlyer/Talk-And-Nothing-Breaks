using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{

    public static int totalScore = 0;
    public static int currentScore = 0;
    public static int currentStrikes = 0;
    public static string currentLevel = "";
    public static float time = 0;
    public static string death = "";
    public static bool win = false;
    public static string playerID;
    public static Dictionary<string, LevelStats> stats = new Dictionary<string, LevelStats>();

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

    public static void UpdateLevelStats (string name)
    {
        if (stats.ContainsKey(name)) {
            stats[name].UpdateStats(time, currentStrikes, currentScore, win);
        } else {
            stats.Add(name, new LevelStats(time, currentStrikes, currentScore, win));
        }
    }

    public class LevelStats
    {
        public float time;
        public int strikes;
        public int score;
        public bool win;

        public LevelStats (float time, int strikes, int score, bool win)
        {
            this.time = time;
            this.strikes = strikes;
            this.score = score;
            this.win = win;
        }

        public void UpdateStats (float time, int strikes, int score, bool win)
        {
            if (score > this.score) {
                this.score = score;
            }
            if (win) {
                this.win = win;
                if (time < this.time) {
                    this.time = time;
                }
                if (strikes < this.strikes) {
                    this.strikes = strikes;
                }
                
            }
        }
    }
    
}
