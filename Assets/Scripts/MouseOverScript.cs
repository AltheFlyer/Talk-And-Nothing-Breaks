using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverScript : MonoBehaviour
{
    public bool mouseOver = false;

    public void Start() {
        
    }

    public void OnMouseEnter() {
        mouseOver = true;
    }

    public void OnMouseExit() {
        mouseOver = false;
    }
} 