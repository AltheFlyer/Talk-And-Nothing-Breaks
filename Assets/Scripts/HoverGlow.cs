using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverGlow : MouseOverScript
{

    public Color startColor;
    public Color mouseOverColor;
    
    public Material startMaterial;
    public Material mouseOverMaterial;

    public void Start() {
        GetComponent<Renderer>().material.SetColor("_Color", startColor);
    }

    public void OnMouseEnter() {
        base.OnMouseEnter();
        GetComponent<Renderer>().material = mouseOverMaterial;
        GetComponent<Renderer>().material.SetColor("_Color", mouseOverColor);
    }

    public void OnMouseExit() {
        base.OnMouseExit();
        GetComponent<Renderer>().material = startMaterial;
        GetComponent<Renderer>().material.SetColor("_Color", startColor);
    }
    
}