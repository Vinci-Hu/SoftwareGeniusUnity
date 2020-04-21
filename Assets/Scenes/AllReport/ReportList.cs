using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ReportList : MonoBehaviour
{
    private Transform buttonScrollList;
    private Transform buttonListViewPort;
    private Transform buttonListContent;
    private Transform reportEntryTemplate;
    private List<Transform> reportEntryTransformList;
    private int choice;
    private Entries entries;
    bool updated = false;

    static public string studentId;


    public void HandleInputData(int val)
    {
        choice = val;
        updated = true;
        Debug.Log(choice);
    }


    private class Entries
    {
        public List<ReportEntry> reportEntryList;
    }


    [System.Serializable]
    private class ReportEntry
    {
        public string studentId;
        public string name;
        public float accuracy;
    }


    public void Update()
    {
        if (updated == true)
        {
            ClearList(reportEntryTransformList);
            Display(choice);
            updated = false;
        }
    }


    public void ReportClicked()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.transform.Find("StudentId").GetComponent<Text>().text);
        StaticVariable.studentId = EventSystem.current.currentSelectedGameObject.transform.Find("StudentId").GetComponent<Text>().text;
        SceneManager.LoadScene("IndiReport");
    }

    private void Start()
    {
        buttonScrollList = transform.Find("ButtonScrollList");
        buttonListViewPort = buttonScrollList.Find("ButtonListViewPort");
        buttonListContent = buttonListViewPort.Find("ButtonListContent");
        reportEntryTemplate = buttonListContent.Find("ReportEntryTemplate");
        reportEntryTemplate.gameObject.SetActive(false);

        // when launching for the first time, create test lib
        //AddTestLibrary();

        // load data from json
        string jsonString = PlayerPrefs.GetString("reportTable");
        entries = JsonUtility.FromJson<Entries>(jsonString);

        // add new entry (would be added to json)
        //AddQuestionEntry("id=1", "nice question", 0.6f);
        //AddQuestionEntry("id=2", "good question", 0.7f);
        //AddQuestionEntry("id=3", "hello question", 0.5f);
        //AddQuestionEntry("id=4", "what question", 0.3f);
        //AddQuestionEntry("id=5", "aiii question", 0.6f);
        //AddQuestionEntry("id=6", "sad question", 0.9f);
        //AddQuestionEntry("id=7", "what todo question", 1.0f);
        //AddQuestionEntry("id=8", "aaaah question", 0.4f);
        //AddQuestionEntry("id=9", "damn question", 0.5f);
        //AddQuestionEntry("id=10", "orz question", 0.66f);
        //AddQuestionEntry("id=11", "hell question", 0.67f);

        Display(choice);
    }


    private void Display(int choice)
    {
        List<ReportEntry> sortedList = Sort(entries.reportEntryList, choice);

        reportEntryTransformList = new List<Transform>();
        for (int i = 0; i < sortedList.Count; i++)
        {
            Transform entryTransform = Instantiate(reportEntryTemplate, buttonListContent);
            entryTransform.gameObject.SetActive(true);
            reportEntryTransformList.Add(entryTransform);
            reportEntryTransformList[i].Find("StudentId").GetComponent<Text>().text = sortedList[i].studentId;
            reportEntryTransformList[i].Find("Name").GetComponent<Text>().text = sortedList[i].name;
            reportEntryTransformList[i].Find("Accuracy").GetComponent<Text>().text = (sortedList[i].accuracy * 100).ToString() + "%";
            reportEntryTransformList[i].Find("bg").gameObject.SetActive(i % 2 == 0);
        }
    }


    private void ClearList(List<Transform> reportEntryTransformList)
    {
        foreach (Transform entryTransform in reportEntryTransformList)
        {
            Destroy(entryTransform.gameObject);
        }
    }


    private void AddReportEntry(string studentId, string name, float accuracy)
    {
        ReportEntry reportEntry = new ReportEntry { studentId = studentId, name = name, accuracy = accuracy };
        string jsonString = PlayerPrefs.GetString("reportTable");
        entries = JsonUtility.FromJson<Entries>(jsonString);
        entries.reportEntryList.Add(reportEntry);
        string json = JsonUtility.ToJson(entries);
        PlayerPrefs.SetString("reportTable", json);
        PlayerPrefs.Save();
    }


    private void AddTestLibrary()
    {
        List<ReportEntry> reportEntryListcreated = new List<ReportEntry>()
        {
            new ReportEntry{studentId = "3471827", name = "Dou Maokang", accuracy = 0.6f},
            new ReportEntry{studentId = "4535345", name = "Duan Maokang", accuracy = 0.7f},
            new ReportEntry{studentId = "5254646", name = "Wang Maokang", accuracy = 0.34f},
            new ReportEntry{studentId = "3457856", name = "Li Maokang", accuracy = 0.23f},
            new ReportEntry{studentId = "3453657", name = "Zhang Maokang", accuracy = 0.98f},
            new ReportEntry{studentId = "2346765", name = "Chen Maokang", accuracy = 0.6f},
            new ReportEntry{studentId = "8765424", name = "Jin Maokang", accuracy = 0.54f},
            new ReportEntry{studentId = "6867453", name = "Zhou Maokang", accuracy = 0.6f},
            new ReportEntry{studentId = "5673463", name = "Zou Maokang", accuracy = 0.67f},
            new ReportEntry{studentId = "9876534", name = "Gan Maokang", accuracy = 0.88f}
        };
        Entries reports = new Entries { reportEntryList = reportEntryListcreated };
        string json = JsonUtility.ToJson(reports);
        PlayerPrefs.SetString("reportTable", json);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString("reportTable"));
    }



    private List<ReportEntry> Sort(List<ReportEntry> reportEntryList, int criteria)
    {
        if (criteria == 0)
        {
            for (int i = 0; i < reportEntryList.Count; i++)
            {
                for (int j = i + 1; j < reportEntryList.Count; j++)
                {
                    if (string.Compare(reportEntryList[j].studentId, reportEntryList[i].studentId) < 0)
                    {
                        ReportEntry temp = reportEntryList[i];
                        reportEntryList[i] = reportEntryList[j];
                        reportEntryList[j] = temp;
                    }
                }
            }

        }
        else if (criteria == 1)
        {
            for (int i = 0; i < reportEntryList.Count; i++)
            {
                for (int j = i + 1; j < reportEntryList.Count; j++)
                {
                    if (reportEntryList[j].accuracy < reportEntryList[i].accuracy)
                    {
                        ReportEntry temp = reportEntryList[i];
                        reportEntryList[i] = reportEntryList[j];
                        reportEntryList[j] = temp;
                    }
                }
            }

        }
        else if (criteria == 2)
        {
            for (int i = 0; i < reportEntryList.Count; i++)
            {
                for (int j = i + 1; j < reportEntryList.Count; j++)
                {
                    if (reportEntryList[j].accuracy > reportEntryList[i].accuracy)
                    {
                        ReportEntry temp = reportEntryList[i];
                        reportEntryList[i] = reportEntryList[j];
                        reportEntryList[j] = temp;
                    }
                }
            }

        }
        else if (criteria == 3)
        {
            for (int i = 0; i < reportEntryList.Count; i++)
            {
                for (int j = i + 1; j < reportEntryList.Count; j++)
                {
                    if (string.Compare(reportEntryList[j].name, reportEntryList[i].name) < 0)
                    {
                        ReportEntry temp = reportEntryList[i];
                        reportEntryList[i] = reportEntryList[j];
                        reportEntryList[j] = temp;
                    }
                }
            }

        }
        return reportEntryList;
    }
}
