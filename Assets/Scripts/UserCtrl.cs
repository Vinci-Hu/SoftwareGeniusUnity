using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System;

public class UserCtrl : MonoBehaviour
{
    private UserInfo m_user;
    //public static string m_username;
    void Start()
    {
        StartCoroutine(GetAllUserRequest());
    }

    IEnumerator GetAllUserRequest()
    {
        string url = "http://localhost:9090/api/player/getAll";
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log("Error: " + webRequest.error);
            yield break;
        }

        if (webRequest.downloadHandler.text == "")
        {
            Debug.Log("User does not exist");
            yield break;
        }

        JSONNode rawJson = JSON.Parse(webRequest.downloadHandler.text);

        m_user = UserInfo.CreateFromJSON(rawJson.ToString());
    }

}
public class UserInfo
{
    private string username;
    private string userAvatar;
    private string password;
    private string email;
    private string accountType;
    private bool isAdmin;
    private int overallExp;
    private int id;

    public static UserInfo CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<UserInfo>(jsonString);
    }
}
/*@GetMapping("/getUser/{userId}")
URL: “localhost:9090/api/player/getUser/8”
Return: 
{
    "username": "Jack",
    "userAvatar": null,
    "password": "ps",
    "email": "2014@gmail.com",
    "accountType": "FB",
    "isAdmin": false,
    "overallExp": 100,
    "id": 8
}
*/
