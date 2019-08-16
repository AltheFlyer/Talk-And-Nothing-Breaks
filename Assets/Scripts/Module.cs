using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module: MonoBehaviour 
{

    public bool moduleComplete;
    public LevelGenerator bombSource;

    public void Start() {
        moduleComplete = false;
    }
}