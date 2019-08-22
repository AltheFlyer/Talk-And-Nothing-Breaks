using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverGlow : MouseOverScript
{
    
    public Material startMaterial;
    public Material mouseOverMaterial;

    public void Start() {
    }

    public void OnMouseEnter() {
        if (enabled) {
        base.OnMouseEnter();
        Color c = GetComponent<Renderer>().material.GetColor("_Color");
        Color ec = GetComponent<Renderer>().material.GetColor("_EmissionColor");
        GetComponent<Renderer>().material = mouseOverMaterial;
        GetComponent<Renderer>().material.SetColor("_Color", c);
        GetComponent<Renderer>().material.SetColor("_EmissionColor", ec);
        }
    }

    public void OnMouseExit() {
        base.OnMouseExit();
        Color c = GetComponent<Renderer>().material.GetColor("_Color");
        Color ec = GetComponent<Renderer>().material.GetColor("_EmissionColor");
        GetComponent<Renderer>().material = startMaterial;
        GetComponent<Renderer>().material.SetColor("_Color", c);
        GetComponent<Renderer>().material.SetColor("_EmissionColor", ec);
    }
    
}