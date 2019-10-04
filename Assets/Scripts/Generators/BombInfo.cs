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
    public string name = "";
    public string description = "";
    ///modules gets converted into pools at runtime for backwards compat
    public List<ModuleInfo> modules = new List<ModuleInfo>();
    public List<PoolInfo> pools = new List<PoolInfo>();

    ///Converts modules into individual single module pools
    public void MergePools() {
        foreach (ModuleInfo m in modules) {
            PoolInfo next = new PoolInfo();
            //Create a module pool with only one module
            next.name = m.name;
            next.weight = m.weight;
            next.min = m.min;
            next.max = m.max;
            next.modules = new List<ModuleInfo>();
            next.modules.Add(m);
            pools.Add(next);
        }
    }

    public void Validate() {
        foreach (PoolInfo p in pools) {
            if (!p.Validate()) {
                pools.Remove(p);
            }
        }
    }
}