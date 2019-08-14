using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverGlow : MonoBehaviour 
{

    public Color startColor;
    public Color mouseOverColor;
    public bool mouseOver = false;
    public Material startMaterial;
    public Material mouseOverMaterial;

    public void Start() {
        GetComponent<Renderer>().material.SetColor("_Color", startColor);
    }

    public void OnMouseEnter() {
        mouseOver = true;
        GetComponent<Renderer>().material = mouseOverMaterial;
        GetComponent<Renderer>().material.SetColor("_Color", mouseOverColor);
    }

    public void OnMouseExit() {
        mouseOver = false;
        GetComponent<Renderer>().material = startMaterial;
        GetComponent<Renderer>().material.SetColor("_Color", startColor);
    }
    
}