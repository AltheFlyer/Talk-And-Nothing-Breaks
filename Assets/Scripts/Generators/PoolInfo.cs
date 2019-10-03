using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PoolInfo {
    public string name = "";
    public int weight = 1;
    public int min = 0;
    public int max = -1;
    public string generationType = "random";
    public List<ModuleInfo> modules;

    ///Checks if this pool can generate modules or not
    public bool Validate() {
        return modules.Count > 0;
    }

    public string Generate() {
        int fullWeight = 0;
        foreach (ModuleInfo mInfo in modules) {
            fullWeight += mInfo.weight;
        }

        Debug.Log("In Generate Function");
        int rand = StaticRandom.NextInt(fullWeight);
        Debug.Log("Outside Generate Function");

        int weightCounter = fullWeight;

        for (int j = modules.Count - 1; j >= 0; j--) {
            if (rand >= weightCounter - modules[j].weight) {
                string moduleName = modules[j].name;
                modules[j].max--;
                if (modules[j].max == 0) {
                    modules.RemoveAt(j);
                } 
                return moduleName;
            }
            weightCounter -= modules[j].weight;
        }
        //This WILL crash
        return "";
    }
}