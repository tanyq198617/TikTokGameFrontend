using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using YooAsset;

public class LocalizationMgr : Singleton<LocalizationMgr>
{
    private List<TextLocalization> allTexts;
    private Dictionary<string, string> languageDic = null;
    private bool isInit = false;

    private void Init()
    {
        if (null == YooAssets.GetPackage(AssetConst.DefaultPackage))
        {
            Debug.LogError("请先初始化 YooAssets");
            return;
        }

        languageDic = new Dictionary<string, string>();
        isInit = true;
        LoadLanguage();
    }

    private void LoadLanguage()
    {
        languageDic.Clear();
        TextAsset file = YooAssets.LoadAssetSync<TextAsset>("zh_CN").GetAssetObject<TextAsset>();
        byte[] array = Encoding.UTF8.GetBytes(file.text);
        MemoryStream stream = new MemoryStream(array);
        StreamReader sr = new StreamReader(stream, Encoding.Default);
        Regex rex = new Regex(@"^(\w+)\s+(.+)");
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            Match m = rex.Match(line);
            if (m.Success)
            {
                string value = m.Groups[2].ToString().Replace("\\n", "\n");
                languageDic[m.Groups[1].ToString()] = value;
            }
        }
        sr.Close();
    }


    /// <summary>
    /// 将所有挂在ui预设上的组件加入列表
    /// </summary>
    public void AddText(TextLocalization lt)
    {
        if (allTexts == null)
        {
            allTexts = new List<TextLocalization>();
        }
        allTexts.Add(lt);
    }

    public string GetString(string key)
    {
        if (!isInit)
        {
            Init();
        }

        string ret = string.Empty;
        if (languageDic != null && languageDic.TryGetValue(key, out ret))
        {
            return ret;
        }

        return ret;
    }

    public string GetString(string key, params object[] args)
    {
        string ret = GetString(key);
        if (!string.IsNullOrEmpty(ret))
            ret = string.Format(ret, args);
        return ret;
    }
}
