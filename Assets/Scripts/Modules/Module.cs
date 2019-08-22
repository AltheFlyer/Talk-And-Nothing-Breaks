using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module: MonoBehaviour 
{

    public bool moduleComplete;
    public LevelGenerator bombSource;
    public GameObject completionLED;
    public GameObject completionLightSource;
    public Color completionColor = new Color(0.1333333f, 0.5647059f, 0.07058824f, 0);

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
        completionLED.GetComponent<Renderer>().material.SetColor("_Color", completionColor);
        completionLED.GetComponent<Renderer>().material.SetColor("_EmissionColor", completionColor);
        completionLED.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");

        //Deactivate Hover Glow on all children
        foreach (HoverGlow hg in GetComponentsInChildren<HoverGlow>()) {
            hg.enabled = false;
        }

        //Send message to main bomb
        bombSource.GetComponent<LevelGenerator>().CheckCompletion();
    }
}