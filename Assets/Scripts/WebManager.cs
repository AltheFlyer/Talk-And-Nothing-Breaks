using UnityEngine;
using UnityEngine.Networking;

public static class WebManager {

    private static string url = "https://plenary-totem-219601.appspot.com/tanb-data";
    private static string playerID;

    static WebManager()
    {
        string chars = "1234567890qwertyuiopasdfghjklzxcvbnm";
        for (int i = 0; i < 16; i++) {
            int rand = StaticRandom.NextInt(chars.Length);
            playerID += chars[rand];
        }
        Debug.Log("PLAYER ID");
        Debug.Log(playerID);
    }

    public static void SendData() {
        WWWForm form = new WWWForm();
        Debug.Log("Sending Data");
        using (var www = UnityWebRequest.Post(url, form)) {
            form.AddField("score", PlayerData.score);
            www.SendWebRequest();
        }
    }
}