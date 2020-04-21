﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Character
{
    public int charId;
    public int userId;
    public string charName;
    public int exp;
    public int level;
    public int attackPt;
    public int correctQuesNo;
    public int totalQuesNo;
}


public class DataLoaded
{
    public Character character;
    public int combatId;
    public List<QuestionLoad> questions;

}

public class Qqustion
{
    public List<QuestionLoad> ListOfQuestions;

}
[System.Serializable]
public class QuestionLoad
{
    public string category;
    public string problem;
    public string choice1;
    public string choice2;
    public string choice3;
    public string choice4;
    public int answer;
    public int difficultyLevel;
    public int userAnswered;
    public int userCorrect;
    public int id;

}