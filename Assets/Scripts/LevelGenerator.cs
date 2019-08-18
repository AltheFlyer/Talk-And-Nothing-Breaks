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

    //Generator settings
    public int width;
    public int height;
    public int numModules;

    //Strikes
    public int strikes;

    public Module[,] modules;


    //Rotation animation


    // Start is called before the first frame update
    void Start()
    {
        strikes = 0;

        GameObject tmp;
        System.Random rnd = new System.Random();
        //Generate a list of modules to make
        List<GameObject> genModules = new List<GameObject>();
        for (int i = 0; i < width * height; ++i) {
            if (i < numModules) {
                //Insert some fancy heuristic some other day
                genModules.Add(booleanModule);
            } else {
                //When number of modules is exhausted, add blanks until the list is full
                genModules.Add(blankModule);
            }
        }


        modules = new Module[width, height];
        //Shuffle the list
        //WARNING: This needs to be changed if interdependent modules exist
        for (int i = 0; i < width * height; ++i) {
            int j = rnd.Next(i, width * height);
            tmp = genModules[i];
            genModules[i] = genModules[j];
            genModules[j] = tmp;
        }

        for (int x = 0; x < width; ++x) {
            for (int z = 0; z < height; ++z) {
                GameObject go = Instantiate(genModules[x + z * width], new Vector3(x * 2 - (width - 1), 1, z * 2 - (height - 1)), Quaternion.identity);
                if (go.GetComponent<Module>() != null) {
                    go.GetComponent<Module>().bombSource = this;
                }
                modules[x, z] = go.GetComponent<Module>();
                go.transform.parent = this.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (strikes >= 3) {
            //Something about losing
        }
        
        if (Input.GetKey(KeyCode.Q)) {
            transform.Rotate(90 * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.E)) {
            transform.Rotate(-90 * Time.deltaTime, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            transform.Rotate(0, 0, 180);
        }

        if (Input.GetMouseButton(1)) {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");
            transform.Rotate(3 * v, 0, 3 * -h, Space.Self);
            Quaternion q = transform.rotation;
            transform.rotation = Quaternion.Euler(q.eulerAngles.x, 0, q.eulerAngles.z);
        }

    }

    public void CheckCompletion() {
        bool isComplete = true;
        for (int x = 0; x < width; ++x) {
            for (int z = 0; z < height; ++z) {
                if (!modules[x,z].moduleComplete) {
                    isComplete = false;
                }
            }
        }
        if (isComplete) {
            print("Hooray!");
        }
    }
}
