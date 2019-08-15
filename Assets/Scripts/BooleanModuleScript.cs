using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BooleanModuleScript : MonoBehaviour
{

    public GameObject leftLight;
    public GameObject rightLight;
    public GameObject operationPrompt;
    public GameObject toggleLight;
    public GameObject sumbitButton;
    public GameObject moduleBase;

    public Color trueColor;
    public Color falseColor;

    public bool solution;

    public bool toggleState;

    public bool moduleActive;

    // Start is called before the first frame update
    void Start()
    {
        moduleActive = true;

        //Find child components
        leftLight = transform.Find("LeftLight").gameObject;
        rightLight = transform.Find("RightLight").gameObject;
        operationPrompt = transform.Find("Prompt").gameObject;
        toggleLight = transform.Find("OutputLight").gameObject;
        sumbitButton = transform.Find("Submit").gameObject;
        moduleBase = transform.Find("Base").gameObject;

        bool a, b;

        int operation;

        //Puzzle and answer are generated
        operation = StaticRandom.NextInt(3);

        a = StaticRandom.Next() < 0.5;
        b = StaticRandom.Next() < 0.5;

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

        leftLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", a ? trueColor : falseColor);
        rightLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", b ? trueColor : falseColor);
        
        leftLight.GetComponent<Renderer>().material.SetColor("_Color", a ? trueColor : falseColor);
        rightLight.GetComponent<Renderer>().material.SetColor("_Color", b ? trueColor : falseColor);

        toggleState = StaticRandom.Next() < 0.5;
        toggleLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", toggleState ? trueColor : falseColor);
        toggleLight.GetComponent<Renderer>().material.SetColor("_Color", toggleState ? trueColor : falseColor);

    }

    // Update is called once per frame
    void Update()
    {
        if (moduleActive) {
            if (toggleLight.GetComponent<MouseOverScript>().mouseOver && Input.GetMouseButtonDown(0)) {
                toggleState = !toggleState;
                toggleLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", toggleState ? trueColor : falseColor);
                toggleLight.GetComponent<Renderer>().material.SetColor("_Color", toggleState ? trueColor : falseColor);
            }
            if (sumbitButton.GetComponent<MouseOverScript>().mouseOver && Input.GetMouseButtonDown(0)) {
                moduleActive = false;
                moduleBase.GetComponent<Renderer>().material.SetColor("_Color", (toggleState == solution) ? trueColor : falseColor);
            }
        }
    }
}
