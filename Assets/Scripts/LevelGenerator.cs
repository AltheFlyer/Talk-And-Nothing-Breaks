using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelGenerator : MonoBehaviour
{
    
    //Prefabs
    public GameObject timerModule;
    public GameObject blankModule;
    public GameObject booleanModule;
    public GameObject additionModule;
    public GameObject brightModule;

    [SerializeField]
    public List<GameObject> prefabModules;

    //Generator settings
    public int width;
    public int height;
    public int numModules;

    //Strikes
    public int strikes;

    public Module[,] modules;

    //Bomb data
    public int id;
    public string idAsBinary;
    GameObject idTag;

    public string serialCode;
    public int serialLength = 8;

    public bool serialContainsVowel;
    GameObject serialTag;

    GameObject loadedTimerModule;

    // Start is called before the first frame update
    void Start()
    {
        GenerateBomb();
        GeneratorCodes();        
    }

    public void CheckCompletion() {
        bool isComplete = true;
        for (int x = 0; x < width; ++x) {
            for (int z = 0; z < height * 2; ++z) {
                //print("Z:" + z.ToString());
                print(modules[x, z]);
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

        int fullWeight = 0;
        BombData data = null;

        if (GameObject.Find("BombData")) {
            data = GameObject.Find("BombData").GetComponent<BombData>();
        }
        if (data) {
            width = data.width;
            height = data.height;
            numModules = data.numModules;
            prefabModules = data.modules;
            data.Consume();
            for (int i = 0; i < prefabModules.Count; i++) {
                fullWeight += data.weights[i];
            }
        } else {
            width = LevelData.width;
            height = LevelData.height;
            numModules = LevelData.numModules;
            for (int i = 0; i < prefabModules.Count; i++) {
                fullWeight += prefabModules[i].GetComponent<Module>().spawnWeight;
            }
        }

        GameObject tmp;
        System.Random rnd = new System.Random();

        //Generate a list of modules to make
        List<GameObject> genModules = new List<GameObject>();
        genModules.Add(timerModule);

        for (int i = 0; i < width * height * 2 - 1; ++i) {
            if (i < numModules) {
                double rand = StaticRandom.NextInt(fullWeight);
                //Insert some fancy heuristic some other day
                int counter = fullWeight;
                //print("A");
                for (int j = prefabModules.Count - 1; j >= 0; j--) {
                    //TODO I REALLY want to remove this check, I'll do it after debug is over when BombData is required
                    if (data) {
                        if (rand >= counter - data.weights[j]) {
                            genModules.Add(prefabModules[j]);
                            break;
                        } else {
                            counter -= data.weights[j];
                        }
                    } else {
                        if (rand >= counter - prefabModules[j].GetComponent<Module>().spawnWeight) {
                            genModules.Add(prefabModules[j]);
                            break;
                        } else {
                            counter -= prefabModules[j].GetComponent<Module>().spawnWeight;
                        }
                    }
                }
                //print("B");
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
                if (go.GetComponent<BombTimerScript>()) {
                    loadedTimerModule = go;
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
                if (go.GetComponent<BombTimerScript>()) {
                    loadedTimerModule = go;
                }
                modules[x, z] = go.GetComponent<Module>();
                go.transform.parent = this.transform;
            }
        }

        if (data) {
            loadedTimerModule.GetComponent<BombTimerScript>().secondsLeft = data.time;
        } else {
            loadedTimerModule.GetComponent<BombTimerScript>().secondsLeft = 300;
        }
    }

    public void GeneratorCodes() {
        string codeLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string vowels = "AEIOU";
        string codeNumbers = "0123456789";

        id = StaticRandom.NextInt(256);
        idAsBinary = "";
        int tmpId = id;
        for (int power = 128; power > 0; power /= 2) {
            if (tmpId >= power) {
                idAsBinary += "1";
                tmpId -= power;
            } else {
                idAsBinary += "0";
            }
        }

        GameObject idPlate = transform.Find("ID Plate").gameObject;
        idPlate.transform.localPosition = new Vector3(modules[0, 0].transform.position.x - 1.1f, modules[0, 0].transform.position.y, modules[0, 0].transform.position.z);


        idTag = idPlate.transform.Find("ID").gameObject;
        idTag.GetComponent<TMP_Text>().text = "ID: " + idAsBinary.ToString().Substring(0, 4) + " " + idAsBinary.ToString().Substring(4, 4);
        //idTag.transform.localPosition = new Vector3(modules[0, 0].transform.position.x - 1.01f, modules[0, 0].transform.position.y, modules[0, 0].transform.position.z);

        //Serial code generation
        serialContainsVowel = false;
        serialCode = "";
        for (int i = 0; i < serialLength; ++i) {
            if (StaticRandom.Next() < 0.5f) {
                serialCode += codeLetters.Substring(StaticRandom.NextInt(26), 1);
                if (vowels.Contains(serialCode.Substring(i, 1))) {
                    serialContainsVowel = true;
                }
            } else {
                serialCode += codeNumbers.Substring(StaticRandom.NextInt(10), 1);
            }
        }
        serialTag = idPlate.transform.Find("Serial Code").gameObject;
        serialTag.GetComponent<TMP_Text>().text = serialCode;
        //serialTag.transform.localPosition = new Vector3(modules[0, 0].transform.position.x - 1.01f, modules[0, 0].transform.position.y - 0.5f, modules[0, 0].transform.position.z);
    }

    public void AddStrike() {
        strikes++;
        loadedTimerModule.GetComponent<BombTimerScript>().AddStrike(strikes);
        if (strikes >= 3) {
            SceneManager.LoadScene("GameMenuScene");
        }
    }
}
