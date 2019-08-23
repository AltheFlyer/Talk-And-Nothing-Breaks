using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module: MonoBehaviour 
{

    public bool moduleComplete;
    public LevelGenerator bombSource;
    GameObject completionLED;
    GameObject completionLightSource;
    Color completionColor = new Color(0.1333333f, 0.5647059f, 0.07058824f, 0);

    public void Start() {
        moduleComplete = false;
        completionLED = transform.Find("CompletionLightbulb").Find("LED").gameObject;
        completionLightSource = transform.Find("CompletionLightSource").gameObject;
        //Turn off completion light
        completionLightSource.GetComponent<Light>().enabled = false;
    }


    public void DeactivateModule() {
        //Mark module as complete
        moduleComplete = true;

        //Turn on completion light
        completionLightSource.GetComponent<Light>().color = completionColor;
        completionLightSource.GetComponent<Light>().enabled = true;
        SetObjectColor(completionLED, "_Color", completionColor);
        SetObjectColor(completionLED, "_EmissionColor", completionColor);
        completionLED.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");

        //Deactivate Hover Glow on all children
        foreach (HoverGlow hg in GetComponentsInChildren<HoverGlow>()) {
            hg.enabled = false;
        }

        //Send message to main bomb
        bombSource.GetComponent<LevelGenerator>().CheckCompletion();
    }

    public bool IsMouseOver(GameObject obj) {
        return obj.GetComponent<MouseOverScript>().mouseOver;
    }

    public void SetObjectColor(GameObject obj, string type, Color c) {
        obj.GetComponent<Renderer>().material.SetColor(type, c);
    }
}