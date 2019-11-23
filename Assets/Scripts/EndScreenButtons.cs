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
    GameObject level, time, causeTitle, cause, score, defused, fade;
    [SerializeField]
    AudioSource audio;
    [SerializeField]
    AudioClip boom;
    [SerializeField]
    AudioClip victory;
    BombData data;
    float pause;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerData.win) {
            audio.PlayOneShot(boom);
        } else {
            audio.PlayOneShot(victory);
        }
        string name = PlayerData.currentLevel;
        data = GameObject.Find("BombData").GetComponent<BombData>();
        data.SetData(name);
        level.GetComponent<Text>().text = "Level: " + data.meta.name;
        if (PlayerData.death == "Out of Time") {
            time.GetComponent<Text>().text = "00:00.00";
        } else {
            time.GetComponent<Text>().text = ((int)(PlayerData.time / 60)).ToString("00") + ":" + (PlayerData.time % 60).ToString("00.00");
        }
        score.GetComponent<Text>().text = "Level Score:" + PlayerData.currentScore;

        if (PlayerData.win) {
            cause.SetActive(false);
            causeTitle.SetActive(false);
            defused.SetActive(true);
            StartCoroutine(MakeWinRequest());
        } else {
            cause.SetActive(true);
            causeTitle.SetActive(true);
            defused.SetActive(false);
            cause.GetComponent<Text>().text = PlayerData.death;
            StartCoroutine(MakeKaboomRequest());
        }

        PlayerData.UpdateLevelStats(name);
        pause = Time.time;
    }

    public void Update ()
    {
        if (Time.time - pause > 2 && fade.GetComponent<Image>().color.a > 0) {
            fade.GetComponent<Image>().color = new Color(0, 0, 0, (float)(fade.GetComponent<Image>().color.a - 0.1 * Time.deltaTime));
        }
    }

    public void PlayLevel()
    {
        PlayerData.currentScore = 0;
        PlayerData.currentStrikes = 0;
        SceneManager.LoadScene("LevelGeneratorScene");
    }

    public void ReturnToMenu()
    {
        data.Consume();
        SceneManager.LoadScene("GameMenuScene");
    }

    public IEnumerator MakeKaboomRequest() {
        WWWForm form = new WWWForm();
        form.AddField("user", PlayerData.playerID);
        form.AddField("death", PlayerData.death);
        form.AddField("level", PlayerData.currentLevel);
        
        Debug.Log("Sending Data");
        
        using (var www = UnityWebRequest.Post(WebManager.kaboomUrl, form)) {
            yield return www.SendWebRequest();
        }
    }

    public IEnumerator MakeWinRequest() {
        WWWForm form = new WWWForm();
        form.AddField("user", PlayerData.playerID);
        form.AddField("level", PlayerData.currentLevel);
        form.AddField("timeLeft", PlayerData.time.ToString());
        form.AddField("strikes", PlayerData.currentStrikes.ToString());
        
        Debug.Log("Sending Data");
        
        using (var www = UnityWebRequest.Post(WebManager.winUrl, form)) {
            yield return www.SendWebRequest();
        }
    }
}
