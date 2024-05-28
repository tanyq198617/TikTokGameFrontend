using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Z_HotfixTest : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;

    public void OnUpdateInfo(string v)
    {
        //textMeshProUGUI.text = v;
        Debug.LogError(v);
    }

    void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        Debug.LogError($"你好， Z_HotfixTest.");
        //textMeshProUGUI.text = $"这是一段新的代码热更内容.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
