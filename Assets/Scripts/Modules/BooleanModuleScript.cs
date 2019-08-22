﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BooleanModuleScript : Module
{

    GameObject leftLight;
    GameObject rightLight;
    GameObject completionLED;
    GameObject leftLightSource;
    GameObject rightLightSource;
    GameObject completionLightSource;
    GameObject operationPrompt;
    GameObject toggleLight;
    GameObject submitButton;
    GameObject moduleBase;
    GameObject[] rounds;

    public Color completionColor;
    public Color trueColor;
    public Color falseColor;
    public Color inertColor;

    public bool solution;
    public bool toggleState;

    readonly int TOTAL_TRIALS = 3;
    int trialsLeft;
    public int operation;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        trialsLeft = TOTAL_TRIALS;

        //Find child components
        leftLight = transform.Find("LeftLight").gameObject;
        rightLight = transform.Find("RightLight").gameObject;
        completionLED = transform.Find("CompletionLightbulb").Find("LED").gameObject;
        leftLightSource = transform.Find("LeftLightSource").gameObject;
        rightLightSource = transform.Find("RightLightSource").gameObject;
        completionLightSource = transform.Find("CompletionLightSource").gameObject;
        operationPrompt = transform.Find("Prompt").gameObject;
        toggleLight = transform.Find("OutputLight").gameObject;
        submitButton = transform.Find("Submit").gameObject;
        moduleBase = transform.Find("Base").gameObject;
        rounds = new GameObject[TOTAL_TRIALS];
        for (int i = 0; i < TOTAL_TRIALS; ++i) {
            rounds[i] = transform.Find("RoundCounter").Find("Round" + (i + 1)).gameObject;
        }

        //Turn off completion light
        completionLightSource.GetComponent<Light>().enabled = false;

        //Randomize the toggle light
        toggleState = StaticRandom.Next() < 0.5;
        toggleLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", toggleState ? trueColor : falseColor);
        toggleLight.GetComponent<Renderer>().material.SetColor("_Color", toggleState ? trueColor : falseColor);

        GenerateTrial();
    }

    // Update is called once per frame
    void Update()
    {
        if (!moduleComplete) {
            if (toggleLight.GetComponent<MouseOverScript>().mouseOver && Input.GetMouseButtonDown(0)) {
                toggleState = !toggleState;
                toggleLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", toggleState ? trueColor : falseColor);
                toggleLight.GetComponent<Renderer>().material.SetColor("_Color", toggleState ? trueColor : falseColor);
            }
            if (submitButton.GetComponent<MouseOverScript>().mouseOver && Input.GetMouseButtonDown(0)) {
                if (toggleState == solution) {
                    trialsLeft--;
                    int currentTrial = TOTAL_TRIALS - trialsLeft - 1;
                    rounds[currentTrial].GetComponent<Renderer>().material.SetColor("_EmissionColor", trueColor);
                    rounds[currentTrial].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                    if (trialsLeft == 0) {
                        moduleComplete = true;
                        completionLightSource.GetComponent<Light>().color = completionColor;
                        completionLightSource.GetComponent<Light>().enabled = true;
                        completionLED.GetComponent<Renderer>().material.SetColor("_Color", completionColor);
                        completionLED.GetComponent<Renderer>().material.SetColor("_EmissionColor", completionColor);
                        completionLED.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                        bombSource.GetComponent<LevelGenerator>().CheckCompletion();

                        //These not working
                        submitButton.GetComponent<HoverGlow>().enabled = false;
                        toggleLight.GetComponent<HoverGlow>().enabled = false;
                    } else {
                        GenerateTrial();
                    }
                } else {
                    bombSource.strikes++;
                }
            }
        }
    }

    void GenerateTrial() {
        bool a, b;

        //int operation;

        //Puzzle and answer are generated
        //The first trial will never have the not combined operators (NOR, NAND, XNOR)
        if (trialsLeft == 3) {
            operation = StaticRandom.NextInt(4);
        } else {
            operation = StaticRandom.NextInt(7);
        }
        //Operations:
        //NOT, AND, OR, XOR, NAND, NOR, XNOR

        a = StaticRandom.Next() < 0.5;
        b = StaticRandom.Next() < 0.5;

        //Choose operation
        if (operation == 0) {
            solution = !a;
            operationPrompt.GetComponent<TMP_Text>().text = "!";
        } else if (operation == 1) {
            solution = a && b;
            operationPrompt.GetComponent<TMP_Text>().text = "&&";
        } else if (operation == 2) {
            solution = a || b;
            operationPrompt.GetComponent<TMP_Text>().text = "||";
        } else if (operation == 3) {
            solution = a ^ b;
            operationPrompt.GetComponent<TMP_Text>().text = "^^";
        } else if (operation == 4) {
            solution = !(a && b);
            operationPrompt.GetComponent<TMP_Text>().text = "!&";
        } else if (operation == 5) {
            solution = !(a || b);
            operationPrompt.GetComponent<TMP_Text>().text = "!|";
        } else if (operation == 6) {
            solution = (a == b);
            operationPrompt.GetComponent<TMP_Text>().text = "!^";
        }

        //Light up components according to puzzle
        leftLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", a ? trueColor : falseColor);
        rightLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", b ? trueColor : falseColor);

        leftLight.GetComponent<Renderer>().material.SetColor("_Color", a ? trueColor : falseColor);
        rightLight.GetComponent<Renderer>().material.SetColor("_Color", b ? trueColor : falseColor);

        leftLightSource.GetComponent<Light>().color = a ? trueColor : falseColor;
        rightLightSource.GetComponent<Light>().color = b ? trueColor : falseColor;

        if (operation == 0) {
             rightLight.GetComponent<Renderer>().material.SetColor("_Color", inertColor);
             rightLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", inertColor);
            rightLightSource.GetComponent<Light>().color = inertColor;
        }
    }
}
