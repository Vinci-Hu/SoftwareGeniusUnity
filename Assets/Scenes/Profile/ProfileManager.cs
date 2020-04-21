using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ProfileManager : MonoBehaviour
{
    private Transform student;
    private Transform bars;
    private Transform barValues;
    private Transform tableContent;

    private Profile profile;

    [Serializable]
    private class Profile
    {
        public string studentId;
        public string username;
        public string realName;
        public List<int> expPoints;
        public List<int> damagePoints;
        public List<int> hitPoints;
        public List<double> accuracy;
    }

    private class JsonEntry
    {
        public Profile profile;
    }

    private void AddTestData()
    {
        Profile currentProfile = new Profile
        {
            studentId = "1",
            username = "Yostas Specter",
            realName = "Duan Yiting",
            expPoints = new List<int>() { 55, 46, 99, 67, 10 },
            damagePoints = new List<int>() { 40, 41, 42, 43, 44 },
            hitPoints = new List<int>() { 60, 61, 62, 63, 64 },
            accuracy = new List<double>() { 0.78, 0.87, 0.99, 0.91, 0.90}
        };

        JsonEntry jsonEntry = new JsonEntry { profile = currentProfile };
        string jsonString = JsonUtility.ToJson(jsonEntry);

        PlayerPrefs.SetString("profile", jsonString);
        PlayerPrefs.Save();
        Debug.Log(jsonString);
    }

    public void Start()
    {
        student = transform.Find("Student");
        bars = transform.Find("BarChart").Find("Bars");
        barValues = transform.Find("BarChart").Find("BarValues");
        tableContent = transform.Find("BarChart").Find("TableContent");

        // uncomment this if running for first time only!!!
        // AddTestData();

        string jsonString = PlayerPrefs.GetString("profile");
        JsonEntry jsonEntry = JsonUtility.FromJson<JsonEntry>(jsonString);
        Debug.Log(jsonString);
        profile = jsonEntry.profile;

        SetStudent();
        SetTable("Rect1");
        SetBarValues();
        SetBarHeights();
        SetValuePositions();
    }

    private void SetStudent()
    {
        student.Find("StudentName").GetComponent<Text>().text = profile.realName;
        student.Find("Username").GetComponent<Text>().text = profile.username;
        student.Find("Initials").GetComponent<Text>().text = GetInitials();
        student.Find("Avatar").GetComponent<Image>().color = SetColor();

    }

    private void SetTable(string choice)
    {
        // tableContent.Find("Accuracy").GetComponent<Text>().text = ((int)System.Math.Round(profile.accuracy * 100)).ToString() + "%";
        if (choice == "Rect0" || choice == "all")
        {
            tableContent.Find("Role").GetComponent<Text>().text = "Overall";
            tableContent.Find("Level").GetComponent<Text>().text = CalculateLevel(profile.expPoints[0]).ToString();
            tableContent.Find("Exp").GetComponent<Text>().text = profile.expPoints[0].ToString();
            tableContent.Find("Damage").GetComponent<Text>().text = profile.damagePoints[0].ToString();
            tableContent.Find("Hp").GetComponent<Text>().text = profile.hitPoints[0].ToString();
            tableContent.Find("Accuracy").GetComponent<Text>().text = ((int)System.Math.Round(profile.accuracy[0] * 100)).ToString() + "%";
        }
        else if (choice == "Rect1" || choice == "char0")
        {
            tableContent.Find("Role").GetComponent<Text>().text = "Software Engineering";
            tableContent.Find("Level").GetComponent<Text>().text = CalculateLevel(profile.expPoints[1]).ToString();
            tableContent.Find("Exp").GetComponent<Text>().text = profile.expPoints[1].ToString();
            tableContent.Find("Damage").GetComponent<Text>().text = profile.damagePoints[1].ToString();
            tableContent.Find("Hp").GetComponent<Text>().text = profile.hitPoints[1].ToString();
            tableContent.Find("Accuracy").GetComponent<Text>().text = ((int)System.Math.Round(profile.accuracy[1] * 100)).ToString() + "%";

        }
        else if (choice == "Rect2" || choice == "char1")
        {
            tableContent.Find("Role").GetComponent<Text>().text = "Software Architecture";
            tableContent.Find("Level").GetComponent<Text>().text = CalculateLevel(profile.expPoints[2]).ToString();
            tableContent.Find("Exp").GetComponent<Text>().text = profile.expPoints[2].ToString();
            tableContent.Find("Damage").GetComponent<Text>().text = profile.damagePoints[2].ToString();
            tableContent.Find("Hp").GetComponent<Text>().text = profile.hitPoints[2].ToString();
            tableContent.Find("Accuracy").GetComponent<Text>().text = ((int)System.Math.Round(profile.accuracy[2] * 100)).ToString() + "%";

        }
        else if (choice == "Rect3" || choice == "char2")
        {
            tableContent.Find("Role").GetComponent<Text>().text = "Product Management";
            tableContent.Find("Level").GetComponent<Text>().text = CalculateLevel(profile.expPoints[3]).ToString();
            tableContent.Find("Exp").GetComponent<Text>().text = profile.expPoints[3].ToString();
            tableContent.Find("Damage").GetComponent<Text>().text = profile.damagePoints[3].ToString();
            tableContent.Find("Hp").GetComponent<Text>().text = profile.hitPoints[3].ToString();
            tableContent.Find("Accuracy").GetComponent<Text>().text = ((int)System.Math.Round(profile.accuracy[3] * 100)).ToString() + "%";

        }
        else if (choice == "Rect4" || choice == "char3")
        {
            tableContent.Find("Role").GetComponent<Text>().text = "Quality Assurance";
            tableContent.Find("Level").GetComponent<Text>().text = CalculateLevel(profile.expPoints[4]).ToString();
            tableContent.Find("Exp").GetComponent<Text>().text = profile.expPoints[4].ToString();
            tableContent.Find("Damage").GetComponent<Text>().text = profile.damagePoints[4].ToString();
            tableContent.Find("Hp").GetComponent<Text>().text = profile.hitPoints[4].ToString();
            tableContent.Find("Accuracy").GetComponent<Text>().text = ((int)System.Math.Round(profile.accuracy[4] * 100)).ToString() + "%";

        }
    }

    private void SetBarValues()
    {
        barValues.Find("Exp0").GetComponent<Text>().text = profile.expPoints[0].ToString();
        barValues.Find("Exp1").GetComponent<Text>().text = profile.expPoints[1].ToString();
        barValues.Find("Exp2").GetComponent<Text>().text = profile.expPoints[2].ToString();
        barValues.Find("Exp3").GetComponent<Text>().text = profile.expPoints[3].ToString();
        barValues.Find("Exp4").GetComponent<Text>().text = profile.expPoints[4].ToString();
    }

    private void SetBarHeights()
    {
        bars.Find("Rect0").GetComponent<RectTransform>().sizeDelta = new Vector2(profile.expPoints[0], bars.Find("Rect0").GetComponent<RectTransform>().rect.width);
        bars.Find("Rect1").GetComponent<RectTransform>().sizeDelta = new Vector2(profile.expPoints[1], bars.Find("Rect1").GetComponent<RectTransform>().rect.width);
        bars.Find("Rect2").GetComponent<RectTransform>().sizeDelta = new Vector2(profile.expPoints[2], bars.Find("Rect2").GetComponent<RectTransform>().rect.width);
        bars.Find("Rect3").GetComponent<RectTransform>().sizeDelta = new Vector2(profile.expPoints[3], bars.Find("Rect3").GetComponent<RectTransform>().rect.width);
        bars.Find("Rect4").GetComponent<RectTransform>().sizeDelta = new Vector2(profile.expPoints[4], bars.Find("Rect4").GetComponent<RectTransform>().rect.width);

    }

    private void SetValuePositions()
    {
        barValues.Find("Exp0").GetComponent<RectTransform>().anchoredPosition = new Vector2(bars.Find("Rect0").GetComponent<RectTransform>().anchoredPosition.x, bars.Find("Rect0").GetComponent<RectTransform>().rect.width-85);
        barValues.Find("Exp1").GetComponent<RectTransform>().anchoredPosition = new Vector2(bars.Find("Rect1").GetComponent<RectTransform>().anchoredPosition.x, bars.Find("Rect1").GetComponent<RectTransform>().rect.width-85);
        barValues.Find("Exp2").GetComponent<RectTransform>().anchoredPosition = new Vector2(bars.Find("Rect2").GetComponent<RectTransform>().anchoredPosition.x, bars.Find("Rect2").GetComponent<RectTransform>().rect.width-85);
        barValues.Find("Exp3").GetComponent<RectTransform>().anchoredPosition = new Vector2(bars.Find("Rect3").GetComponent<RectTransform>().anchoredPosition.x, bars.Find("Rect3").GetComponent<RectTransform>().rect.width-85);
        barValues.Find("Exp4").GetComponent<RectTransform>().anchoredPosition = new Vector2(bars.Find("Rect4").GetComponent<RectTransform>().anchoredPosition.x, bars.Find("Rect4").GetComponent<RectTransform>().rect.width-85);

    }

    public void OnClick()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.transform.name);
        SetTable(EventSystem.current.currentSelectedGameObject.transform.name);
    }

    public void OnclickCharacters()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.transform.name);
        SetTable(EventSystem.current.currentSelectedGameObject.transform.name);
    }

    private int CalculateLevel (int exp)
    {
        return exp / 10;
    }

    private string GetInitials()
    {
        string name = profile.realName;
        string[] words = name.Split(' ');
        string res = "";

        for (int i = 0; i < words.Length; i++)
        {
            res += words[i][0];
        }
        return res.ToUpper();
    }

    private Color32 SetColor()
    {
        List<Color32> colors = new List<Color32>();
        Color32 color;

        color = new Color32(130, 190, 225, 200);
        colors.Add(color);
        color = new Color32(176, 191, 26, 200);
        colors.Add(color);
        color = new Color32(255, 191, 0, 200);
        colors.Add(color);
        color = new Color32(147, 112, 219, 200);
        colors.Add(color);
        color = new Color32(229, 43, 80, 200);
        colors.Add(color);


        return colors[Int32.Parse(profile.studentId) % 5];

    }

}
