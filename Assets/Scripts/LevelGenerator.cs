using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    public GameObject buttonModule;
    public GameObject blankModule;
    public int width;
    public int height;
    public int numModules;

    // Start is called before the first frame update
    void Start()
    {
        GameObject tmp;
        System.Random rnd = new System.Random();
        //Generate a list of modules to make
        List<GameObject> modules = new List<GameObject>();
        for (int i = 0; i < width * height; ++i) {
            if (i < numModules) {
                //Insert some fancy heuristic some other day
                modules.Add(buttonModule);
            } else {
                //When number of modules is exhausted, add blanks until the list is full
                modules.Add(blankModule);
            }
        }

        //Shuffle the list
        //WARNING: This needs to be changed if interdependent modules exist
        for (int i = 0; i < width * height; ++i) {
            int j = rnd.Next(i, width * height);
            tmp = modules[i];
            modules[i] = modules[j];
            modules[j] = tmp;
        }

        for (int x = 0; x < width; ++x) {
            for (int z = 0; z < height; ++z) {
                Instantiate(modules[x + z * width], new Vector3(x * 2 - (width - 1), 1, z * 2 - (height - 1)), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
