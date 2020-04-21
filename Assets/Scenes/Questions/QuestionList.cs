using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class QuestionList : MonoBehaviour
{

    private Transform buttonScrollList;
    private Transform buttonListViewPort;
    private Transform buttonListContent;
    private Transform questionEntryTemplate;
    private List<Transform> questionEntryTransformList;
    private int choice;
    private Entries entries;
    bool updated = false;

    static public string questionId;

    public void HandelInputData(int val)
    {
        choice = val;
        updated = true;
        Debug.Log(choice);
    }

    private class Entries
    {
        public List<QuestionEntry> questionEntryList;
    }

    [System.Serializable]
    private class QuestionEntry
    {
        public string questionId;
        public string title;
        public float accuracy;
    }

    void Update()
    {
        if (updated == true)
        {
            ClearList(questionEntryTransformList);
            Display(choice);
            updated = false;
        }
    }

    public void QuestionClicked()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.transform.Find("QuestionId").GetComponent<Text>().text);
        StaticVariable.questionId = EventSystem.current.currentSelectedGameObject.transform.Find("QuestionId").GetComponent<Text>().text;
        SceneManager.LoadScene("QuestionTeacher");
    }

    private void Start()
    {
        buttonScrollList = transform.Find("ButtonScrollList");
        buttonListViewPort = buttonScrollList.Find("ButtonListViewPort");
        buttonListContent = buttonListViewPort.Find("ButtonListContent");
        questionEntryTemplate = buttonListContent.Find("QuestionEntryTemplate");
        questionEntryTemplate.gameObject.SetActive(false);

        // when launching for the first time, create test lib
        AddTestLibrary();

        // load data from json
        string jsonString = PlayerPrefs.GetString("questionTable");
        entries = JsonUtility.FromJson<Entries>(jsonString);

        // add new entry (would be added to json)
        //AddQuestionEntry("id=1", "nice question", 0.6f);
        //AddQuestionEntry("id=2", "good question", 0.7f);

        Debug.Log("Start Called");
        Display(choice);

    }

    private void Display(int choice)
    {
        Debug.Log("Display called");
        List<QuestionEntry> sortedList = Sort(entries.questionEntryList, choice);

        questionEntryTransformList = new List<Transform>();
        for (int i = 0; i < sortedList.Count; i++)
        {
            Transform entryTransform = Instantiate(questionEntryTemplate, buttonListContent);
            entryTransform.gameObject.SetActive(true);
            questionEntryTransformList.Add(entryTransform);
            questionEntryTransformList[i].Find("QuestionId").GetComponent<Text>().text = sortedList[i].questionId;
            questionEntryTransformList[i].Find("Title").GetComponent<Text>().text = sortedList[i].title;
            questionEntryTransformList[i].Find("Accuracy").GetComponent<Text>().text = (sortedList[i].accuracy * 100).ToString() + "%";
            questionEntryTransformList[i].Find("bg").gameObject.SetActive(i % 2 == 0);
        }
    }

    private void ClearList(List<Transform> questionEntryTransformList)
    {
        foreach(Transform entryTransform in questionEntryTransformList)
        {
            Destroy(entryTransform.gameObject);
        }
    }

    private void AddQuestionEntry(string questionId, string title, float accuracy)
    {
        QuestionEntry questionEntry = new QuestionEntry { questionId = questionId, title = title, accuracy = accuracy };
        string jsonString = PlayerPrefs.GetString("questionTable");
        entries = JsonUtility.FromJson<Entries>(jsonString);
        entries.questionEntryList.Add(questionEntry);
        string json = JsonUtility.ToJson(entries);
        PlayerPrefs.SetString("questionTable", json);
        PlayerPrefs.Save();
    }

    private List<QuestionEntry> Sort(List<QuestionEntry> questionEntryList, int criteria)
    {
        // sort by question ID
        if (criteria == 0)
        {
            for (int i = 0; i < questionEntryList.Count; i++)
            {
                for (int j = i + 1; j < questionEntryList.Count; j++)
                {
                    if (string.Compare(questionEntryList[j].questionId, questionEntryList[i].questionId) < 0)
                    {
                        QuestionEntry temp = questionEntryList[i];
                        questionEntryList[i] = questionEntryList[j];
                        questionEntryList[j] = temp;
                    }
                }
            }
        }
        // sort by accuracy low to high
        else if (criteria == 1)
        {
            for (int i = 0; i < questionEntryList.Count; i++)
            {
                for (int j = i + 1; j < questionEntryList.Count; j++)
                {
                    if (questionEntryList[j].accuracy < questionEntryList[i].accuracy)
                    {
                        QuestionEntry temp = questionEntryList[i];
                        questionEntryList[i] = questionEntryList[j];
                        questionEntryList[j] = temp;
                    }
                }
            }
        }
        // sort by accuracy high to low
        else if (criteria == 2)
        {
            for (int i = 0; i < questionEntryList.Count; i++)
            {
                for (int j = i + 1; j < questionEntryList.Count; j++)
                {
                    if (questionEntryList[j].accuracy > questionEntryList[i].accuracy)
                    {
                        QuestionEntry temp = questionEntryList[i];
                        questionEntryList[i] = questionEntryList[j];
                        questionEntryList[j] = temp;
                    }
                }
            }
        }
        // sort by title
        else if (criteria == 3)
        {
            for (int i = 0; i < questionEntryList.Count; i++)
            {
                for (int j = i + 1; j < questionEntryList.Count; j++)
                {
                    if (string.Compare(questionEntryList[j].title, questionEntryList[i].title) < 0)
                    {
                        QuestionEntry temp = questionEntryList[i];
                        questionEntryList[i] = questionEntryList[j];
                        questionEntryList[j] = temp;
                    }
                }
            }

        }
        return questionEntryList;
    }


    private void AddTestLibrary()
    {
        // create first entry
        List<QuestionEntry> questionEntries = new List<QuestionEntry>
        {
            new QuestionEntry {questionId = "000", title = "test lib question", accuracy = 1.0f},
            new QuestionEntry {questionId = "111", title = "test lib question2", accuracy = 0.9f},

        };

        // create json file
        Entries testEntries = new Entries { questionEntryList = questionEntries };
        string json_questions = JsonUtility.ToJson(testEntries);
        PlayerPrefs.SetString("questionTable", json_questions);
        PlayerPrefs.Save();
    }
}
