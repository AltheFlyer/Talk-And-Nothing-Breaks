using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BombData : MonoBehaviour
{

    //These values needs to be set...
    public List<GameObject> prefabModules;
    public List<string> moduleNames;
    //For this to work
    public Dictionary<string, GameObject> allModules = new Dictionary<string, GameObject>();

    //Generation data
    public BombInfo meta;

    
    //public Dictionary<string, ModuleInfo> moduleConfig = new Dictionary<string, ModuleInfo>();

    // Start is called before the first frame update
    public void Start()
    {
        meta = JsonUtility.FromJson<BombInfo>(Levels.GetGenerator(""));
        for (int i = 0; i < moduleNames.Count; i++) {
            allModules.Add(moduleNames[i], prefabModules[i]);
        }
        DontDestroyOnLoad(this);
        SendData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Consume() {
        Destroy(this.gameObject);
    }

    public void SetData(string jsonPath) {
        /*"Assets/Generators/test.json"*/
        meta = JsonUtility.FromJson<BombInfo>(Levels.GetGenerator(jsonPath));
        /*
        foreach (ModuleInfo mf in meta.modules) {
            moduleConfig.Add(mf.name, mf);
        }
        */
    }

    //GOD OBJECT TIME
    public void SendData() {
        StartCoroutine(SD());
    }       

    public IEnumerator SD() {  
        WWWForm form = new WWWForm();
        form.AddField("id", PlayerData.playerID);
        form.AddField("score", PlayerData.totalScore);
        
        Debug.Log("Sending Data");
        
        using (var www = UnityWebRequest.Post(WebManager.url, form)) {
            yield return www.SendWebRequest();
            
            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            }
            else {
                Debug.Log("Score Data Sent");
            }
        }
    }  
}
