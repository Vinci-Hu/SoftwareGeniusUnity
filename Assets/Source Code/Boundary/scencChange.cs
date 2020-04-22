using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scencChange : MonoBehaviour
{


    [SerializeField] GameEvent events = null;

    public void onClickSA()
    {
        events.WorldType = 1;
        events.mode = "battle";
        SceneManager.LoadScene("MapSolo");
    }
    public void onClickPM()
    {
        events.WorldType = 2;
        events.mode = "battle";
        SceneManager.LoadScene("MapSolo");
    }
    public void onClickQA()
    {
        events.WorldType = 3;
        events.mode = "battle";
        SceneManager.LoadScene("MapSolo");
    }
    public void onClickSE()
    {
        events.WorldType = 0;
        events.mode = "battle";
        SceneManager.LoadScene("MapSolo");
    }

}