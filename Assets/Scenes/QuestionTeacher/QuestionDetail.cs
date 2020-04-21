using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionDetail : MonoBehaviour
{
    
    private Transform questionHeader;
    private Transform description;

    private Transform optionA;
    private Transform optionB;
    private Transform optionC;
    private Transform optionD;
    private Transform tick;
    private Transform accuracy;

    private Question question;

    [System.Serializable]
    private class Question
    {
        public string questionId;
        public string category;
        public string questionDescription;
        public string correctAnswer;
        public string optionA;
        public string optionB;
        public string optionC;
        public string optionD;
        public double correctRate;
    }

    private class JsonEntry
    {
        public Question question;
    }

    public void Start()
    {
        questionHeader = transform.Find("LeftSide").Find("QuestionHeader");
        description = transform.Find("LeftSide").Find("ScrollArea").Find("TextContainer").Find("Description");
        optionA = transform.Find("RightSide").Find("optionA");
        optionB = transform.Find("RightSide").Find("optionB");
        optionC = transform.Find("RightSide").Find("optionC");
        optionD = transform.Find("RightSide").Find("optionD");
        tick = transform.Find("RightSide").Find("tick");
        accuracy = transform.Find("RightSide").Find("correctRate");

        // initialize if run for first time to create the json file
        AddTestData();

        // construct object from json file
        string jsonString = PlayerPrefs.GetString("questionDetails");
        JsonEntry jsonEntry = JsonUtility.FromJson<JsonEntry>(jsonString);
        Debug.Log(jsonString);
        question = jsonEntry.question;
        

        SetText();
        SetTickPosition();

    }

    private void SetText()
    {

        questionHeader.Find("id").GetComponent<Text>().text = question.questionId;
        questionHeader.Find("category").GetComponent<Text>().text = question.category;

        description.GetComponent<Text>().text = question.questionDescription;
        
        optionA.GetComponent<Text>().text = question.optionA;
        optionB.GetComponent<Text>().text = question.optionB;
        optionC.GetComponent<Text>().text = question.optionC;
        optionD.GetComponent<Text>().text = question.optionD;

        Debug.Log(question.correctRate * 100);
        accuracy.GetComponent<Text>().text = "Correct Rate: " + ((int)System.Math.Round(question.correctRate * 100)).ToString() + "%";
    }

    private void SetTickPosition()
    {
        RectTransform optionRectTransform = new RectTransform();
        
        if (question.correctAnswer == "A") optionRectTransform = transform.Find("Static").Find("A").GetComponent<RectTransform>();
        else if (question.correctAnswer == "B") optionRectTransform = transform.Find("Static").Find("B").GetComponent<RectTransform>();
        else if (question.correctAnswer == "C") optionRectTransform = transform.Find("Static").Find("C").GetComponent<RectTransform>();
        else if (question.correctAnswer == "D") optionRectTransform = transform.Find("Static").Find("D").GetComponent<RectTransform>();

        tick.GetComponent<RectTransform>().anchoredPosition = new Vector2(optionRectTransform.anchoredPosition.x - 50, optionRectTransform.anchoredPosition.y);
    }

    public void AddTestData()
    {
        Question currentQuestion = new Question
        {
            questionId = StaticVariable.questionId,
            category = "SWE",
            questionDescription = "I have a simple question , how can I delete a cloned or instantiated object after 1 second without deleting the original. The GameObject is just a 3d object with no scripts attached. All I want to do is instantiate an object at certain coordinates when required and destroy it after 1 second. Any help will be appreciated",
            correctAnswer = "C",
            optionA = "Choose me choose me",
            optionB = "Don't listen to him",
            optionC = "Hello don't ignore me",
            optionD = "I am the best!",
            correctRate = 0.78f
        };

        JsonEntry jsonEntry = new JsonEntry { question = currentQuestion };
        string json_question = JsonUtility.ToJson(jsonEntry);

        PlayerPrefs.SetString("questionDetails", json_question);
        PlayerPrefs.Save();
        Debug.Log(json_question);
    }
}
