using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoBtn : MonoBehaviour
{
    [SerializeField]
    private Image myAvatar;
    [SerializeField]
    private Text myUserName;

    public int userId;
    public void SetAvatar(Sprite mySprite)
    {
        myAvatar.sprite = mySprite;

    }
    public void SetName(string username)
    {
        myUserName.text = username;

    }
    public void SetId(int id)
    {
        userId = id;
    }
}
