using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    //[SerializeField] private string level;
    public void ButtonMoveScene(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void toSignUpAdmin()
    {
        SceneManager.LoadScene("SignUpAdmin");
        StaticVariable.isStudent = false;
    }

    public void toSignUpStudent()
    {
        SceneManager.LoadScene("SignUpStudent");
        StaticVariable.isStudent = true;
    }

    public void toProfilePage()
    {
        SceneManager.LoadScene("Profile");
    }
    public void toAddQuestionScene()
    {
        SceneManager.LoadScene("AddQuestion");
    }

    public void toMainScene()
    {
        if (StaticVariable.isStudent)
        {
            SceneManager.LoadScene("Mode");
        }
        else
        {
            SceneManager.LoadScene("TeacherMode");
        }
        StaticVariable.scene = "Mode";
    }

    public void toLeaderBoard()
    {
        SceneManager.LoadScene("LeaderBoard");
    }

    public void profilePageTransferScene()
    {
        SceneManager.LoadScene(StaticVariable.scene);
    }

    public void reportPageTransferScene()
    {
        SceneManager.LoadScene(StaticVariable.scene);
    }

    public void toIndiReport()
    {
        SceneManager.LoadScene("IndiReport");
    }

    public void toAllReport()
    {
        SceneManager.LoadScene("AllReport");
        StaticVariable.scene = "AllReport";
    }

    public void toQuestionTank()
    {
        SceneManager.LoadScene("Questions");
    }
    public void SignUpOptions()
    {
        SceneManager.LoadScene("SignUpOptions");
    }

    public void SignUpStudent()
    {
        SceneManager.LoadScene("SignUpStudent");
    }

    public void SignUpAdmin()
    {
        SceneManager.LoadScene("SignUpAdmin");
    }

    public void Login()
    {
        SceneManager.LoadScene("Login");
    }
    public void toGame()
    {
        SceneManager.LoadScene("1level");
    }
    public void toOverallReport()
    {
        SceneManager.LoadScene("OverallReport");
    }
}
