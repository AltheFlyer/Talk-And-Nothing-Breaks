using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class StartScreenButtons : MonoBehaviour
{
    public BombData data;

    public Text additionalInfo;
    public Text scoreText;
    public Text titleText;
    public Text descText;
    public Text timeText;
    public Text levelScoreText;
    public Text completeText;

    public GameObject levelScreen;

    public void Start() {
        data = GameObject.Find("BombData").GetComponent<BombData>();
        levelScreen = transform.Find("Levels").gameObject;
        
        additionalInfo = levelScreen.transform.Find("Additional Info").Find("Panel Title").GetComponent<Text>();
        scoreText = levelScreen.transform.Find("Additional Info").Find("Score Display").GetComponent<Text>();
        titleText = levelScreen.transform.Find("Additional Info").Find("Level Title").GetComponent<Text>();
        descText = levelScreen.transform.Find("Additional Info").Find("Level Description").GetComponent<Text>();
        timeText = levelScreen.transform.Find("Additional Info").Find("Best Time").GetComponent<Text>();
        levelScoreText = levelScreen.transform.Find("Additional Info").Find("Level Score").GetComponent<Text>();
        completeText = levelScreen.transform.Find("Additional Info").Find("Complete").GetComponent<Text>();

        if (PlayerData.playerID == null) {
            string chars = "1234567890";
            for (int i = 0; i < 16; i++) {
                int rand = StaticRandom.NextInt(chars.Length);
                PlayerData.playerID += chars[rand];
            }
        }

        scoreText.text = "Total Score: " + PlayerData.totalScore;
    }

    ///Runs a level based on whatever the current level configuration is.
    public void Play() {
        SceneManager.LoadScene("LevelGeneratorScene");
    }

    ///Sets the level configuration
    public void PlayLevel(string name) {
        data.SetData(name);
        PlayerData.currentLevel = name;
        PlayerData.currentScore = 0;
        PlayerData.currentStrikes = 0;
        Play();
    }

    public void SetAdditionalInfo(string name) {
        BombInfo tempData = JsonUtility.FromJson<BombInfo>(Levels.GetGenerator(name));
        titleText.text = tempData.name;
        descText.text = tempData.description;
        if (PlayerData.stats.ContainsKey(name))
        {
            if (PlayerData.stats[name].win)
            {
                timeText.text = "Best Time: " + ((int)(PlayerData.stats[name].time / 60)).ToString("00") + ":" + (PlayerData.stats[name].time % 60).ToString("00.00");
                completeText.text = "Complete: ✓";
            }
            else
            {
                timeText.text = "Best Time: N/A";
                completeText.text = "Complete: ✘";
            }
            levelScoreText.text = "Best Score: " + PlayerData.stats[name].score;
        } else
        {
            timeText.text = "Best Time: N/A";
            levelScoreText.text = "Best Score: N/A";
            completeText.text = "Complete: ✘";
        }
    }
}
