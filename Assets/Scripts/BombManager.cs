using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BombManager : MonoBehaviour
{
    
    //Prefabs
    public GameObject timerModule;
    public GameObject blankModule;

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

    //HARDCODING module holder generation
    private float moduleHolderWidth = 2.4f;
    private float edgeConstant = 0.2f;
    
    [SerializeField]
    GameObject moduleHolder;
    [SerializeField]
    GameObject edgeModuleHolder;
    [SerializeField]
    GameObject cornerModuleHolder;

    // Start is called before the first frame update
    void Start()
    {
        GenerateBomb();
        GenerateCodes();        
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
            PlayerData.time = loadedTimerModule.GetComponent<BombTimerScript>().secondsLeft;
            PlayerData.totalScore += PlayerData.currentScore;
            PlayerData.win = true;
            SceneManager.LoadScene("GameOverScene");
        }
    }

    public void GenerateBomb() {
        strikes = 0;

        int fullWeight = 0;
        BombData bombData = null;
        //This try catch block is only necessary while in production
        //So the game can still run from the level generation scene
        try {
            bombData = GameObject.Find("BombData").GetComponent<BombData>();
        } catch {
            UnityEngine.Object o = Resources.Load("BombData");
            print(o);
            GameObject oo = GameObject.Instantiate(o) as GameObject;
            bombData = oo.GetComponent<BombData>();
            //I shouldnt need this but screw me I guess
            bombData.SetData("Assets/Generators/default.json");
            bombData.Start();
        }
        BombInfo generator = bombData.meta;
        //print(JsonUtility.ToJson(generator));
        width = generator.width;
        height = generator.height;
        numModules = generator.numModules;

        //Merge modules and pools in generate
        generator.MergePools();
        print(JsonUtility.ToJson(generator));

        //Count number of modules generated pre-randomizer
        int generatedModules = 0;

        //Generate a list of modules to make
        List<GameObject> genModules = new List<GameObject>();
        //Always include timer module!
        genModules.Add(timerModule);


        //Add weights to get a maximum weight value for random generation
        for (int i = 0; i < generator.pools.Count; i++) {
            fullWeight += generator.pools[i].weight;
        }

        //Ensure minimum module generation is done
        foreach (PoolInfo pInfo in generator.pools) {
            List<string> poolSpecificModules = pInfo.GetMinModules();
            foreach (string moduleName in poolSpecificModules) {
                genModules.Add(bombData.allModules[moduleName]);
                generatedModules++;
            }
            for (int i = 0; i < pInfo.min; i++) {
                genModules.Add(bombData.allModules[pInfo.Generate()]);
                generatedModules++;
            }
        }

        //Number of modules = width * height, front and back, minus 1 for timer module
        for (int i = generatedModules; i < width * height * 2 - 1; ++i) {
            if (i < numModules && generator.pools.Count > 0) {
                int rand = StaticRandom.NextInt(fullWeight);
                //Insert some fancy heuristic some other day
                int weightCounter = fullWeight;
                
                //We can guarantee a certain module order from the fullWeight generation
                for (int j = generator.pools.Count - 1; j >= 0; j--) {
                    if (rand >= weightCounter - generator.pools[j].weight) {
                        string s = generator.pools[j].Generate();
                        //print(s);
                        genModules.Add(bombData.allModules[s]);
                        //Dirty maximum check
                        //Note how this ignores maximum if <= 0 on startup
                        //This IS intended
                        if (!generator.pools[j].Validate() || generator.pools[j].max == 0) {
                            print("REMOVED A POOL");
                            generator.pools.RemoveAt(j);
                            //Regenrate the fullWeight value
                            fullWeight = 0;
                            for (int k = 0; k < generator.pools.Count; k++) {
                                fullWeight += generator.pools[k].weight;
                            }
                        }
                        break;
                    } else {
                        weightCounter -= generator.pools[j].weight;
                    }
                } 
            } else {
                //When number of modules is exhausted, add blanks until the list is full
                genModules.Add(blankModule);
            }
        }

        //Shuffle the list
        //WARNING: This needs to be changed if interdependent modules exist
        //Used for shuffling modules
        Debug.Log("Shuffling");
        GameObject tmp;
        for (int i = 0; i < width * height * 2; ++i) {
            int j = StaticRandom.NextInt(i, width * height * 2);
            tmp = genModules[i];
            genModules[i] = genModules[j];
            genModules[j] = tmp;
        }
        

        //Now we actually fill the bomb with modules
        modules = new Module[width, height * 2];
        for (int x = 0; x < width; ++x) {
            for (int z = 0; z < height; ++z) {
                GameObject model = Instantiate(moduleHolder, new Vector3(edgeConstant + x * moduleHolderWidth - ((moduleHolderWidth / 2) * width - 1), 0.5f, edgeConstant + z * moduleHolderWidth - ((moduleHolderWidth / 2) * height - 1)), Quaternion.Euler(-90, 0, 0));
                GameObject go = Instantiate(genModules[x + z * width], new Vector3(edgeConstant + x * moduleHolderWidth - ((moduleHolderWidth / 2) * width - 1), 0.5f, edgeConstant + z * moduleHolderWidth - ((moduleHolderWidth / 2) * height - 1)), Quaternion.identity);
                if (go.GetComponent<Module>() != null) {
                    go.GetComponent<Module>().bombSource = this;
                }
                if (go.GetComponent<BombTimerScript>()) {
                    loadedTimerModule = go;
                }
                modules[x, z] = go.GetComponent<Module>();

                model.transform.parent = this.transform;
                go.transform.parent = this.transform;
            }
        }

        for (int x = 0; x < width; ++x) {
            for (int z = height; z < height * 2; ++z) {
                GameObject model = Instantiate(moduleHolder, new Vector3(edgeConstant + x * moduleHolderWidth - ((moduleHolderWidth / 2) * width - 1), -0.5f, edgeConstant + (z % height) * moduleHolderWidth - ((moduleHolderWidth / 2) * height - 1)), Quaternion.Euler(90, 0, 180));
                GameObject go = Instantiate(genModules[(width * height) + x + (z % height) * width], new Vector3(edgeConstant + x * moduleHolderWidth - ((moduleHolderWidth / 2) * width - 1), -0.5f, edgeConstant + (z % height) * moduleHolderWidth - ((moduleHolderWidth / 2) * height - 1)), Quaternion.Euler(0, 0, 180));
                if (go.GetComponent<Module>() != null) {
                    go.GetComponent<Module>().bombSource = this;
                }
                if (go.GetComponent<BombTimerScript>()) {
                    loadedTimerModule = go;
                }
                modules[x, z] = go.GetComponent<Module>();

                model.transform.parent = this.transform;
                go.transform.parent = this.transform;
            }
        }

        loadedTimerModule.GetComponent<BombTimerScript>().secondsLeft = generator.time;

        //bombData.Consume();
    }

    public void GenerateCodes() {
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
        idPlate.transform.localPosition = new Vector3(modules[0, 0].transform.position.x - (1.1f + edgeConstant), modules[0, 0].transform.position.y, modules[0, 0].transform.position.z);


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

    public void AddStrike(string module) {
        strikes++;
        loadedTimerModule.GetComponent<BombTimerScript>().AddStrike(strikes);
        PlayerData.currentStrikes++;
        if (strikes >= 3) {
            PlayerData.time = loadedTimerModule.GetComponent<BombTimerScript>().secondsLeft;
            PlayerData.death = module;
            PlayerData.win = false;
            Kill();
        }
    }

    public void Kill() {
        SceneManager.LoadScene("GameOverScene");
    }
}
