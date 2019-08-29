﻿using System.Collections;
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

    public void Start() {
        UpdateText();
        data = GameObject.Find("BombData").GetComponent<BombData>();
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

    public void PlayBooleanOnly() {
        data.width = 1;
        data.height = 1;
        data.numModules = 2;
        data.SelectModule("boolean");
        data.Use();
        Play();
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

    public void PlaySingleModule(string name) {
        data.width = 1;
        data.height = 1;
        data.numModules = 2;
        data.SelectModule(name);
        data.Use();
        Play();
    }
}
