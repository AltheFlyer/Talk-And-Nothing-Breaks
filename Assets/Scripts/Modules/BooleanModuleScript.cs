using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BooleanModuleScript : Module
{

    GameObject leftLight;
    GameObject rightLight;
    GameObject leftLightSource;
    GameObject rightLightSource;

    GameObject operationPrompt;
    GameObject toggleLight;
    GameObject submitButton;
    GameObject moduleBase;
    GameObject[] rounds;

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
        leftLightSource = transform.Find("LeftLightSource").gameObject;
        rightLightSource = transform.Find("RightLightSource").gameObject;
        
        operationPrompt = transform.Find("Prompt").gameObject;
        toggleLight = transform.Find("OutputLight").gameObject;
        submitButton = transform.Find("Submit").gameObject;
        moduleBase = transform.Find("Base").gameObject;
        rounds = new GameObject[TOTAL_TRIALS];
        for (int i = 0; i < TOTAL_TRIALS; ++i) {
            rounds[i] = transform.Find("RoundCounter").Find("Round" + (i + 1)).gameObject;
        }

        //Randomize the toggle light
        toggleState = StaticRandom.Next() < 0.5;
        SetObjectColor(toggleLight, "_EmissionColor", toggleState ? trueColor : falseColor);
        SetObjectColor(toggleLight, "_Color", toggleState ? trueColor : falseColor);

        GenerateTrial();
    }

    // Update is called once per frame
    void Update()
    {
        if (!moduleComplete && Input.GetMouseButtonDown(0)) {
            if (IsMouseOver(toggleLight)) {
                toggleState = !toggleState;
                SetObjectColor(toggleLight, "_EmissionColor", toggleState ? trueColor : falseColor);
                SetObjectColor(toggleLight, "_Color", toggleState ? trueColor : falseColor);
            }
            if (IsMouseOver(submitButton)) {
                if (toggleState == solution) {
                    trialsLeft--;
                    int currentTrial = TOTAL_TRIALS - trialsLeft - 1;
                    SetObjectColor(rounds[currentTrial], "_EmissionColor", trueColor);
                    rounds[currentTrial].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                    if (trialsLeft == 0) {
                        DeactivateModule();
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
        SetObjectColor(leftLight, "_EmissionColor", a ? trueColor : falseColor);
        SetObjectColor(rightLight, "_EmissionColor", b ? trueColor : falseColor);

        SetObjectColor(leftLight, "_Color", a ? trueColor : falseColor);
        SetObjectColor(rightLight, "_Color", b ? trueColor : falseColor);

        leftLightSource.GetComponent<Light>().color = a ? trueColor : falseColor;
        rightLightSource.GetComponent<Light>().color = b ? trueColor : falseColor;

        if (operation == 0) {
            SetObjectColor(rightLight, "_Color", inertColor);
            SetObjectColor(rightLight, "_EmissionColor", inertColor);
            rightLightSource.GetComponent<Light>().color = inertColor;
        }
    }
}
