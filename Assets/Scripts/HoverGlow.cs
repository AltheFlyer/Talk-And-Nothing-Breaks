using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverGlow : MonoBehaviour 
{

    public Color startColor;
    public Color mouseOverColor;
    bool mouseOver = false;
    public Material startMaterial;
    public Material mouseOverMaterial;

    void Start() {
        GetComponent<Renderer>().material.SetColor("_Color", startColor);
    }

    void OnMouseEnter() {
        mouseOver = true;
        GetComponent<Renderer>().material = mouseOverMaterial;
        GetComponent<Renderer>().material.SetColor("_Color", mouseOverColor);
    }

    void OnMouseExit() {
        mouseOver = false;
        GetComponent<Renderer>().material = startMaterial;
        GetComponent<Renderer>().material.SetColor("_Color", startColor);
    }
    
}