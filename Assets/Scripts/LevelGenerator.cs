using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    
    //Prefabs
    public GameObject buttonModule;
    public GameObject blankModule;
    public GameObject booleanModule;
    public GameObject additionModule;
    public GameObject brightModule;

    //Generator settings
    public int width;
    public int height;
    public int numModules;

    //Strikes
    public int strikes;

    public Module[,] modules;

    // Start is called before the first frame update
    void Start()
    {
        GenerateBomb();
    }

    // Update is called once per frame
    void Update()
    {
        if (strikes >= 3) {
            SceneManager.LoadScene("GameMenuScene");
        }
    }

    public void CheckCompletion() {
        bool isComplete = true;
        for (int x = 0; x < width; ++x) {
            for (int z = 0; z < height * 2; ++z) {
                print("Z:" + z.ToString());
                if (!modules[x,z].moduleComplete) {
                    isComplete = false;
                }
            }
        }
        if (isComplete) {
            print("Hooray!");
            SceneManager.LoadScene("GameMenuScene");
        }
    }

    public void GenerateBomb() {
        strikes = 0;

        width = LevelData.width;
        height = LevelData.height;
        numModules = LevelData.numModules;

        GameObject tmp;
        System.Random rnd = new System.Random();
        //Generate a list of modules to make
        List<GameObject> genModules = new List<GameObject>();
        for (int i = 0; i < width * height * 2; ++i) {
            if (i < numModules) {
                //Insert some fancy heuristic some other day
                if (StaticRandom.Next() < 0.5) {
                    genModules.Add(brightModule);
                } else {
                    genModules.Add(booleanModule);
                }
            } else {
                //When number of modules is exhausted, add blanks until the list is full
                genModules.Add(blankModule);
            }
        }

        modules = new Module[width, height * 2];
        //Shuffle the list
        //WARNING: This needs to be changed if interdependent modules exist
        for (int i = 0; i < width * height * 2; ++i) {
            int j = rnd.Next(i, width * height * 2);
            tmp = genModules[i];
            genModules[i] = genModules[j];
            genModules[j] = tmp;
        }

        for (int x = 0; x < width; ++x) {
            for (int z = 0; z < height; ++z) {
                GameObject go = Instantiate(genModules[x + z * width], new Vector3(x * 2 - (width - 1), 0.5f, z * 2 - (height - 1)), Quaternion.identity);
                if (go.GetComponent<Module>() != null) {
                    go.GetComponent<Module>().bombSource = this;
                }
                modules[x, z] = go.GetComponent<Module>();
                go.transform.parent = this.transform;
            }
        }

        for (int x = 0; x < width; ++x) {
            for (int z = height; z < height * 2; ++z) {
                GameObject go = Instantiate(genModules[(width * height) + x + (z % height) * width], new Vector3(x * 2 - (width - 1), -0.5f, (z % height) * 2 - (height - 1)), Quaternion.Euler(0, 0, 180));
                if (go.GetComponent<Module>() != null) {
                    go.GetComponent<Module>().bombSource = this;
                }
                modules[x, z] = go.GetComponent<Module>();
                go.transform.parent = this.transform;
            }
        }
    }
}
