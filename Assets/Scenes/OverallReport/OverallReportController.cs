using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class OverallReportController : MonoBehaviour
{
    private Transform tableContent;
    private OverallReport overallReport;

    [Serializable]
    private class OverallReport
    {
        public double overallAccuracy;
        public double sweAccuracy;
        public double saAccuracy;
        public double pmAccuracy;
        public double qaAccuracy;
        public double onlineTime;
    }

    private class JsonEntry
    {
        public OverallReport overallReport;
    }

    private void AddTestData()
    {
        OverallReport currentOverallReport = new OverallReport
        {
            overallAccuracy = 0.85,
            sweAccuracy = 0.95,
            saAccuracy = 0.95,
            pmAccuracy = 0.8,
            qaAccuracy = 0.8,
            onlineTime = 7.6
        };

        JsonEntry jsonEntry = new JsonEntry { overallReport = currentOverallReport};
        string jsonString = JsonUtility.ToJson(jsonEntry);

        PlayerPrefs.SetString("overallReport", jsonString);
        PlayerPrefs.Save();
        Debug.Log(jsonString);
    }

    void Start()
    {
        tableContent = transform.Find("TableContent");

        AddTestData();

        string jsonString = PlayerPrefs.GetString("overallReport");
        JsonEntry jsonEntry = JsonUtility.FromJson<JsonEntry>(jsonString);
        Debug.Log(jsonString);
        overallReport = jsonEntry.overallReport;

        tableContent.Find("overall").GetComponent<Text>().text = ((int)Math.Round(overallReport.overallAccuracy * 100)).ToString() + "%";
        tableContent.Find("swe").GetComponent<Text>().text = ((int)Math.Round(overallReport.sweAccuracy * 100)).ToString() + "%";
        tableContent.Find("sa").GetComponent<Text>().text = ((int)Math.Round(overallReport.saAccuracy * 100)).ToString() + "%";
        tableContent.Find("pm").GetComponent<Text>().text = ((int)Math.Round(overallReport.pmAccuracy * 100)).ToString() + "%";
        tableContent.Find("qa").GetComponent<Text>().text = ((int)Math.Round(overallReport.qaAccuracy * 100)).ToString() + "%";
        tableContent.Find("onlineTime").GetComponent<Text>().text = overallReport.onlineTime.ToString() + "h";

    }


    void Update()
    {
        
    }
}
