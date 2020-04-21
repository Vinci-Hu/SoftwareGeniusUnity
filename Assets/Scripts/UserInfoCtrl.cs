using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoCtrl : MonoBehaviour
{
    private List<UserInfo> UserList;

    [SerializeField]//set for "UserInfo"
    private GameObject buttonTemplate;
    [SerializeField]//set for "Content"
    private GridLayoutGroup gridGroup;

    [SerializeField]//set for "Avatar"'s sprites
    private Sprite[] avatars;
    [SerializeField]
    private string[] usernames;

    void Start()
    {
        UserList = new List<UserInfo>();

        for (int i = 0; i < 30; i++)
        {
            UserInfo newUser = new UserInfo();
            newUser.iconSprite = avatars[Random.Range(0, avatars.Length)];
            newUser.userName = usernames[Random.Range(0, usernames.Length)];

            UserList.Add(newUser);
        }
        GenList();
    }

    void GenList()
    {
        if (UserList.Count < 6)
        {
            gridGroup.constraintCount = UserList.Count;
        }
        else
        {
            gridGroup.constraintCount = 5;
        }
        foreach (UserInfo newUser in UserList)
        {
            GameObject newButton = Instantiate(buttonTemplate) as GameObject;
            newButton.SetActive(true);

            newButton.GetComponent<UserInfoBtn>().SetAvatar(newUser.iconSprite);
            newButton.GetComponent<UserInfoBtn>().SetName(newUser.userName);
            newButton.transform.SetParent(buttonTemplate.transform.parent, false);
        }
    }
    public class UserInfo
    {
        public Sprite iconSprite;
        public string userName;
    }
}
