using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LandBtn : MonoBehaviour
{
    //public DataManagerMap dataManager;

    private Button m_button;
    [SerializeField]
    private Image myIcon;

    private TextMeshProUGUI myText;
    private int landNo;
    public void SetIcon(Sprite mySprite)
    {
        myIcon.sprite = mySprite;

    }
    public void SetText(string ownername)
    {
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myText.text = ownername;

    }
    public void SetNo(int number)
    {
        landNo = number;
        //Debug.Log("Land creation: " + landNo);
    }
    public void SetClicked()
    {
        LandCtrl.standingOn = landNo;
        CharacCtrl.ChangePosition(landNo);
        Debug.Log("You clicked on: "+ landNo);
    }
    void Start()
    {
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(() =>SetClicked());
    }
}
