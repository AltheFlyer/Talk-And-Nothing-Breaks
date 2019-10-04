using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public Text titleText;
    public Text descText;

    //Button sets
    public GameObject currentSubScreen;
    public GameObject mainScreen;
    public GameObject levelScreen;

    public void Start() {
        data = GameObject.Find("BombData").GetComponent<BombData>();
        mainScreen = transform.Find("Main").gameObject;
        levelScreen = transform.Find("Levels").gameObject;
        additionalInfo = levelScreen.transform.Find("Additional Info").Find("Panel Title").GetComponent<Text>();
        titleText = levelScreen.transform.Find("Additional Info").Find("Level Title").GetComponent<Text>();
        descText = levelScreen.transform.Find("Additional Info").Find("Level Description").GetComponent<Text>();
        
        currentSubScreen = levelScreen;
        //levelScreen.SetActive(false);
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
        data.SetData("Assets/Generators/" + name + ".json");
        PlayerData.currentLevel = name;
        Play();
    }

    public void WeightTest() {
        data.SetData("Assets/Generators/weighted.json");
        Play();
    }

    public void SetAdditionalInfo(string name) {
        BombInfo tempData = JsonUtility.FromJson<BombInfo>(FileIO.ReadString("Assets/Generators/" + name + ".json"));
        titleText.text = tempData.name;
        descText.text = tempData.description;
    }
}
