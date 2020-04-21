using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class LandCtrl : MonoBehaviour
{
    public static int standingOn = 3;//position of charac!!
    public static int worldId;
    public static string gameMode = "battle";
    [SerializeField] GameEvent events = null;//set for "Game{}"
    
    private int explored;//to be deleted

    private List<Land> Map;//
    
    [SerializeField]//set for "Land"
    private GameObject buttonTemplate;
    [SerializeField]//set for "MapButtons"
    private GridLayoutGroup gridGroup;
    [SerializeField]//set for "Image"
    private Sprite[] iconSprites;
    
    private int worldType = 0;//not synchronized for now
    private string worldCode;
    private int userId = 1;//not scynchronized for now
    private string landListStr;
    private Lands landList;
     

    // Start is called before the first frame update
    void Start()
    {
        //worldType = events.WorldType;
        worldCode = getWorldTypeStr(worldType);
        // url for getLandsByUserIdAndCategory/
        string url1 = "http://localhost:9090/api/world/getLandsByUserIdAndCategory/" + userId.ToString() + "/" + worldCode;
        Debug.Log("url1:" + url1);
        StartCoroutine(GetLandsRequest(url1));
               
    }
    void GenMap()
    {
        foreach (Land newLand in Map)
        {
            GameObject newButton = Instantiate(buttonTemplate) as GameObject;
            newButton.SetActive(true);

            newButton.GetComponent<LandBtn>().SetIcon(newLand.iconSprite);
            newButton.GetComponent<LandBtn>().SetText(newLand.owner);
            newButton.GetComponent<LandBtn>().SetNo(newLand.landNo);
            newButton.transform.SetParent(buttonTemplate.transform.parent, false);
        }
    }
    private string getWorldTypeStr(int num)
    {
        switch (num)
        {
            case 0:
                return "SE";
            case 1:
                return "SA";
            case 2:
                return "QA";
            case 3:
                return "PM";
            default:
                return "";

        }
    }
    private string getDifficultyStr(int num)
    {
        switch (num)
        {
            case 1:
                return "easy";
            case 2:
                return "medium";
            case 3:
                return "hard";
            default:
                return "";

        }
    }
    IEnumerator GetLandsRequest(string url)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log("Error: " + webRequest.error);
            yield break;
        }

        if (webRequest.downloadHandler.text == "")
        {
            Debug.Log("World has not been unlocked");
            yield break;
        }
        
        JSONNode rawJson = JSON.Parse(webRequest.downloadHandler.text);
        landListStr = "{\"listOfLands\":" + rawJson.ToString() + "}";
        Debug.Log("Land List:" + rawJson.ToString());

        landList = JsonUtility.FromJson<Lands>(landListStr);
        
        Map = new List<Land>();
        Debug.Log("count:"+landList.listOfLands.Count);
        foreach (LandInfo landInfo in landList.listOfLands)
        {
            Land newLand = new Land();
            newLand.landNo = landInfo.ind;
            if (landInfo.ownerId != 0)
            {
                newLand.iconSprite = iconSprites[0];//which is flower
                newLand.owner = landInfo.ownerName;
                //newLand.owner = getOwnerName(landInfo.ownerId);
                newLand.difficulty = getDifficultyStr(landInfo.ownerDifficulty);
            }
            else
            {
                newLand.iconSprite = iconSprites[1];//which is soil
                newLand.owner = landInfo.landId.ToString();
            }
            Map.Add(newLand);
        }
        GenMap();
    }

    public class Lands
    {
        public List<LandInfo> listOfLands;
    }

    [System.Serializable]
    public class LandInfo
    {
        public int landId;//1-24
        public int ind;//0-23
        public int worldId;
        public int ownerId;
        public string ownerName;
        public int ownerDifficultyLevel;
        public int ownerDifficulty;
        
        // Given JSON input:
        // {"name":"Dr Charles","lives":3,"health":0.8}
        // this example will return a PlayerInfo object with
        // name == "Dr Charles", lives == 3, and health == 0.8f.
    }
   
    public class Land
    {
        public Sprite iconSprite;
        public string owner;
        public int landNo;
        public string difficulty;
    }
    [System.Serializable]
    public class UserInfo
    {
        public string username;
        private string userAvatar;
        private string password;
        private string email;
        private string accountType;
        private bool isAdmin;
        private int overallExp;
        private int id;

        public static UserInfo CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<UserInfo>(jsonString);
        }
    }
}
