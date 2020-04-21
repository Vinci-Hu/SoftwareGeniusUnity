using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManagement : MonoBehaviour
{

    public Question[] _questions = null;
    public Question[] Questions { get { return _questions; } }
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
            if (events.npc_CurrentFinalScore <= 0 || events.Play_CurrentFinalScore <= 0)
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
        events.Play_CurrentFinalScore = 100;
        events.npc_CurrentFinalScore = 100;
        events.status = "pending";
        events.numOfQnsAnswered = 0;
        events.idOfAnsweredQns= new List<int>();
        events.idOfCorrectlyAnsweredQns= new List<int>();
}
    void Start()
    {
        //events.StartupHighscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);


        timerDefaultColor = timerText.color;
        LoadQuestions();

        timerStateParaHash = Animator.StringToHash("TimerState");

        var seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        UnityEngine.Random.InitState(seed);

        Display();
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

        if (question.UseTimer)
        {
            UpdateTimer(question.UseTimer);
        }
    }

    public void Accept()
    {
        UpdateTimer(false);
        bool isCorrect = CheckAnswers();
        FinishedQuestions.Add(currentQuestion);
        events.numOfQnsAnswered++;
        events.idOfAnsweredQns.Add(currentQuestion);
        if(isCorrect)
        {
            events.idOfCorrectlyAnsweredQns.Add(currentQuestion);
        }
        UpdateScore((isCorrect) ? Questions[currentQuestion].AddScore : (- Questions[currentQuestion].AddScore));

        

        var type = (IsFinished)? UIManager.ResolutionScreenType.Finish: (isCorrect) ? UIManager.ResolutionScreenType.Correct: UIManager.ResolutionScreenType.Incorrect;

        if (events.DisplayResolutionScreen != null)
        {
            int score = 0;
            if(isCorrect)
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
        var totalTime = Questions[currentQuestion].Timer;
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
            List<int> c = Questions[currentQuestion].GetCorrectAnswers();
            List<int> p = PickedAnswers.Select(x => x.AnswerIndex).ToList();

            var f = c.Except(p).ToList();
            var s = p.Except(c).ToList();

            return !f.Any() && !s.Any();
        }

        return false;
    } 

    Question GetRandomQuestion()
    {
        var randomIndex = GetRandomQuestionIndex();
        currentQuestion = randomIndex;

        return Questions[currentQuestion];
    }

    int GetRandomQuestionIndex()
    {
        var random = 0;
        if (FinishedQuestions.Count < Questions.Length)
        {
            do
            {
                random = UnityEngine.Random.Range(0, Questions.Length);
            } while (FinishedQuestions.Contains(random) || random == currentQuestion);
        }
        return random;
    }

    void LoadQuestions()
    {
        Object[] objs = Resources.LoadAll("Questions", typeof(Question));
        _questions = new Question[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            _questions[i] = (Question)objs[i];
        }
    }

    private void UpdateScore(int add)
    {
        events.CurrentFinalScore += add;
        if(add>0)
        {
            events.npc_CurrentFinalScore -= events.Player_DP;
        }
        if (add < 0)
        {
            events.Play_CurrentFinalScore -= events.NPC_DP;

        }
        if (events.ScoreUpdated != null)
        {
            events.ScoreUpdated();
        }
    }

    public void ReStart()
    {
        saveDataAfter();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        
    }

    public void BackToMap()
    {
        saveDataAfter();
        SceneManager.LoadScene("mapSolo");
    }
    public void saveDataAfter()
    {


        PostAfterData AfterData = new PostAfterData();
        AfterData.characterId = events.playerId;
        AfterData.status = events.status;
        AfterData.numOfQnsAnswered = events.numOfQnsAnswered;
        AfterData.idOfAnsweredQns = events.idOfAnsweredQns;
        AfterData.idOfCorrectlyAnsweredQns = events.idOfCorrectlyAnsweredQns;

        string json = JsonUtility.ToJson(AfterData);
        Debug.Log(json);

        //File.WriteAllText(Application.dataPath + "saveFile.json",json);

    }
}

public class PostAfterData
{
    public string characterId;
    public string status;
    public int numOfQnsAnswered;
    public List<int> idOfAnsweredQns;
    public List<int> idOfCorrectlyAnsweredQns;

}
