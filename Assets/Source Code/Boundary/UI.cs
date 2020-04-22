using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class UI : MonoBehaviour
{
    public InputField userName;

    public InputField loginUserName;

    public InputField email;

    public InputField realName;

    public InputField password;

    public InputField loginPassword;

    public InputField reEnterPW;

    //public Dropdown question;

    public Button signupBtn;

    public Button loginBtn;

    //public InputField answerForSecurity;

    public DataManager dataManager;

    private bool showPopUp = false;

    private bool showSuccessPopUp = false;

    private GUIStyle guiStyle = new GUIStyle(); //create a new variable

    [SerializeField] public AlertWindow alert;
    private void Start()
    {
        this.userName.characterLimit = 15;
        this.realName.characterLimit = 25;
        this.password.characterLimit = 15;
        this.reEnterPW.characterLimit = 15;
        this.loginPassword.characterLimit = 15;
        this.loginUserName.characterLimit = 15;
    }

    public void changeUserName(string userName)
    {
        dataManager.data.userName = userName;
    }

    public void changeLoginUserName(string loginUserName)
    {
        dataManager.logindata.userName = loginUserName;
    }

    public void changeEmail(string email)
    {
        dataManager.data.email = email;
    }

    public void changeRealName(string realName)
    {
        dataManager.data.realName = realName;
    }

    public void changePassword(string pw)
    {
        dataManager.data.password = pw;
    }

    public void changeLoginPassword(string loginPw)
    {
        dataManager.logindata.password = loginPw;
    }

    //backend not supported
    /*
    public void changeQuestion(Dropdown question)
    {
        dataManager.data.dropdown = question.value;
    }

    public void changeAnswer(string answer)
    {
        dataManager.data.answerForSecurity = answer;
    }
    */
    //string userName, string email, string realName, string pw, string answerForSecurity

    public const string MatchEmailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

    public static bool validateEmail(string email)
    {
        if ( email != null)
            return Regex.IsMatch(email, MatchEmailPattern);
        else
            return false;
    }

    public void ClickSignUp()
    {
        bool emailformat = validateEmail(dataManager.data.email);
        // || dataManager.data.dropdown ==0 || dataManager.data.answerForSecurity ==""
        if (dataManager.data.userName == "" || dataManager.data.email == "" ||  emailformat == false|| dataManager.data.realName == ""
            || dataManager.data.password == "" || dataManager.data.password != reEnterPW.text)
        {
            alert.Show("Invalid input");
        }
        else
        {
            StartCoroutine(dataManager.signUp());
            alert.Show("Sign up successfully!");
            userName.text = "";
            email.text = "";
            realName.text = "";
            password.text = "";
            reEnterPW.text = "";
            //question.value = 0;
            //answerForSecurity.text = "";
        }
    }

    public void ClickLogin()
    {
        if (dataManager.logindata.userName == "" || dataManager.logindata.password == "")
        {
            alert.Show("Invalid input");
        }
        else
        {
            StartCoroutine(dataManager.login());
            alert.Show("Login successfully!");
            userName.text = "";
            email.text = "";
        }
    }

    /*public void OnGUI()
    {
        if (showPopUp)
        {
            GUI.Window(0, new Rect((Screen.width / 2) - 150, (Screen.height / 2) - 75
                   , 500, 300), ShowGUI, "Invalid input");

        }
        if (showSuccessPopUp)
        {
            GUI.Window(1, new Rect((Screen.width / 2) - 150, (Screen.height / 2) - 75
                   , 500, 300), SucessEnterShowGUI, "Success");

        }
    }*/

   /* public void ShowGUI(int windowID)
    {
        // You may put a label to show a message to the player

        guiStyle.fontSize = 25;
        GUI.Label(new Rect(65, 70, 500, 100), "invalid input, please fill up properly.", guiStyle);

        // You may put a button to close the pop up too

        if (GUI.Button(new Rect(50, 150, 80, 40), "OK"))
        {
            showPopUp = false;
            signupBtn.interactable = true;
            signupBtn.enabled = true;
            loginBtn.interactable = true;
            loginBtn.enabled = true;
            // you may put other code to run according to your game too
        }

    }*/

  /*  public void successEnter()
    {
        if (showSuccessPopUp)
        {
            GUI.Window(1, new Rect((Screen.width / 2) - 150, (Screen.height / 2) - 75
                   , 500, 300), SucessEnterShowGUI, "Success");

        }
    }

    public void SucessEnterShowGUI(int windowID)
    {
        // You may put a label to show a message to the player

        guiStyle.fontSize = 25;
        GUI.Label(new Rect(65, 70, 500, 100), "Successfully entered!", guiStyle);

        // You may put a button to close the pop up too

        if (GUI.Button(new Rect(50, 150, 80, 40), "OK"))
        {
            showSuccessPopUp = false;
            // you may put other code to run according to your game too
        }

    }*/
}
