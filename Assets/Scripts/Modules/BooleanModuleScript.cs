using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BooleanModuleScript : Module
{

    GameObject leftLight;
    GameObject rightLight;
    GameObject leftLightLight;
    GameObject rightLightLight;
    GameObject operationPrompt;
    GameObject toggleLight;
    GameObject sumbitButton;
    GameObject moduleBase;

    public Color trueColor;
    public Color falseColor;
    public Color inertColor;

    public bool solution;
    public bool toggleState;

    int trialsLeft;
    public int operation;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        trialsLeft = 3;

        //Find child components
        leftLight = transform.Find("LeftLight").gameObject;
        rightLight = transform.Find("RightLight").gameObject;
        leftLightLight = transform.Find("LeftLightLight").gameObject;
        rightLightLight = transform.Find("RightLightLight").gameObject;
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
                    if (trialsLeft == 0) {
                        moduleComplete = true;
                        moduleBase.GetComponent<Renderer>().material.SetColor("_Color", trueColor);
                        bombSource.GetComponent<LevelGenerator>().CheckCompletion();
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

        leftLightLight.GetComponent<Light>().color = a ? trueColor : falseColor;
        rightLightLight.GetComponent<Light>().color = b ? trueColor : falseColor;

        if (operation == 0) {
             rightLight.GetComponent<Renderer>().material.SetColor("_Color", inertColor);
             rightLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", inertColor);
            rightLightLight.GetComponent<Light>().color = inertColor;
        }


        //Randomize the toggle light
        toggleState = StaticRandom.Next() < 0.5;
        toggleLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", toggleState ? trueColor : falseColor);
        toggleLight.GetComponent<Renderer>().material.SetColor("_Color", toggleState ? trueColor : falseColor);
    }
}
