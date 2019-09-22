using System.IO;

public class FileIO {

    public static string ReadString(string path) {
        StreamReader reader = new StreamReader(path);
        string s = reader.ReadToEnd();
        reader.Close();
        return s;
    }
}