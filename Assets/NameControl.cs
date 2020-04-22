﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameControl : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI txt;

    [SerializeField] GameEvent events = null;

    // Start is called before the first frame update
    void Start()
    {
        string name = events.stuName;
        txt = GetComponent<TextMeshProUGUI>();
        txt.text = "[ "+name+" ]";
    }
}
