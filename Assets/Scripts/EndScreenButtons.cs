using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class EndScreenButtons : MonoBehaviour
{
    [SerializeField]
    GameObject level, time, cause, score;
    BombData data;

    // Start is called before the first frame update
    void Start()
    {
        level.GetComponent<Text>().text = "Level: " + PlayerData.currentLevel;
        time.GetComponent<Text>().text = (int)(PlayerData.time / 60) + ":" + PlayerData.time % 60;
        cause.GetComponent<Text>().text = PlayerData.death;
        score.GetComponent<Text>().text = "Level Score:" + PlayerData.currentScore;

        data = GameObject.Find("BombData").GetComponent<BombData>();

        StartCoroutine(MakeRequest());
    }

    public void PlayLevel()
    {
        string name = PlayerData.currentLevel;
        data.SetData("Assets/Generators/" + name + ".json");
        PlayerData.currentLevel = name;
        SceneManager.LoadScene("LevelGeneratorScene");
    }

    public void ReturnToMenu()
    {
        print("yes");
        data.Consume();
        SceneManager.LoadScene("GameMenuScene");
    }

    public IEnumerator MakeRequest() {
        WWWForm form = new WWWForm();
        form.AddField("user", PlayerData.playerID);
        form.AddField("death", PlayerData.death);
        form.AddField("level", PlayerData.currentLevel);
        
        Debug.Log("Sending Data");
        
        using (var www = UnityWebRequest.Post(WebManager.kaboomUrl, form)) {
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
