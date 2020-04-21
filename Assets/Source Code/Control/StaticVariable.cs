using UnityEngine;

public class StaticVariable : MonoBehaviour
{
    //keeping track of previous scene
    //mainly used for profile
    //as page can be accessed through both mode & leaderboard
    static public string scene = "Mode";
    static public string nameStudent = "Angel";
    static public bool isStudent = true;
    static public string questionId;
    static public string studentId = "1";
    static public string reportId = "1";
    static public string leaderboardId;
    static public bool isFromReportList;
    static public bool isFromLeaderboard;
    //defalut Mode Scene
}
