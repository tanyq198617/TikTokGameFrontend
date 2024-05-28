using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;

[CustomEditor(typeof(TextLocalization))]
public class TextLocalizationEditor : Editor
{
    SerializedProperty key;
    TextLocalization m_Component;
    TextMeshProUGUI textMeshPro;

    private void OnEnable()
    {
        key = serializedObject.FindProperty("stringKey");
        m_Component = target as TextLocalization;
        if (textMeshPro == null)
            textMeshPro = m_Component.gameObject.GetComponent<TextMeshProUGUI>();
        if (textMeshPro == null)
            Debug.LogError("can not find text component", m_Component.gameObject);
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(key, new GUIContent("key"));
        serializedObject.ApplyModifiedProperties();
        if (GUILayout.Button("刷新"))
        {
            SetText();
        }
    }

    private void SetText()
    {
        if (textMeshPro == null)
            return;
        string textString = GetI18NString(m_Component.stringKey);
        textMeshPro.text = textString;
    }

    private Dictionary<string, string> stringMap = null;

    private string GetI18NString(string key)
    {
        if (stringMap == null)
        {
            stringMap = new Dictionary<string, string>();
            LoadStrings();
        }
        string ret = string.Empty;
        if (stringMap.TryGetValue(key, out ret))
        {
            return ret;
        }

        return ret;
    }

    private string filePath = "Assets/HotUpdateResources/TextAsset/Localization/zh-CN.txt";
    private string fileDir = "Assets/HotUpdateResources/TextAsset/Localization/";
    private FileSystemWatcher fsw = null;
    private void LoadStrings()
    {
        stringMap.Clear();
        string[] lines = File.ReadAllLines(filePath);
        Regex rex = new Regex(@"^(\w+)\s+(.+)");
        foreach (var line in lines)
        {
            Match m = rex.Match(line);
            if (m.Success)
            {
                string key = m.Groups[1].ToString();
                if (stringMap.ContainsKey(key))
                {
                    Debug.LogError("语言文件中存在相同的key = " + key + ", 请先修改！！！！");
                }
                string value = m.Groups[2].ToString();
                value = value.Replace("\\n", "\n");
                stringMap[key] = value;
            }
        }

        if (fsw == null)
        {
            fsw = new FileSystemWatcher();
            fsw.Path = fileDir;
            fsw.IncludeSubdirectories = false;
            fsw.Filter = "zh-CN.txt";
            fsw.EnableRaisingEvents = true;
            fsw.NotifyFilter = NotifyFilters.LastWrite;

            fsw.Changed += delegate (object sender, FileSystemEventArgs e)
            {
                LoadStrings();
            };
        }
    }
}
