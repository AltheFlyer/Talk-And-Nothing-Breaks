using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using System;

//TODO: implement glyphs or something
public class AlchemyModuleScript: Module
{

    int startSymbol;
    int currentSymbol;
    //Get an array of textures or something
    static string[] symbol = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J"};    
    
    public Texture[] symbolTextures;
    GameObject symbolText;
    GameObject mainIcon;

    //For the symbol at index i in symbols, reaching pass[i] = win, fail[i] = lose
    //PassSymbols
    static int[] passSymbols = {5, 6, 7, 4, 8, 9, 3, 1, 0, 2};
    //FailSymbols
    static int[] failSymbols = {1, 9, 4, 0, 2, 7, 8, 5, 6, 3};   

    //The neighbours of each symbol, ordered by function (a, b, c) ->
    //[symbol, function] -> new symbol
    static int[,] symbolNeighbours = {
        {1,3,2},
        {0,4,9},
        {4,6,0},
        {9,0,7},
        {2,1,5},
        {6,7,4},
        {5,2,8},
        {8,5,3},
        {7,9,6},
        {3,8,1}
    };

    GameObject[] functionButtons;

    void Start() {
        base.Start();

        startSymbol = StaticRandom.NextInt(10);
        currentSymbol = startSymbol;

        //symbolText = transform.Find("Text").gameObject;
        //symbolText.GetComponent<TMP_Text>().text = symbol[currentSymbol];

        mainIcon = transform.Find("Main Icon").gameObject;
        mainIcon.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", symbolTextures[currentSymbol]);

        functionButtons = new GameObject[3];

        functionButtons[0] = transform.Find("Top Button").gameObject;
        functionButtons[1] = transform.Find("Left Button").gameObject;
        functionButtons[2] = transform.Find("Right Button").gameObject;
    }

    void Update() {
        if (!moduleComplete) {
            if (Input.GetMouseButtonDown(0)) {
                for (int i = 0; i < 3; i++) {
                    if (IsMouseOver(functionButtons[i])) {
                        currentSymbol = symbolNeighbours[currentSymbol, i];
                        //symbolText.GetComponent<TMP_Text>().text = symbol[currentSymbol];
                        mainIcon.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", symbolTextures[currentSymbol]);
                    }
                }
            }

            if (currentSymbol == passSymbols[startSymbol]) {
                DeactivateModule();
            } else if (currentSymbol == failSymbols[startSymbol]) {
                currentSymbol = startSymbol;
                //symbolText.GetComponent<TMP_Text>().text = symbol[currentSymbol];
                mainIcon.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", symbolTextures[currentSymbol]);
                AddStrike();
            }
        }
    }
}