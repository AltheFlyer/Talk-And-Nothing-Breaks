using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class AdditionModuleScript: Module
{

    //Prefab for digits
    public GameObject digitPrefab;

    GameObject wire;
    GameObject lightSource;

    GameObject digitSet;
    GameObject submit;
    GameObject moduleBase;

    GameObject[] operationBits;
    bool[] operationState;

    public int answer;
    bool[] lights;

    public Color wireColor;
    public Color trueColor;
    public Color falseColor;
    public Color inertColor;

    IEnumerator blink;

    //Tester values, modify for balance, 8 was default try
    public int bitsPerNumber = 8;
    public int a,b;

    void Start() {
        base.Start();

        submit = transform.Find("Submit").gameObject;
        wire = transform.Find("Wire").gameObject;
        lightSource = transform.Find("LightSource").gameObject;

        digitSet = transform.Find("ToggleDigits").gameObject;
        moduleBase = transform.Find("Base").gameObject;

        //Lights initially off
        lightSource.GetComponent<Light>().enabled = false;

        operationBits = new GameObject[bitsPerNumber];
        operationState = new bool[bitsPerNumber];

        for (int i = 1; i < bitsPerNumber + 1; ++i) {
            //Create the prefab...
            GameObject go = Instantiate(digitPrefab);
            //Set the parent...
            go.transform.SetParent(digitSet.transform);
            //No really, SET THE DAMN PARENT
            go.transform.localPosition = new Vector3(0, 0, 0);
            //Use magic to move the bit to the right place, in English:
            //Take an arbitrary number, subtract half the bit width from it, then subtract for every bit already made
            go.transform.localPosition = new Vector3((0.64f - (0.64f/bitsPerNumber)) - (i - 1) * (1.24f / bitsPerNumber), 0, 0);
            //print (i.ToString() + " " + go.transform.position.ToString());
            //Scale it accordingly
            go.transform.localScale = new Vector3(1.24f / bitsPerNumber, 0.2f, 0.4f);
            //Give it a name
            go.name = "Digit " + i;

            //Prepare puzzle values
            operationBits[i - 1] = go;
            operationState[i - 1] = false;
            go.GetComponent<Renderer>().material.SetColor("_Color", falseColor);
            go.GetComponent<Renderer>().material.SetColor("_EmissionColor", falseColor);
        }
        
        //Answer generation, both numbers, and the answer are guaranteed to fit within the alloted bits per number
        a = StaticRandom.NextInt((int) (Math.Pow(2, bitsPerNumber)) - 1);
        b = StaticRandom.NextInt((int) (Math.Pow(2, bitsPerNumber)) - 1 - a);

        lights = new bool[bitsPerNumber * 2];
        IntToBoolArray(a, lights, 0);
        IntToBoolArray(b, lights, bitsPerNumber);

        answer = a + b;

        blink = BlinkLight();
        StartCoroutine(blink);
    }

    void Update() {
        if (!moduleComplete) {
            if (Input.GetMouseButtonDown(0)) {
                if (IsMouseOver(submit)) {
                    int sum = 0;
                    int powerOfTwo = 1;
                    for (int i = 0; i < bitsPerNumber; ++i) {
                        if (operationState[i]) {
                            sum += powerOfTwo;
                        }
                        powerOfTwo *= 2;
                    }
                    if (sum == answer) {
                        DeactivateModule();
                        
                        //(sum);
                    } else {
                        AddStrike("Binary Addition");
                        //print(sum);
                    }
                } else {
                    for (int i = 0; i < bitsPerNumber; ++i) {
                        if (IsMouseOver(operationBits[i])) {
                            operationState[i] = !operationState[i];
                            SetObjectColor(operationBits[i], "_Color", operationState[i] ? trueColor: falseColor);
                            SetObjectColor(operationBits[i], "_EmissionColor", operationState[i] ? trueColor: falseColor);
                        }
                    }
                } 

            }
        }
    }

    IEnumerator BlinkLight() {
        int currentLight = 0;
        //The long pause between light flashes
        bool paused = true;
        while (!moduleComplete) {
            if (paused) {
                yield return new WaitForSeconds(2);
                paused = false;
            } else if (currentLight < bitsPerNumber * 2) {
                SetObjectColor(wire, "_Color", lights[currentLight] ? trueColor : falseColor);
                SetObjectColor(wire, "_EmissionColor", lights[currentLight] ? trueColor : falseColor);
                wire.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                lightSource.GetComponent<Light>().color = lights[currentLight] ? trueColor : falseColor;
                lightSource.GetComponent<Light>().enabled = true;
                yield return new WaitForSeconds(0.5f);
                SetObjectColor(wire, "_Color", wireColor);
                wire.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                lightSource.GetComponent<Light>().enabled = false;
                yield return new WaitForSeconds(0.25f);
                currentLight++;
            } else {
                currentLight = 0;
                paused = true;
            }
        }
        StopCoroutine(blink);
    }

    /*
     * Converts an 8-bit integer into 8 booleans in an array starting from the specified index
     */
    void IntToBoolArray(int val, bool[] arr, int index) {
        int powerOfTwo = (int) Math.Pow(2, bitsPerNumber - 1);
        for (int i = 0; i < bitsPerNumber; ++i) {
            if (val >= powerOfTwo) {
                val -= powerOfTwo;
                arr[index + i] = true;
            } else {
                arr[index + i] = false;
            }
            powerOfTwo /= 2;
        }
    }
}