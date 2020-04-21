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
        SceneManager.LoadScene("MapSolo");
    }
    public void onClickPM()
    {
        events.WorldType = 2;
        SceneManager.LoadScene("MapSolo");
    }
    public void onClickQA()
    {
        events.WorldType = 3;
        SceneManager.LoadScene("MapSolo");
    }
    public void onClickSE()
    {
        events.WorldType = 0;
        SceneManager.LoadScene("MapSolo");
    }

}