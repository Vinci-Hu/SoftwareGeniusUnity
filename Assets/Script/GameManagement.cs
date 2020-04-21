using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.Networking;

public class GameManagement : MonoBehaviour
{

    public JSONNode newJson;
    public Qqustion QList;
    public int QuestionIndex = 0;
    [SerializeField] GameEvent events = null;

    [SerializeField] Animator timerAnimtor = null;
    [SerializeField] TextMeshProUGUI timerText = null;
    [SerializeField] Color timerHalfWayOutColor = Color.yellow;
    [SerializeField] Color timerAlmostOutColor = Color.red;
    private Color timerDefaultColor = Color.white;


    private List<AnswerData> PickedAnswers = new List<AnswerData>();
    private List<int> FinishedQuestions = new List<int>();
    private int currentQuestion = 0;

    private int timerStateParaHash = 0;

    private IEnumerator IE_WaitTillNextRound = null;
    private IEnumerator IE_StartTimer = null;

    private bool IsFinished
    {
        get
        {
            if (events.npc_HP <= 0 || events.Play_HP <= 0)
                return true;
            else
                return false;

        }
    }

    void OnEnable()
    {
        events.UpdateQuestionAnswer += UpdateAnswers;
    }

    void OnDisable()
    {
        events.UpdateQuestionAnswer -= UpdateAnswers;
    }

    void Awake()
    {
        events.EXP = 0;
        events.CurrentFinalScore = 0;
        events.Play_HP = 100;
        events.npc_HP = 100;
        events.status = "pending";
        events.numOfQnsAnswered = 0;
        events.idOfAnsweredQns = new List<int>();
        events.idOfCorrectlyAnsweredQns = new List<int>();
    }

    void Start()
    {
        StartCoroutine(PostBefore());
    }

    public void UpdateAnswers(AnswerData newAnswer)
    {
        //if (Questions[currentQuestion].GetAnswerType == Question.AnswerType.Single)
        {
            foreach (var answer in PickedAnswers)
            {
                if (answer != newAnswer)
                {
                    answer.Reset();
                }
            }
            PickedAnswers.Clear();
            PickedAnswers.Add(newAnswer);
        }
        /*else/ for multiple question
        {
            bool alreadyPicked = PickedAnswers.Exists(x => x == newAnswer);
            if (alreadyPicked)
            {
                PickedAnswers.Remove(newAnswer);
            }
            else
            {
                PickedAnswers.Add(newAnswer);
            }
        }*/
    }

    void Display()
    {
        //EraseAnswers
        PickedAnswers = new List<AnswerData>();

        var question = GetRandomQuestion();

        if (events.UpdateQuestionUI != null)
        {
            events.UpdateQuestionUI(question);
        }
        else { Debug.LogWarning("Something wrong while trying to display new Question UI Data. GameEvents.UpdateQuestionUI is null. Issue occured in GameManager.Display() method."); }



        UpdateTimer(true);

    }

    public void Accept()
    {
        UpdateTimer(false);
        bool isCorrect = CheckAnswers();
        FinishedQuestions.Add(currentQuestion);
        events.numOfQnsAnswered++;
        events.idOfAnsweredQns.Add(currentQuestion);
        if (isCorrect)
        {
            events.idOfCorrectlyAnsweredQns.Add(currentQuestion);
        }
        UpdateScore((isCorrect) ? true : false);

        var type = (IsFinished) ? UIManager.ResolutionScreenType.Finish : (isCorrect) ? UIManager.ResolutionScreenType.Correct : UIManager.ResolutionScreenType.Incorrect;

        if (events.DisplayResolutionScreen != null)
        {
            int score = 0;
            if (isCorrect)
            {
                score = events.Player_DP;
            }
            else
            {
                score = events.NPC_DP;
            }
            events.DisplayResolutionScreen(type, score);
        }

        //AudioManager.Instance.PlaySound((isCorrect) ? "CorrectSFX" : "IncorrectSFX");

        if (type != UIManager.ResolutionScreenType.Finish)
        {
            if (IE_WaitTillNextRound != null)
            {
                StopCoroutine(IE_WaitTillNextRound);
            }
            IE_WaitTillNextRound = WaitTillNextRound();
            StartCoroutine(IE_WaitTillNextRound);
        }
    }

    void UpdateTimer(bool state)
    {
        switch (state)
        {
            case true:
                IE_StartTimer = StartTimer();
                StartCoroutine(IE_StartTimer);

                timerAnimtor.SetInteger(timerStateParaHash, 2); //pop up
                break;
            case false:
                if (IE_StartTimer != null)
                {
                    StopCoroutine(IE_StartTimer);
                }

                timerAnimtor.SetInteger(timerStateParaHash, 1);
                break;
        }
    }

    IEnumerator StartTimer()
    {
        var totalTime = 25;
        var timeLeft = totalTime;

        timerText.color = timerDefaultColor;
        while (timeLeft >= 0)
        {
            timerText.text = timeLeft.ToString();


            //AudioManager.Instance.PlaySound("CountdownSFX");

            if (timeLeft < totalTime / 2 && timeLeft > totalTime / 4)
            {
                timerText.color = timerHalfWayOutColor;
            }
            if (timeLeft < totalTime / 4)
            {
                timerText.color = timerAlmostOutColor;
            }


            yield return new WaitForSeconds(1.0f);
            timeLeft--;
        }
        Accept();
    }

    IEnumerator WaitTillNextRound()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        Display();
    }

    bool CheckAnswers()
    {
        if (!CompareAnswers())
        {
            return false;
        }
        return true;
    }

    bool CompareAnswers()
    {
        if (PickedAnswers.Count > 0)
        {
            List<int> c = new List<int>();
            int ans = QList.ListOfQuestions[currentQuestion].answer - 1;
            c.Add(ans);

            List<int> p = PickedAnswers.Select(x => x.AnswerIndex).ToList();

            var f = c.Except(p).ToList();
            var s = p.Except(c).ToList();

            return !f.Any() && !s.Any();
        }

        return false;
    }

    QuestionLoad GetRandomQuestion()
    {
        var randomIndex = GetRandomQuestionIndex();
        currentQuestion = randomIndex;
        Debug.Log(QList.ListOfQuestions[currentQuestion].answer);
        return QList.ListOfQuestions[currentQuestion];
    }

    int GetRandomQuestionIndex()
    {
        QuestionIndex++;
        return QuestionIndex;
    }

    private void UpdateScore(bool add)
    {

        if (add)
        {
            events.npc_HP -= events.Player_DP;
        }
        else
        {
            events.Play_HP -= events.NPC_DP;

        }
        if (events.ScoreUpdated != null)
        {
            events.ScoreUpdated();
        }
    }

    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMap()
    {
        SceneManager.LoadScene("mapSolo");
    }

    IEnumerator PostBefore()
    {
        PostBeforeData BeforeData = new PostBeforeData();
        /*BeforeData.worldId = events.WorldType;
        BeforeData.landID = events.landID;
        BeforeData.difficultyLevel = events.level;
        BeforeData.mode = events.mode;
        BeforeData.playID = events.playerId;*/
        BeforeData.worldId = 1;
        BeforeData.landID = 1;
        BeforeData.difficultyLevel = 1;
        BeforeData.mode = "battle";
        BeforeData.playID = 1;

        string json = JsonUtility.ToJson(BeforeData);
        string url = "localhost:9090/api/combat/start";

        UnityWebRequest userRequest = UnityWebRequest.Post(url, json);
        userRequest.uploadHandler.contentType = "application/json";
        userRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        userRequest.SetRequestHeader("Accept", "application/json");
        userRequest.SetRequestHeader("Content-Type", "application/json");

        //Debug.Log(json);

        yield return userRequest.SendWebRequest();

        if (userRequest.isNetworkError || userRequest.isHttpError)
        {
            Debug.LogError(userRequest.error);
            yield break;
        }
        else
        {


            //Debug.Log(userRequest.downloadHandler.text);
            JSONNode StartBattleInfo = JSON.Parse(userRequest.downloadHandler.text);
            if (StartBattleInfo.Tag == JSONNodeType.Object)
            {
                foreach (KeyValuePair<string, JSONNode> kvp in (JSONObject)StartBattleInfo)
                {
                    newJson = kvp.Value;

                }
            }
            //Debug.Log(newJson.Count);

            string StartBattleString = "{\"ListOfQuestions\": " + newJson.ToString() + "}";
            QList = JsonUtility.FromJson<Qqustion>(StartBattleString);
            Debug.Log(QList.ListOfQuestions.Count + "load successfully");

            timerDefaultColor = timerText.color;
            //LoadQuestions();

            timerStateParaHash = Animator.StringToHash("TimerState");

            var seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            UnityEngine.Random.InitState(seed);

            Display();
        }

    }

    public class PostBeforeData
    {
        public int worldId;
        public int landID;
        public int difficultyLevel;
        public string mode;
        public int playID;

    }


}

