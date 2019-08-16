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
        base.OnMouseEnter();
        Color c = GetComponent<Renderer>().material.GetColor("_Color");
        GetComponent<Renderer>().material = mouseOverMaterial;
        GetComponent<Renderer>().material.SetColor("_Color", c);
    }

    public void OnMouseExit() {
        base.OnMouseExit();
        Color c = GetComponent<Renderer>().material.GetColor("_Color");
        GetComponent<Renderer>().material = startMaterial;
        GetComponent<Renderer>().material.SetColor("_Color", c);
    }
    
}