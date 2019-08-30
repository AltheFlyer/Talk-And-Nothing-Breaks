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

    //Button sets
    public GameObject currentSubScreen;
    public GameObject mainScreen;
    public GameObject levelScreen;

    public void Start() {
        UpdateText();
        data = GameObject.Find("BombData").GetComponent<BombData>();
        mainScreen = transform.Find("Main").gameObject;
        levelScreen = transform.Find("Levels").gameObject;

        currentSubScreen = mainScreen;
        levelScreen.SetActive(false);
    }

    public void Play() {
        SceneManager.LoadScene("LevelGeneratorScene");
    }

    public void IncrementModules() {
        if (LevelData.numModules < MaxModules()) {
            LevelData.numModules++;
        }
        UpdateText();
    }

    public void DecrementModules() {
        if (LevelData.numModules > 1) {
            LevelData.numModules--;
        }
        UpdateText();
    }

    public void IncrementWidth() {
        LevelData.width++;
        UpdateText();
    }

    public void DecrementWidth() {
        if (LevelData.width > 0) {
            LevelData.width--;
            if (LevelData.numModules > MaxModules()) {
                LevelData.numModules = MaxModules();
            }
        }
        UpdateText();
    }

    public void IncrementHeight() {
        LevelData.height++;
        UpdateText();
    }

    public void DecrementHeight() {
        if (LevelData.height > 0) {
            LevelData.height--;
            if (LevelData.numModules > MaxModules()) {
                LevelData.numModules = MaxModules();
            }
        }
        UpdateText();
    }

    private int MaxModules() {
        return LevelData.width * LevelData.height * 2;
    }

    private void UpdateText() {
        BombInfoPanel.GetComponent<Text>().text = 
            "Modules: " + LevelData.numModules.ToString() + "\n" +
            "Width: " + LevelData.width.ToString() + "\n" +
            "Height: " + LevelData.height.ToString() + "\n";
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
        data.width = 1;
        data.height = 1;
        data.numModules = 2;
        data.SelectModule(name);
        data.Use();
        Play();
    }

    public void WeightTest() {
        data.width = 3;
        data.height = 2;
        data.numModules = 12;
        data.SelectModule("boolean", 5);
        data.SelectModule("addition", 1);
        data.Use();
        Play();
    }
}
