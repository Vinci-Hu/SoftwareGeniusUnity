using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PostBeforeData
{
    public int worldId;
    public int landID;
    public int difficultyLevel;
    public string mode;
    public string playID;
    public int npcId;

}


public class LevelManagement : MonoBehaviour
{


    [SerializeField] GameEvent events = null;

    [System.Obsolete]
    public void onClickEasy()
    {
        events.level = 0;
        saveDataBefore();
        SceneManager.LoadScene("2Question");
    }
    public void onClickMedium()
    {
        events.level = 1;
        saveDataBefore();
        SceneManager.LoadScene("2Question");
    }
    public void onClickHard()
    {
        events.level = 2;
        saveDataBefore();
        SceneManager.LoadScene("2Question");
    }
    public void onClickBack()
    {
        SceneManager.LoadScene("MapSolo");
    }

    public void saveDataBefore()
    {


        PostBeforeData BeforeData = new PostBeforeData();
        BeforeData.worldId = events.WorldType;
        BeforeData.landID = events.landID;
        BeforeData.difficultyLevel = events.level;
        BeforeData.mode = events.mode;
        BeforeData.playID = events.playerId;
        BeforeData.npcId = events.level;

        string json = JsonUtility.ToJson(BeforeData);
        Debug.Log(json);

        //File.WriteAllText(Application.dataPath + "saveFile.json",json);

    }

}
