using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BombInfo {
    public int width = 3;
    public int height = 2;
    public int time = 600;
    public int numModules = 1;
    public string generationType = "random";
    public List<ModuleInfo> modules;
}