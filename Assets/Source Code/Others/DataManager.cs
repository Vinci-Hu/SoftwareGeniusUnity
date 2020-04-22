using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour
{
    public Data data;
    public loginData logindata;
    public string signupurl = "http://localhost:9090/api/player/addUser";
    public string loginurl = "http://localhost:9090/api/player/login";

    private string signupfile = "signupData.txt";

    private string loginfile = "loginData.txt";

    public IEnumerator signUp()
    {
        string json = JsonUtility.ToJson(this.data);
        UnityWebRequest PostRequest = UnityWebRequest.Post(signupurl, json);
        PostRequest.uploadHandler.contentType = "application/json";
        PostRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        PostRequest.SetRequestHeader("Accept", "application/json");
        PostRequest.SetRequestHeader("Content-Type", "application/json");

        yield return PostRequest.SendWebRequest();

        if (PostRequest.isNetworkError || PostRequest.isHttpError)
        {
            Debug.LogError(PostRequest.error);
            yield break;
        }
        Debug.Log(json);
        //writeToFile(signupfile, json);
    }

    public IEnumerator login()
    {
        //login的data是不是重新定义一个data structure
        string json = JsonUtility.ToJson(this.logindata);
        UnityWebRequest PostRequest = UnityWebRequest.Post(loginurl, json);
        PostRequest.uploadHandler.contentType = "application/json";
        PostRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        PostRequest.SetRequestHeader("Accept", "application/json");
        PostRequest.SetRequestHeader("Content-Type", "application/json");

        yield return PostRequest.SendWebRequest();

        if (PostRequest.isNetworkError || PostRequest.isHttpError)
        {
            Debug.LogError(PostRequest.error);
            yield break;
        }
        else
        {
            Debug.Log(json);
            // return the successful login in stu id
            StaticVariable.studentId = PostRequest.downloadHandler.text;
        }
        //writeToFile(loginfile, json);
    }

    public void writeToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }

    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }
}
