using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombData : MonoBehaviour
{

    public List<GameObject> allModules;
    public List<GameObject> modules;
    public int width;
    public int height;
    public int numModules;

    // Start is called before the first frame update
    void Start()
    {
        modules = new List<GameObject>();  
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
    }

    public void SelectModule(string name) {
        if (name == "boolean") {
            modules.Add(allModules[0]);
        } else if (name == "addition") {
            modules.Add(allModules[1]);
        } else if (name == "bright") {
            modules.Add(allModules[2]);
        }
    }
}
