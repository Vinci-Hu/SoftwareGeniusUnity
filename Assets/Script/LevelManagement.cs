using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;



public class LevelManagement : MonoBehaviour
{


    [SerializeField] GameEvent events = null;
    [System.Obsolete]
    public void onClickEasy()
    {
        events.level = 0;
        events.levelType = GameEvent.LevelType.Easy;
        SceneManager.LoadScene("2Question");
    }
    public void onClickMedium()
    {
        events.level = 1;
        events.levelType = GameEvent.LevelType.Medium;
        SceneManager.LoadScene("2Question");
    }
    public void onClickHard()
    {
        events.level = 2;
        events.levelType = GameEvent.LevelType.Hard;
        SceneManager.LoadScene("2Question");
    }
    public void onClickBack()
    {
        SceneManager.LoadScene("MapSolo");
    }



}
