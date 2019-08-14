using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverGlow : MonoBehaviour 
{

    public Color startColor;
    public Color mouseOverColor;
    bool mouseOver = false;

    void Start() {
        GetComponent<Renderer>().material.SetColor("_Color", startColor);
    }

    void OnMouseEnter() {
        mouseOver = true;
        GetComponent<Renderer>().material.SetColor("_Color", mouseOverColor);
    }

    void OnMouseExit() {
        mouseOver = false;
        GetComponent<Renderer>().material.SetColor("_Color", startColor);
    }
    
}