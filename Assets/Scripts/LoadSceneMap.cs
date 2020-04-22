using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneMap : MonoBehaviour
{
    [SerializeField] GameEvent events = null;
    public void sceneLoader(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }
    public void EnterCombatSolo()
    {
        events.landID = LandCtrl.standingOn;
        events.mode = "battle";
        SceneManager.LoadScene(13);
    }
    public void EnterCombatDuel()
    {
        events.landID = LandCtrl.standingOn;
        events.mode = "duel";
        //LandCtrl.
        SceneManager.LoadScene(13);
    }
    public void EnterBattleScene()
    {
        events.mode = "duel";
        SceneManager.LoadScene(16);
    }
}
