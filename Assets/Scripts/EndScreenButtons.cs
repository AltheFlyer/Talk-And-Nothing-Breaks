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
    GameObject level, time, causeTitle, cause, score, defused;
    BombData data;

    // Start is called before the first frame update
    void Start()
    {
        string name = PlayerData.currentLevel;
        data = GameObject.Find("BombData").GetComponent<BombData>();
        data.SetData("Assets/Generators/" + name + ".json");
        level.GetComponent<Text>().text = "Level: " + data.meta.name;
        time.GetComponent<Text>().text = ((int)(PlayerData.time / 60)).ToString("00") + ":" + (PlayerData.time % 60).ToString("00.00");
        score.GetComponent<Text>().text = "Level Score:" + PlayerData.currentScore;

        if (PlayerData.win) {
            cause.SetActive(false);
            causeTitle.SetActive(false);
            defused.SetActive(true);

        } else {
            cause.SetActive(true);
            causeTitle.SetActive(true);
            defused.SetActive(false);
            cause.GetComponent<Text>().text = PlayerData.death;
        }

        PlayerData.UpdateLevelStats(name);

        StartCoroutine(MakeRequest());
    }

    public void PlayLevel()
    {
        PlayerData.currentScore = 0;
        PlayerData.currentStrikes = 0;
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
