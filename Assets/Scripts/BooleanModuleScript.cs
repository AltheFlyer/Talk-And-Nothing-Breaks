using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BooleanModuleScript : Module
{

    GameObject leftLight;
    GameObject rightLight;
    GameObject operationPrompt;
    GameObject toggleLight;
    GameObject sumbitButton;
    GameObject moduleBase;

    public Color trueColor;
    public Color falseColor;

    bool solution;
    bool toggleState;

    int trialsLeft;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        trialsLeft = 3;

        //Find child components
        leftLight = transform.Find("LeftLight").gameObject;
        rightLight = transform.Find("RightLight").gameObject;
        operationPrompt = transform.Find("Prompt").gameObject;
        toggleLight = transform.Find("OutputLight").gameObject;
        sumbitButton = transform.Find("Submit").gameObject;
        moduleBase = transform.Find("Base").gameObject;

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
            if (sumbitButton.GetComponent<MouseOverScript>().mouseOver && Input.GetMouseButtonDown(0)) {
                if (toggleState == solution) {
                    trialsLeft--;
                    GenerateTrial();
                    if (trialsLeft == 0) {
                        moduleComplete = true;
                        moduleBase.GetComponent<Renderer>().material.SetColor("_Color", trueColor);
                    }
                } else {
                    bombSource.strikes++;
                }
            }
        }
    }

    void GenerateTrial() {
        bool a, b;

        int operation;

        //Puzzle and answer are generated
        operation = StaticRandom.NextInt(3);

        a = StaticRandom.Next() < 0.5;
        b = StaticRandom.Next() < 0.5;

        //Choose operation
        if (operation == 0) {
            solution = !a;
            operationPrompt.GetComponent<TextMesh>().text = "!";
        } else if (operation == 1) {
            solution = a && b;
            operationPrompt.GetComponent<TextMesh>().text = "&&";
        } else if (operation == 2) {
            solution = a || b;
            operationPrompt.GetComponent<TextMesh>().text = "||";
        }

        //Light up components according to puzzle
        leftLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", a ? trueColor : falseColor);
        rightLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", b ? trueColor : falseColor);
        
        leftLight.GetComponent<Renderer>().material.SetColor("_Color", a ? trueColor : falseColor);
        rightLight.GetComponent<Renderer>().material.SetColor("_Color", b ? trueColor : falseColor);

        //Randomize the toggle light
        toggleState = StaticRandom.Next() < 0.5;
        toggleLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", toggleState ? trueColor : falseColor);
        toggleLight.GetComponent<Renderer>().material.SetColor("_Color", toggleState ? trueColor : falseColor);
    }
}
