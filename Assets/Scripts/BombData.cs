using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombData : MonoBehaviour
{

    public List<GameObject> allModules;
    public List<GameObject> modules;
    public List<int> weights;
    public int width;
    public int height;
    public int numModules;
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        modules = new List<GameObject>();
        weights = new List<int>();
        time = 600;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use() {
        DontDestroyOnLoad(this);  
    }

    public void Consume() {
        Destroy(this.gameObject);
    }

    public void clearData() {
        width = 3;
        height = 2;
        numModules = 0;
        modules = new List<GameObject>();
        weights = new List<int>();
        time = 600;
    }

    public void SelectModule(string name) {
        GameObject selection = allModules[0];
        if (name == "boolean") {
            selection = allModules[0];
        } else if (name == "addition") {
            selection = allModules[1];
        } else if (name == "bright") {
            selection = allModules[2];
        } else if (name == "alchemy") {
            selection = allModules[3];
        }

        modules.Add(selection);
        weights.Add(selection.GetComponent<Module>().spawnWeight);
    }

    public void SelectModule(string name, int weight) {
        GameObject selection = allModules[0];
        if (name == "boolean") {
            selection = allModules[0];
        } else if (name == "addition") {
            selection = allModules[1];
        } else if (name == "bright") {
            selection = allModules[2];
        } else if (name == "alchemy") {
            selection = allModules[3];
        }

        modules.Add(selection);
        weights.Add(weight);
    }
}
