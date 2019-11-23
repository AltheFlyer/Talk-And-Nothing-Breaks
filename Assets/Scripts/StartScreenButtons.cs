using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class StartScreenButtons : MonoBehaviour
{
    
    [SerializeField]
    private GameObject  IncreaseModules, 
                        DecreaseModules,
                        IncreaseWidth,
                        DecreaseWidth,
                        IncreaseHeight,
                        DecreaseHeight,
                        BombInfoPanel;

    public BombData data;

    public Text additionalInfo;
    public Text scoreText;
    public Text titleText;
    public Text descText;
    public Text timeText;
    public Text levelScoreText;
    public Text completeText;

    public InputField userField;
    public InputField passField;

    //Button sets
    public GameObject currentSubScreen;
    public GameObject mainScreen;
    public GameObject levelScreen;
    public GameObject loginScreen;

    public void Start() {
        data = GameObject.Find("BombData").GetComponent<BombData>();
        mainScreen = transform.Find("Main").gameObject;
        levelScreen = transform.Find("Levels").gameObject;
        loginScreen = transform.Find("Login").gameObject;

        additionalInfo = levelScreen.transform.Find("Additional Info").Find("Panel Title").GetComponent<Text>();
        scoreText = levelScreen.transform.Find("Additional Info").Find("Score Display").GetComponent<Text>();
        titleText = levelScreen.transform.Find("Additional Info").Find("Level Title").GetComponent<Text>();
        descText = levelScreen.transform.Find("Additional Info").Find("Level Description").GetComponent<Text>();
        timeText = levelScreen.transform.Find("Additional Info").Find("Best Time").GetComponent<Text>();
        levelScoreText = levelScreen.transform.Find("Additional Info").Find("Level Score").GetComponent<Text>();
        completeText = levelScreen.transform.Find("Additional Info").Find("Complete").GetComponent<Text>();

        userField = loginScreen.transform.Find("Username Input").GetComponent<InputField>();
        passField = loginScreen.transform.Find("Password Input").GetComponent<InputField>();

        if (PlayerData.playerID == null) {
            currentSubScreen = loginScreen;
            //currentSubScreen = levelScreen;
            levelScreen.SetActive(false);
        } else {
            currentSubScreen = levelScreen;
            loginScreen.SetActive(false);
            SetScreen("levels");
        }
        scoreText.text = "Total Score: " + PlayerData.totalScore;
        UpdateText();
    }

    public void Play() {
        SceneManager.LoadScene("LevelGeneratorScene");
    }

    public void IncrementModules() {
        if (data.meta.numModules < MaxModules()) {
            data.meta.numModules++;
        }
        UpdateText();
    }

    public void DecrementModules() {
        if (data.meta.numModules > 1) {
            data.meta.numModules--;
        }
        UpdateText();
    }

    public void IncrementWidth() {
        data.meta.width++;
        UpdateText();
    }

    public void DecrementWidth() {
        if (data.meta.width > 1) {
            data.meta.width--;
            if (data.meta.numModules > MaxModules()) {
                data.meta.numModules = MaxModules();
            }
        }
        UpdateText();
    }

    public void IncrementHeight() {
        data.meta.height++;
        UpdateText();
    }

    public void DecrementHeight() {
        if (data.meta.height > 1) {
            data.meta.height--;
            if (data.meta.numModules > MaxModules()) {
                data.meta.numModules = MaxModules();
            }
        }
        UpdateText();
    }

    private int MaxModules() {
        return data.meta.width * data.meta.height * 2 - 1;
    }

    private void UpdateText() {
        print(BombInfoPanel.GetComponent<Text>());
        BombInfoPanel.GetComponent<Text>().text = 
            "Overall Score: " + PlayerData.totalScore.ToString() + "\n" +
            "Modules: " + data.meta.numModules.ToString() + "\n" +
            "Width: " + data.meta.width.ToString() + "\n" +
            "Height: " + data.meta.height.ToString() + "\n";
    }

    public void SetScreen(string name) {
        currentSubScreen.SetActive(false);
        if (name == "main") {
            currentSubScreen = mainScreen;
        } else if (name == "levels") {
            currentSubScreen = levelScreen;
            data.SendData();
        }
        currentSubScreen.SetActive(true);
    }

    public void PlaySingleModule(string name) {
        data.meta.width = 1;
        data.meta.height = 1;
        data.meta.numModules = 1;
        data.meta.time = 60;
        ModuleInfo mInfo = new ModuleInfo();
        mInfo.name = name;
        data.meta.modules = new List<ModuleInfo>();
        data.meta.modules.Add(mInfo);
        Play();
    }

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

    public void LoginToGame() {
        

        StartCoroutine(SD());
    }

    public IEnumerator SD() {
        string username = userField.text;
        string password = passField.text;

        WWWForm form = new WWWForm();
        form.AddField("user", username);
        form.AddField("password", password);
        
        Debug.Log("Sending Data");
        
        using (var www = UnityWebRequest.Post(WebManager.loginCheck, form)) {
            yield return www.SendWebRequest();
            
            if (www.isNetworkError || www.isHttpError) {
                Debug.Log("ERROR");
            }
            else {
                Debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text == "SUCCESS") {
                    PlayerData.playerID = username;
                    SetScreen("levels");
                }
            }
        }
    }
}
