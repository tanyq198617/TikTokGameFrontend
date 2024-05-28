using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Text.RegularExpressions;
using GameNetwork;
using Random = System.Random;

public class GameKeyName
{
    /// <summary>
    /// 并行符号串
    /// </summary>
    public readonly static char[] ParallelSuffix = { '|' };

    //剧情对话专用
    public readonly static char[] PlotSuffix = { '|', '_' };

    /// <summary>
    /// 串行符号串
    /// </summary>
    public readonly static char[] Serialsuffix = { ':', '^', ',', '_' };
    /// <summary>
    /// 剧情串行符号
    /// </summary>
    public readonly static char[] StorySerialsuffix = { ':', '^' };
    public const string KEYNAME_RANK = "*";
    /// <summary>
    /// 区间分隔符
    /// </summary>
    public readonly static char[] SectionSuffix = { '*' };

    public readonly static char[] Record_One = { '|' };
    public readonly static char[] Record_Two = { '&' };
    public readonly static char[] Record_Three = { '$' };
    public readonly static char[] Record_Four = { '*' };
    public readonly static char[] Record_Five = { '_' };
    public readonly static char[] Record_Six = { '^' };
    public readonly static char[] Record_Seven = { '%' };
    public readonly static char[] Record_Eight = { '+' };
    public readonly static char[] Record_Nine = { '#' };
    public readonly static char[] Record_Ten = { ':' };

    public readonly static char[] Slash = { '/' };
    public readonly static char[] Comma = { ',' };

    public readonly static char[] Record_Time = { '-', '_', ':' };
    public readonly static char[] Record_TimeCfg = { '-', ' ', ':', ',' };
}

public static class StringUtility
{
    public static StringBuilder stringBuilder;

    private static void ResetSbuilder()
    {
        if (stringBuilder == null)
            stringBuilder = new StringBuilder();
        else
            stringBuilder.Clear();
    }

    public static int GetRandomInDic(Dictionary<int, int> dic)
    {
        int randomNum;
        int totalWeight = 0;
        int tmpWeight = 0;
        foreach (int value in dic.Values)
            totalWeight += value;
        randomNum = UnityEngine.Random.Range(0, totalWeight);
        foreach (int key in dic.Keys)
        {
            //Debuger.Log("secAttrId: " + key);
            if (randomNum >= tmpWeight && randomNum < (tmpWeight + dic[key]))
                return key;
            tmpWeight += dic[key];

        }
        return -1;
    }

    public static int GetStrIndex(string str, string proSuffix)
    {
        if (string.IsNullOrEmpty(str))
            return -1;

        int posIndex = str.LastIndexOf(proSuffix);
        return int.Parse(str.Substring(posIndex + 1, str.Length - posIndex - 1));
    }


    public static string ReplaceKey(string str, string[] param, string rgb = "febffd")
    {
        string temp = "";
        temp = str.Replace("{", "[" + rgb + "]{");
        temp = temp.Replace("}", "}[-]");
        return string.Format(temp, param);
    }

    /// <summary>
    /// 分割字符串，通过特定字符串(如："[br]")来分离得到字符中的名称和内容
    /// 如：“XX：[br]XX11[br]XX”
    /// </summary>
    public static void DivisionStr(string Source, string flag, out string title, out string content)
    {
        title = "";
        content = "";
        if (!string.IsNullOrEmpty(Source))
        {
            string[] sArry = Source.Split(new string[] { flag }, StringSplitOptions.RemoveEmptyEntries);
            if (sArry.Length == 1)
            {
                content = sArry[0];
            }
            else
            {
                for (int i = 0; i < sArry.Length; ++i)
                {
                    title = sArry[0];
                    if (i > 0)
                    {
                        content = content + sArry[i];
                    }
                }
            }
        }
    }

    #region 分割字符串

    /// <summary>
    /// 形如 xx_xx_xx|zz_zz_zz...
    /// </summary>
    /// <param name="context"></param>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static List<List<int>> StringToLists(string context, char[] first = null, char[] second = null)
    {
        context = context == null ? "" : context;
        first = first == null ? GameKeyName.ParallelSuffix : first;
        second = second == null ? GameKeyName.Serialsuffix : second;
        List<List<int>> curList = new List<List<int>>();
        string[] curArry = context.Split(first);
        List<int> temp;
        if (curArry.Length > 0)
        {
            for (int i = 0; i < curArry.Length; i++)
            {
                temp = StringToList(curArry[i], second);
                if (temp != null && temp.Count > 0)
                    curList.Add(temp);
            }
        }
        return curList;
    }

    public static int[][] StringToNN(string context, char[] first = null, char[] second = null)
    {
        context = context == null ? "" : context;
        first = first == null ? GameKeyName.ParallelSuffix : first;
        second = second == null ? GameKeyName.Serialsuffix : second;
        string[] curArry = context.Split(first);
        int[][] curList = new int[curArry.Length][];
        List<int> temp;
        if (curArry.Length > 0)
        {
            for (int i = 0; i < curArry.Length; i++)
            {
                temp = StrToListInt(curArry[i], second);

                curList[i] = new int[temp.Count];

                if (temp != null && temp.Count > 0)
                {
                    for (int j = 0; j < temp.Count; j++)
                    {
                        curList[i][j] = temp[j];
                    }
                }
            }
        }
        return curList;
    }

    /// <summary>
    ///  形如 xx_xx_xx|zz_zz_zz...
    /// </summary>
    /// <param name="context"></param>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static List<string[]> StringToListSs(string context, char[] first = null, char[] second = null)
    {
        context = context == null ? "" : context;
        first = first == null ? GameKeyName.ParallelSuffix : first;
        second = second == null ? GameKeyName.Serialsuffix : second;
        List<string[]> curList = new List<string[]>();
        string[] curArry = context.Split(first);
        if (curArry.Length > 0)
        {
            for (int i = 0; i < curArry.Length; i++)
            {
                string[] temp = curArry[i].Split(second);
                if (temp != null && temp.Length > 0)
                    curList.Add(temp);
            }
        }
        return curList;
    }

    /// <summary>
    /// 形如 A|xx_xx#B|xx_xx
    /// 1|1301001_10#2|1301001_40#3|xx_xx...
    /// </summary>
    /// <param name="context"></param>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static Dictionary<int, List<int>> StringToDictThree(string context, char[] first = null, char[] second = null, char[] three = null)
    {
        context = context == null ? "" : context;
        Dictionary<int, List<int>> dict = new Dictionary<int, List<int>>();
        first = first == null ? GameKeyName.Record_Nine : first;
        second = second == null ? GameKeyName.Record_One : second;
        three = three == null ? GameKeyName.Record_Five : three;
        string[] curArry = context.Split(first);
        List<int> temp;
        if (curArry.Length > 0)
        {
            for (int i = 0; i < curArry.Length; i++)
            {
                string[] item = curArry[i].Split(second);
                int key = 0;
                int.TryParse(item[0], out key);
                if (item.Length > 1)
                {
                    temp = StringToList(item[1], three);
                    dict.Add(key, temp);
                }
            }
        }
        return dict;
    }

    /// <summary>
    /// 形如 A|xx_xx:xx_xx#B|xx_xx:xx_xx...
    /// 1|1_10:2_10 # 2|1_40:2_10 # 3|xx_xx...
    /// </summary>
    /// <param name="context"></param>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static Dictionary<int, Dictionary<int, int>> StringToDictFour(string context, char[] first = null, char[] second = null, char[] three = null, char[] four = null)
    {
        context = context == null ? "" : context;
        Dictionary<int, Dictionary<int, int>> dict = new Dictionary<int, Dictionary<int, int>>();
        first = first == null ? GameKeyName.Record_Nine : first;// "#"
        second = second == null ? GameKeyName.Record_One : second;// "|"
        three = three == null ? GameKeyName.Record_Ten : three;// ":"
        four = four == null ? GameKeyName.Record_Five : four;//  "_"
        string[] curArry = context.Split(first);
        //List<int> temp;
        if (curArry.Length > 0)
        {
            for (int i = 0; i < curArry.Length; i++)
            {
                string[] item = curArry[i].Split(second);
                int key = 0;
                int.TryParse(item[0], out key);
                Dictionary<int, int> tdict = new Dictionary<int, int>();
                if (item.Length > 1)
                {
                    tdict = StringToDicNN(item[1], three, four);
                }
                dict.Add(key, tdict);
            }
        }
        return dict;
    }


    public static List<string> StringToListStr(string context, char[] way)
    {
        if (string.IsNullOrEmpty(context)) return null;
        string[] arr = context.Split(way);
        List<string> curList = new List<string>();
        for (int i = 0; i < arr.Length; i++)
            curList.Add(arr[i]);
        return curList;
    }

    // x_y|x_y
    public static List<Dictionary<int, int>> StringToListDict(string context, char[] one = null, char[] two = null)
    {
        if (string.IsNullOrEmpty(context))
            return new List<Dictionary<int, int>>();
        one = one == null ? GameKeyName.Record_One : one;// "|"
        two = two == null ? GameKeyName.Record_Five : two;//  "_"
        string[] arr = context.Split(one);
        List<Dictionary<int, int>> curList = new List<Dictionary<int, int>>();
        int id = 0, num;
        for (int i = 0; i < arr.Length; i++)
        {
            string[] str = arr[i].Split(two);
            //x_y
            int.TryParse(str[0], out id);
            int.TryParse(str[1], out num);
            Dictionary<int, int> dict = new Dictionary<int, int>
            {
                { id, num }
            };
            curList.Add(dict);
        }
        return curList;
    }

    public static List<int> StringToList(string context)
    {
        return StringToList(context, GameKeyName.PlotSuffix);
    }

    public static List<int> StringToList(string context, char[] way)
    {
        if (string.IsNullOrEmpty(context)) return new List<int>();
        string[] arr = context.Split(way);
        List<int> curList = new List<int>();
        int id = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            int.TryParse(arr[i], out id);
            curList.Add(id);
        }
        return curList;
    }

    public static List<float> StringToListF(string context, char[] way)
    {
        if (string.IsNullOrEmpty(context)) return null;
        string[] arr = context.Split(way);
        List<float> curList = new List<float>();
        float id = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            float.TryParse(arr[i], out id);
            curList.Add(id);
        }
        return curList;
    }

    public static Dictionary<string, string> StringToDicSS(string context, char[] way_1, char[] way_2)
    {
        if (string.IsNullOrEmpty(context)) return null;
        string[] arr = context.Split(way_1);

        Dictionary<string, string> dic = new Dictionary<string, string>();
        List<string> curList = null;
        foreach (string curStr in arr)
        {
            curList = StringToListStr(curStr, way_2);
            if (dic.ContainsKey(curList[0]) || curList.Count < 2)
                Debug.LogWarning("字符中有同样id = " + curList[0]);
            else
                dic.Add(curList[0], curList[1]);
        }
        return dic;
    }

    public static Dictionary<string, float> StringToDicSF(string context, char[] way_1, char[] way_2)
    {
        if (string.IsNullOrEmpty(context)) return null;
        string[] arr = context.Split(way_1);

        Dictionary<string, float> dic = new Dictionary<string, float>();
        List<string> curList = null;
        float value = 0;
        foreach (string curStr in arr)
        {
            curList = StringToListStr(curStr, way_2);
            if (dic.ContainsKey(curList[0]) || curList.Count < 2)
                Debug.LogWarning("字符中有同样id = " + curList[0]);
            else
            {
                value = 0;
                float.TryParse(curList[1], out value);
                dic.Add(curList[0], value);
            }
        }
        return dic;
    }

    /// <summary>
    /// 字符串转字典<int,string>
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static Dictionary<int, string> StringToDicIS(string context)
    {
        if (string.IsNullOrEmpty(context)) return null;
        string[] arr = context.Split(GameKeyName.ParallelSuffix);

        Dictionary<int, string> dic = new Dictionary<int, string>();
        List<int> curList = null;
        foreach (string curStr in arr)
        {
            curList = StringToList(curStr, GameKeyName.Serialsuffix);

            if (dic.ContainsKey(curList[0]))
                Debug.LogWarning("字符中有同样id = " + curList[0]);
            else
                dic.Add(curList[0], curStr);
        }
        return dic;
    }
    /*
    public static Dictionary<int, int> StringToDic(string context)
    {
        if (string.IsNullOrEmpty(context)) return null;
        string[] arr = context.Split(GameKeyName.ParallelSuffix);

        Dictionary<int, int> dic = new Dictionary<int, int>();
        List<int> curList = null;
        foreach (string curStr in arr)
        {
            curList = StringToList(curStr, GameKeyName.Serialsuffix);

            if (dic.ContainsKey(curList[0]))
                Debug.LogWarning("字符中有同样id = " + curList[0]);
            else
            {
                if(curList.Count == 2)
                    dic.Add(curList[0], curList[1]);
                else 
                    Debug.LogWarning("")
            }

        }
        return dic;
    }
    */

    /// <summary>
    /// 逐渐增大                                                                                                                                                                                                                                                                                                                                                                        
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static int SortAscending(Transform a, Transform b)
    {
        if (a.name.Equals(b.name)) return 0;
        int a_index = int.Parse(a.name);
        int b_index = int.Parse(b.name);
        return a_index > b_index ? 1 : -1;
    }

    /// <summary>
    /// 逐渐减小
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static int SortDown(Transform a, Transform b)
    {
        if (a.name.Equals(b.name)) return 0;
        int a_index = int.Parse(a.name);
        int b_index = int.Parse(b.name);
        return a_index > b_index ? -1 : 1;
    }

    /// <summary>
    /// 字符串转字典<int,List<int>>
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static Dictionary<int, List<int>> StringToDic(string context)
    {
        if (string.IsNullOrEmpty(context)) return null;
        string[] arr = context.Split(GameKeyName.ParallelSuffix);

        Dictionary<int, List<int>> dic = new Dictionary<int, List<int>>();
        List<int> curList = null;
        foreach (string curStr in arr)
        {
            curList = StringToList(curStr, GameKeyName.Serialsuffix);

            if (dic.ContainsKey(curList[0]))
                Debug.LogWarning("字符中有同样id = " + curList[0]);
            else
                dic.Add(curList[0], curList);
        }
        return dic;
    }

    /// <summary>
    /// 字符串转字典<int,List<int>>
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static Dictionary<int, List<int>> StringToDic2(string context)
    {
        if (string.IsNullOrEmpty(context)) return null;
        string[] arr = context.Split(GameKeyName.ParallelSuffix);

        Dictionary<int, List<int>> dic = new Dictionary<int, List<int>>();
        List<int> curList = null;
        foreach (string curStr in arr)
        {
            curList = StringToList(curStr, GameKeyName.Serialsuffix);

            if (dic.ContainsKey(curList[0]))
                Debug.LogWarning("字符中有同样id = " + curList[0]);
            else
                dic.Add(curList[0], curList);
        }
        return dic;
    }

    /// <summary>
    /// 字符串转字典 格式 A_B,C|A_B,C
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static Dictionary<string, object[]> StringToDicObjectArr(string context)
    {
        if (string.IsNullOrEmpty(context)) return null;
        string[] arr = context.Split(GameKeyName.ParallelSuffix);

        Dictionary<string, object[]> dic = new Dictionary<string, object[]>();
        string key = string.Empty;
        object[] objects = null;
        foreach (string curStr in arr)
        {
            string[] val = curStr.Split(GameKeyName.Record_Ten);
            objects = null;
            if (val.Length > 1)
                objects = val[1].Split(GameKeyName.Comma);
            dic.Add(val[0], objects);
        }
        return dic;
    }

    /// <summary>
    /// 字符串转字典<int,int>
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static Dictionary<int, int> StringToDicNN(string context)
    {
        return StringToDicNN(context, GameKeyName.ParallelSuffix, GameKeyName.Serialsuffix);
    }

    public static void CombineDic(ref Dictionary<int, int> targetDic, Dictionary<int, int> decDic)
    {
        if (targetDic == null || decDic == null || decDic.Count == 0) return;
        foreach (KeyValuePair<int, int> curDic in decDic)
        {
            if (targetDic.ContainsKey(curDic.Key))
                targetDic[curDic.Key] = curDic.Value;
            else
                targetDic.Add(curDic.Key, curDic.Value);
        }
    }

    public static Dictionary<int, long> StringToDicNL(string context)
    {
        return StringToDicNL(context, GameKeyName.ParallelSuffix, GameKeyName.Serialsuffix);
    }

    public static Dictionary<long, int> StringToDicLN(string context)
    {
        return StringToDicLN(context, GameKeyName.ParallelSuffix, GameKeyName.Serialsuffix);
    }

    public static Dictionary<int, float> StringToDicNF(string context)
    {
        return StringToDicNF(context, GameKeyName.ParallelSuffix, GameKeyName.Serialsuffix);
    }

    public static Dictionary<float, int> StringToDicFN(string context, char[] curPara, char[] curSer)
    {
        if (string.IsNullOrEmpty(context)) return null;
        string[] arr = context.Split(curPara);
        Dictionary<float, int> dic = new Dictionary<float, int>();
        List<string> curList = null;
        float key = 0;
        int value = 0;
        foreach (string curStr in arr)
        {
            curList = StringToListStr(curStr, GameKeyName.Serialsuffix);
            if (curList == null || curList.Count < 2) continue;
            key = float.Parse(curList[0]);
            value = int.Parse(curList[1]);

            if (dic.ContainsKey(key))
                Debug.LogWarning("字符中有同样id = " + key);
            else
                dic.Add(key, value);
        }
        return dic;
    }

    public static Dictionary<int, float> StringToDicNF(string context, char[] curPara, char[] curSer)
    {
        if (string.IsNullOrEmpty(context)) return null;
        string[] arr = context.Split(curPara);
        Dictionary<int, float> dic = new Dictionary<int, float>();
        List<string> curList = null;
        int key = 0;
        float value = 0;
        foreach (string curStr in arr)
        {
            curList = StringToListStr(curStr, GameKeyName.Serialsuffix);
            if (curList == null || curList.Count < 2) continue;
            key = int.Parse(curList[0]);
            value = float.Parse(curList[1]);

            if (dic.ContainsKey(key))
                Debug.LogWarning("字符中有同样id = " + key);
            else
                dic.Add(key, value);
        }
        return dic;
    }

    public static Dictionary<int, int> StringToDicNN(string context, char[] curPara, char[] curSer)
    {
        if (string.IsNullOrEmpty(context)) return new Dictionary<int, int>();
        string[] arr = context.Split(curPara);
        Dictionary<int, int> dic = new Dictionary<int, int>();
        List<int> curList = null;

        for (int i = 0; i < arr.Length; i++)
        {
            curList = StringToList(arr[i], curSer);
            if (curList.Count == 2)
            {
                if (dic.ContainsKey(curList[0]))
                    Debug.LogWarning("字符中有同样id = " + curList[0]);
                else
                    dic.Add(curList[0], curList[1]);
            }
        }
        return dic;
    }

    public static Dictionary<long, int> StringToDicLN(string context, char[] curPara, char[] curSer)
    {
        if (string.IsNullOrEmpty(context)) return null;
        string[] arr = context.Split(curPara);

        Dictionary<long, int> dic = new Dictionary<long, int>();
        List<string> curList = null;
        long key = 0;
        int value = 0;
        foreach (string curStr in arr)
        {
            curList = StringToListStr(curStr, GameKeyName.Serialsuffix);
            key = long.Parse(curList[0]);
            value = int.Parse(curList[1]);

            if (dic.ContainsKey(key))
                Debug.LogWarning("字符中有同样id = " + key);
            else
                dic.Add(key, value);
        }
        return dic;
    }

    public static Dictionary<int, long> StringToDicNL(string context, char[] curPara, char[] curSer)
    {
        if (string.IsNullOrEmpty(context)) return null;
        string[] arr = context.Split(curPara);

        Dictionary<int, long> dic = new Dictionary<int, long>();
        List<string> curList = null;
        int key = 0;
        long value = 0;
        foreach (string curStr in arr)
        {
            curList = StringToListStr(curStr, GameKeyName.Serialsuffix);
            key = int.Parse(curList[0]);
            value = long.Parse(curList[1]);

            if (dic.ContainsKey(key))
                Debug.LogWarning("字符中有同样id = " + key);
            else
                dic.Add(key, value);
        }
        return dic;
    }
    #endregion

    #region 关键字字符串组合

    /// <summary>
    /// 关键字转换   比如 * = [0xff0000][url=" + * + "][u]等级为5"[/u][/url][]
    /// </summary>
    /// <param name="keyName"></param>
    /// <param name="context"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    //private static string StrConvertToKey(string keyName, string context, Color color)
    //{
    //    //string str = keyName;
    //    string url = "[url=" + keyName + "][u]";
    //    string middle = context + "[/u][/url]";
    //    string newStr = url + middle;
    //    return NGUIText.EncodeColor(newStr, color);
    //    /*
    //    switch (keyName)
    //    {
    //        case GameKeyName.KEYNAME_RANK:
    //            str = NGUIText.EncodeColor(newStr,color);
    //            break;
    //        default:
    //            break;
    //    }
    //    return str;*/
    //}

    /// <summary>
    /// 带指引的特殊字
    /// </summary>
    /// <param name="keyName"></param>
    /// <param name="desc"></param>
    /// <param name="replaceStr"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    //public static string ConvertSpecialStr(string keyName, string desc, string replaceStr, Color color)
    //{
    //    if (string.IsNullOrEmpty(desc))
    //        return "";
    //    return desc.Replace(keyName, StrConvertToKey(keyName, replaceStr, color));
    //}

    /// <summary>
    /// 普通特殊替换
    /// </summary>
    /// <param name="keyName"></param>
    /// <param name="desc"></param>
    /// <param name="replaceStr"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    //public static string ConvertNormalStr(string keyName, string desc, string replaceStr, Color color)
    //{
    //    if (string.IsNullOrEmpty(desc))
    //        return "";
    //    return desc.Replace(keyName, NGUIText.EncodeColor(replaceStr, color));
    //}
    #endregion


    private static char[] Vec3Sign = new char[] { '(', ')' };
    /// <summary>
    ///  字符串转vec3
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static Vector3 StrToVec3(string str)
    {
        if (string.IsNullOrEmpty(str)) return Vector3.zero;

        float x = 0, y = 0, z = 0;
        str = str.Trim(Vec3Sign);
        string[] data = str.Split(GameKeyName.Serialsuffix);
        float.TryParse(data[0], out x);
        float.TryParse(data[1], out y);
        float.TryParse(data[2], out z);
        return new Vector3(x, y, z);
    }

    public static Vector3 StringToVec3(string str)
    {
        if (string.IsNullOrEmpty(str)) return Vector3.zero;

        float x, y, z;
        str = str.Trim("()".ToCharArray());
        string[] data = str.Split(GameKeyName.Serialsuffix);
        float.TryParse(data[0], out x);
        float.TryParse(data[1], out y);
        float.TryParse(data[2], out z);
        return new Vector3(x, y, z);
    }

    public static Vector2 StrToRangeVec2(string str)
    {
        if (string.IsNullOrEmpty(str)) return Vector2.zero;
        float x = 0, y = 0;
        str = str.Trim(Vec3Sign);
        string[] data = str.Split(GameKeyName.Serialsuffix);
        if (data == null || data.Length < 2) return Vector2.zero;
        float.TryParse(data[0], out x);
        float.TryParse(data[1], out y);
        return new Vector2(x, y);
    }

    public static Vector3 StrToVec3(string str, char[] split)
    {
        if (string.IsNullOrEmpty(str)) return Vector3.zero;

        float x, y, z;
        str = str.Trim("()".ToCharArray());
        string[] data = str.Split(split);
        float.TryParse(data[0], out x);
        float.TryParse(data[1], out y);
        if (data.Length == 3)
            float.TryParse(data[2], out z);
        else z = 0;
        return new Vector3(x, y, z);
    }

    public static int StrToInt(string str)
    {
        int num = 0;
        if (string.IsNullOrEmpty(str)) return num;
        int.TryParse(str, out num);
        return num;
    }

    /*
    public static Vector3 PosInfoToVec3(PositionInfo info)
    {
        return new Vector3(info.x, info.y, info.z);
    }*/

    /// <summary>
    /// 字符串转color
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static Color StrToColor(string str)
    {
        if (string.IsNullOrEmpty(str))
            return Color.white;
        float x, y, z, a;
        string[] data = str.Split(GameKeyName.ParallelSuffix);
        if (data == null || data.Length != 4)
            return Color.white;
        float.TryParse(data[0], out x);
        float.TryParse(data[1], out y);
        float.TryParse(data[2], out z);
        float.TryParse(data[3], out a);
        return new Color(x / 255f, y / 255f, z / 255f, a / 255f);
    }

    /// <summary>
    /// Vec3转字符串
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static string Vec3ToStr(Vector3 vec)
    {
        return vec.x.ToString("#0.00") + "," + vec.y.ToString("#0.00") + "," + vec.z.ToString("#0.00");
    }

    /// <summary>
    /// 金钱显示颜色
    /// </summary>
    /// <param name="num"></param>
    /// <param name="isSufficient"></param>
    /// <returns></returns>
    //public static string CurrencyValueColor(string num, bool isSufficient)
    //{
    //    return NGUIText.EncodeColor(num, isSufficient ? Color.green : Color.red);
    //}

    //public static string EncodeColor(string str, string color)
    //{
    //    return NGUIText.EncodeColor(str, NGUIText.ParseColor(color));
    //}

    //public static string EncodeColor(string str, Color color)
    //{
    //    return NGUIText.EncodeColor(str, color);
    //}


    /// <summary>
    /// 名字对应颜色 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Color ColorNameToColor(string name)
    {
        if (string.IsNullOrEmpty(name))
            return Color.clear;

        switch (name.ToLower())
        {
            case "black":
                return Color.black;
            case "blue":
                return Color.blue;
            case "clear":
                return Color.clear;
            case "cyan":
                return Color.cyan;
            case "gray":
                return Color.gray;
            case "green":
                return Color.green;
            case "grey":
                return Color.grey;
            case "magenta":
                return Color.magenta;
            case "red":
                return Color.red;
            case "white":
                return Color.white;
            case "yellow":
                return Color.yellow;
            default:
                return StrToColor(name.ToLower());
        }
    }

    #region 命令拆分
    public static string[] CommandToStrArr(string commandStr, int length, char[] suffix)
    {
        if (string.IsNullOrEmpty(commandStr))
        {
            Debug.Log("!!命令字符串为空!");
            return null;
        }


        string[] arr = commandStr.Split(suffix);

        if (length != 0 && !arr.Length.Equals(length))
        {
            Debug.Log("!!字符串长度不匹配！");
            return null;
        }

        return arr;
    }
    #endregion

    /// <summary>
    ///获取字符串长度，已特殊分隔符
    /// </summary>
    /// <param name="str"></param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    public static int GetLength(string str, char[] suffix)
    {
        if (string.IsNullOrEmpty(str))
            return 0;

        string[] name = str.Split(suffix);
        return name.Length;
    }

    /// <summary>
    /// 数字中添加, off每隔几位
    /// </summary>
    public static string GetNumberStr(string SrcNum, int off)
    {
        if (string.IsNullOrEmpty(SrcNum))
            return "";

        string flt = SrcNum.IndexOf('.') < 0 ? "" : SrcNum.Split('.')[1];
        SrcNum = SrcNum.Split('.')[0];
        int pnt = SrcNum.Length % off;
        if (SrcNum.Length > off)
        {
            while (pnt + 1 < SrcNum.Length)
            {
                int i = pnt % (off + 1) == 0 ? pnt : pnt + 1;
                if (i > 0) SrcNum = SrcNum.Insert(i - 1, ",");
                pnt += (off + 1);
            }
        }
        return SrcNum + flt;
    }

    public static string GetGoldAccountsStr(long num)
    {
        if (num < 100000) return num.ToString();
        else return AppendFormat("{0}W", Mathf.FloorToInt(num * 1.0f / 10000));
    }

    public static string NumToString(this long num)
    {
        if (num < 100000)
            return num.ToString();
        else
            return AppendFormat("{0}[ffc148ff]W[-]", Mathf.FloorToInt(num * 1.0f / 10000));
    }

    public static string NumToString(this int num)
    {
        if (num < 100000)
            return num.ToString();
        else
            return AppendFormat("{0}[ffc148ff]W[-]", Mathf.FloorToInt(num * 1.0f / 10000));
    }

    public static string GetAttackStr(long num)
    {
        if (num < 100000)
        {
            return num.ToString();
        }
        else
        {
            int willNum = Mathf.FloorToInt(num * 1.0f / 1000);
            return AppendFormat("{0}{1}", FormatFloat(willNum * 0.1f), "w");
        }
    }

    public static string GetAttackStr(int num)
    {
        if (num < 100000)
        {
            return num.ToString();
        }
        else
        {
            int willNum = Mathf.FloorToInt(num * 1.0f / 1000);
            return AppendFormat("{0}{1}", FormatFloat(willNum * 0.1f), "w");
        }
    }

    public static string GetPercentageStr(float rate)
    {
        return AppendFormat("{0}{1}", (rate * 100).ToString("#0.0"), "%");
    }

    public static string FormatFloat(float num)
    {
        string old = num.ToString("#0.0");
        return old.Replace(".0", "");
    }
    

    ///// <param name="data">给定数字</param>
    ///// <param name="count">保留有效数字位数</param>
    //public static double ValidFun(double data,int count)
    //{
    //    if (data == 0.0)
    //        return 0;
    //    if (data > 1  || data < -1)
    //        count = count - (int)Math.Log10(Math.Abs(data)) - 1;
    //    else
    //        count = count + (int)Math.Log10(1.0 / Math.Abs(data));
    //    if (count < 0)
    //    {
    //        data = (int)(data / Math.Pow(10, 0 - count)) * Math.Pow(10, 0 - count);
    //        count = 0;
    //    }
    //    return Math.Round(data, count);
    //}

    public static string GetDPSStr(long num)
    {
        if (num < 100000) return num.ToString();
        else if (num >= 100000 && num < 1000000)
            return string.Format("{0}万", Math.Floor(num * 1.0f / 1000));
        else if (num >= 100000000 && num < 1000000000000)
            return string.Format("{0}亿", Math.Floor(num * 1.0f / 1000000));
        else
            return string.Format("{0}b", Math.Floor(num * 1.0f / 100000000));
    }

    public static string GetDPSStr(ulong num)
    {
        if (num < 10000) return num.ToString();
        else if (num < 100000000)
            return LocalizationMgr.Instance.GetString("Common_Wan1", (num * 1.0f / 10000).ToString("#0.0"));
        else
            return LocalizationMgr.Instance.GetString("Common_Yi1", (num * 1.0f / 100000000).ToString("#0.0"));
    }

    public static string GetDPSNumStr(ulong num)
    {
        if (num < 10000) return num.ToString();
        else if (num < 100000000) return (num * 1.0f / 10000).ToString("#0.0");
        else return (num * 1.0f / 100000000).ToString("#0.0");
    }

    public static string GetByteFormat(long num)
    {
        return GetByteFormat(num * 1f);
    }
    public static string GetByteFormat(float num)
    {
        if (num < (1024f * 1024f)) return (num / 1024f).ToString("#0.0") + "KB";
        else if (num < (1024f * 1024f * 1024f)) return (num / (1024f * 1024f)).ToString("#0.0") + "MB";
        else return (num / (1024f * 1024f * 1024f)).ToString("#0.0") + "GB";
    }
    public static string GetDefualtTime(int time)
    {
        if (time < 0)
            time = 0;

        string timeStr = "";

        timeStr += (time / 3600).ToString("#00") + ":";
        timeStr += ((time % 3600) / 60).ToString("#00") + ":";
        timeStr += (time % 60).ToString("#00");

        return timeStr;
    }

    /// <summary> 根据品质得到英雄名称的颜色值   </summary>
    public static string GetHeroNameByQuality(int quality)
    {
        switch (quality)
        {
            case 1:
                return "#ffffff";
            case 2:
                return "#14D409";
            case 3:
                return "#def6ff";
            case 4:
                return "#e6dfff";
            case 5:
                return "#fcf186";
            case 6:
                return "#ff9797";
            default:
                return "#ffffff";
        }
    }
    /// <summary>
    /// item名称及文字描述颜色
    /// </summary>
    public static string GetQualityColor(int quality)
    {
        switch (quality)
        {
            case 1:
                return "#ffffff";
            case 2:
                return "#14D409";
            case 3:
                return "#6e96d2";
            case 4:
                return "#bb6ced";
            case 5:
                return "#e8a54e";
            case 6:
                return "#ff9797";
            default:
                return "#ffffff";
        }
    }

    /// <summary>
    /// 弹框上显示的各种物品名称颜色值
    /// </summary>
    public static string GetJumpFrameNameByQuality(int quality)
    {
        switch (quality)
        {
            case 1:
                return "#ffffff";
            case 2:
                return "#ffffff";
            case 3:
                return "#b3e3ff";
            case 4:
                return "#cdb9ff";
            case 5:
                return "#fdd171";
            case 6:
                return "#ff877d";
            default:
                return "#ffffff";
        }
    }
    /// <summary>
    /// 弹框上显示品质中文的颜色值
    /// </summary>
    public static string GetFrameQualityNameByQuality(int quality)
    {
        switch (quality)
        {
            case 1:
                return "#ffffff";
            case 2:
                return "#ffffff";
            case 3:
                return "#c3e6ff";
            case 4:
                return "#e3cbff";
            case 5:
                return "#fcf186";
            case 6:
                return "#ff9797";
            default:
                return "#ffffff";
        }
    }

    public static string ChangeStrColor(string color, string str)
    {
        return string.Format("<color=#{0}>{1}</color>", color, str);
    }

    //public static Color GetWorldTaskColor(int quality)
    //{
    //    switch (quality)
    //    {
    //        case 1:
    //            return NGUIText.ParseColor("ffffff");           //白
    //        case 2:
    //            return NGUIText.ParseColor("00baff");           //蓝
    //        case 3:
    //            return NGUIText.ParseColor("f780f0");           //紫
    //        case 4:
    //            return NGUIText.ParseColor("ffa758");           //橙
    //        default:
    //            return Color.white;
    //    }
    //}
    //public static Color GetWorldTaskOutLineColor(int quality)
    //{
    //    switch (quality)
    //    {
    //        case 1:

    //            return NGUIText.ParseColor("3a3a3f");           //白
    //        case 2:

    //            return NGUIText.ParseColor("2d4273");           //蓝
    //        case 3:

    //            return NGUIText.ParseColor("7d3672");           //紫
    //        case 4:

    //            return NGUIText.ParseColor("6a3f25");           //橙
    //        default:

    //            return Color.white;
    //    }
    //}

    //public static Color GetMissionColor(int quality, UISprite data =null, UISprite data2=null)
    //{
    //    switch (quality)
    //    {
    //        case 1:
    //            //UIUtility.Safe_NGUI(ref data, "bg_xs_sj_01");
    //           // UIUtility.Safe_NGUI(ref data2, "bng_xs_guangxiao_01");
    //            return NGUIText.ParseColor("e3e3e3");           //白
    //        case 2:
    //           // UIUtility.Safe_NGUI(ref data, "bg_xs_sj_02");
    //           // UIUtility.Safe_NGUI(ref data2, "bng_xs_guangxiao_02");
    //            return NGUIText.ParseColor("40eeec");           //蓝
    //        case 3:
    //          //  UIUtility.Safe_NGUI(ref data, "bg_xs_sj_03");
    //         //   UIUtility.Safe_NGUI(ref data2, "bng_xs_guangxiao_03");
    //            return NGUIText.ParseColor("f083f1");           //紫
    //        case 4:
    //           // UIUtility.Safe_NGUI(ref data, "bg_xs_sj_04");
    //           // UIUtility.Safe_NGUI(ref data2, "bng_xs_guangxiao_04");
    //            return NGUIText.ParseColor("f3aa43");           //橙
    //        default:
    //           // UIUtility.Safe_NGUI(ref data, "bg_xs_sj_01");
    //           // UIUtility.Safe_NGUI(ref data2, "bng_xs_guangxiao_01");
    //            return Color.white;
    //    }
    //}
    //public static Color GetMissionOutLineColor(int quality)
    //{
    //    switch (quality)
    //    {
    //        case 1:

    //            return NGUIText.ParseColor("00000000");           //白
    //        case 2:

    //            return NGUIText.ParseColor("5f5d87");           //蓝
    //        case 3:

    //            return NGUIText.ParseColor("6e2785");           //紫
    //        case 4:

    //            return NGUIText.ParseColor("854a27");           //橙
    //        default:

    //            return Color.white;
    //    }
    //}
    //public static string GetColorText(string text, int quality)
    //{
    //    return NGUIText.EncodeColor(text, GetQualityColor(quality));
    //}

    //public static string GetColorText(string text, string colorStr)
    //{
    //    return NGUIText.EncodeColor(text, NGUIText.ParseColor(colorStr));
    //}

    //public static string GetColorText(string text, Color colorStr)
    //{
    //    return NGUIText.EncodeColor(text, colorStr);
    //}

    /*
    public static string GetItemAttribute(TItemProperty property)
    {
        if (property == null)
            return "";

        List<string> curList = new List<string>();
        FieldInfo[] fileArr = typeof(TItemProperty).GetFields();

        if (fileArr == null || fileArr.Length == 0)
            return "";

        string curStr = "";
        int value = 0;
        for (int i = 0; i < fileArr.Length; i++)
        {
            if (!fileArr[i].Name.Equals("ID") && int.TryParse(fileArr[i].GetValue(property).ToString(), out value))
            {
                if (value != 0)
                    curStr += TItemPropertyManager.Instance.GetDesc(fileArr[i].Name) + " " + value + "\r\n";
            }
        }
        return NGUIText.EncodeColor(curStr, NGUIText.ParseColor("5C47FF"));
    }
    */

    public static string IndexToCharStr(int num)
    {
        return ('a' + num).ToString();
    }

    public static string NumToChar(string typeStr, string context)
    {
        char[] charArray = context.ToCharArray();
        string tmpStr = null;
        for (int i = 0; i < charArray.Length; i++)
        {
            //if (charArray[i] >= '0' && charArray[i] <= '9')
            tmpStr += string.Format(typeStr, charArray[i]);
            // else
            // Debuger.LogWarning("invalid contenxt");
        }
        return tmpStr;
    }

    public static List<int> StrToListInt(string str)
    {
        return StrToListInt(str, GameKeyName.ParallelSuffix);
    }


    public static List<int> StrToListInt(string str, params char[] obj)
    {
        if (!string.IsNullOrEmpty(str))
        {
            List<int> intList = new List<int>();
            string[] strArr = str.Split(obj);
            for (int i = 0; i < strArr.Length; i++)
            {
                int intVal = 0;
                if (int.TryParse(strArr[i], out intVal))
                {
                    intList.Add(intVal);
                }
                else
                {
                    continue;
                }

            }
            return intList;
        }
        return null;
    }

    //public static string QuaSpriteName(ref UISprite sprite, int qua)
    //{
    //    if (sprite == null || string.IsNullOrEmpty(sprite.spriteName)) return "";
    //    if (!sprite.spriteName.Contains("_")) return sprite.spriteName;
    //    return string.Format("{0}_{1}", sprite.spriteName.Substring(0, sprite.spriteName.LastIndexOf('_')), qua);
    //}

    public static string FormatText(string str) { return str.Replace("\\r\\n", "\r\n"); }

    public static string Append(params object[] parms)
    {
        if (parms == null || parms.Length == 0) return default(string);
        ResetSbuilder();
        for (int i = 0; i < parms.Length; i++)
            stringBuilder.Append(parms[i]);
        return stringBuilder.ToString();
    }

    public static string AppendFormat(string format, params object[] parms)
    {
        if (parms == null || parms.Length == 0) return format;
        ResetSbuilder();
        stringBuilder.AppendFormat(format, parms);
        return stringBuilder.ToString();
    }

    /// <summary>
    /// 字符中间加入换行，主要用于UI文字的竖排
    /// </summary>
    public static string InsertLineFeed(string str)
    {
        if (str == null || str.Length == 0) return str;
        ResetSbuilder();
        for (int i = 0; i < str.Length; i++)
        {
            if (i == str.Length - 1)
                stringBuilder.Append(str[i]);
            else
            {
                stringBuilder.Append(str[i]);
                stringBuilder.AppendLine();
            }
        }
        return stringBuilder.ToString();
    }

    public static bool IsNullOrEmpty(this string str) { return string.IsNullOrEmpty(str); }

    public static bool IsHasValue(this string str) { return !string.IsNullOrEmpty(str) && str != "0"; }

    public static bool IsHasValue(this int value) { return value > 0; }

    public static string ReplaceR(this string str)
    {
        return str.Replace("\\r", "\r").Replace("/r", "\r");
    }
    public static string ReplaceN(this string str)
    {
        return str.Replace("\\n", "\n").Replace("/n", "\n");
    }
    public static string ReplaceRN(this string str)
    {
        return str.Replace("\\r\\n", "\r\n").Replace("/r/n", "\r\n");
    }
    public static float ToFloat(this string str)
    {
        float.TryParse(str, out float result);
        return result;
    }
    public static int ToInt(this string str)
    {
        int.TryParse(str, out int result);
        return result;
    }

    /// <summary>
    /// 获取繁体文字
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string GetTraditional(int num)
    {
        switch (num)
        {
            case 0: return "零";
            case 1: return "壹";
            case 2: return "贰";
            case 3: return "叁";
            case 4: return "肆";
            case 5: return "伍";
            case 6: return "陆";
            case 7: return "柒";
            case 8: return "捌";
            case 9: return "玖";
            case 10: return "拾";
        }
        return "";
    }

    /// <summary>
    /// 获取简体文字
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string GetSimplified(int num)
    {
        switch (num)
        {
            case 0: return "零";
            case 1: return "一";
            case 2: return "二";
            case 3: return "三";
            case 4: return "四";
            case 5: return "五";
            case 6: return "六";
            case 7: return "七";
            case 8: return "八";
            case 9: return "九";
            case 10: return "十";
            case 11: return "十一";
            case 12: return "十二";
        }
        return "";
    }

    /// <summary>
    /// 获取英文月份
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string GetMonthEN(int num)
    {
        switch (num)
        {
            case 1: return "January";
            case 2: return "February";
            case 3: return "March";
            case 4: return "April";
            case 5: return "May";
            case 6: return "June";
            case 7: return "July";
            case 8: return "August";
            case 9: return "September";
            case 10: return "October";
            case 11: return "November";
            case 12: return "December";
        }
        return "";
    }


    /// <summary>
    /// 获取罗马数字
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string GetRomeNum(int num)
    {
        switch (num)
        {
            case 1: return "I";
            case 2: return "II";
            case 3: return "III";
            case 4: return "IV";
            case 5: return "V";
            case 6: return "VI";
            case 7: return "VII";
            case 8: return "VIII";
            case 9: return "IX";
            case 10: return "X";
        }
        return "";
    }

    /// <summary>
    /// 数字以W为单位计数
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string TenthousandCount(int num)
    {
        string str = "";

        if (num >= 10000)
        {
            str = (num / 10000).ToString() + "W";
        }
        else
        {
            str = num.ToString();
        }

        return str;
    }

    /// <summary>
    /// 获取数据信息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="msg"></param>
    /// <returns></returns>
    //public static string LanguageMsg(int type, string msg, params object[] args)
    //{
    //    if (type == 1)
    //    {
    //        int msgId;
    //        if (!int.TryParse(msg, out msgId))
    //        {
    //            Debuger.LogError("解析TLanguage表中ID ={0} 错误", msg);
    //            return "";
    //        }
    //        else
    //            return TSelfLanguageMgr.Instance.GetItem(msgId, args);
    //    }
    //    else
    //    {
    //        return AppendFormat(msg, args);
    //    }
    //}

    public static char NumToLChar(uint num)
    {
        if (num >= 26) return 'a';
        else return (char)('a' + num);
    }

    public static char NumToUChar(uint num)
    {
        if (num >= 26) return 'A';
        else return (char)('A' + num);
    }

    /// <summary>
    /// 属性都是整数，但显示时候需要处理
    /// 1是整数
    /// 2是百分比数，如20，显示为20%
    /// 3是万分比数，如1250，显示为12.5%（保留1位小数）
    ///4是不显示（现在没用到）
    /// </summary>
    /// <param name="dataType">AttrDataType</param>
    /// <param name="value">传入值</param>
    /// <returns></returns>
    //public static string showDataType(int dataType, int value)
    //{
    //    string str = "";
    //    switch (dataType)
    //    {
    //        case (int)AttrDataType.Integer:
    //            str = value.ToString();
    //            break;
    //        case (int)AttrDataType.Percent:
    //            string decimalStr = String.Format("{0:N1}", value);
    //            str = decimalStr + "%";
    //            break;
    //        case (int)AttrDataType.Million:
    //            string str1 = String.Format("{0:N1}", value / 100.0f);
    //            str = str1 + "%";
    //            break;
    //        case (int)AttrDataType.Hide:
    //            break;
    //        case (int)AttrDataType.Millisecond:
    //            value = value / 1000;
    //            str = value + "s";
    //            break;
    //        case (int)AttrDataType.Decimals:
    //            str = value.ToString();
    //            break;
    //        default:
    //            break;
    //    }
    //    return str;
    //}

    public static string UnicodeToChs(string unicodeStr)
    {
        if (string.IsNullOrEmpty(unicodeStr))
            return unicodeStr;
        return Regex.Unescape(unicodeStr);
    }

    public static string ToWrite(this int[] arr)
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("[");

        for (int i = 0; i < arr.Length; i++)
        {
            builder.Append(arr[i]);

            if (i < arr.Length - 1)
                builder.Append(",");
        }

        builder.Append("]");
        return builder.ToString();
    }

    public static List<int> ToList(this int[] arr)
    {
        return new List<int>(arr);
    }

    public static string GetFinallyPackage(this string package)
    {
        if (string.IsNullOrEmpty(package))
            return AssetConst.DefaultPackage;
        if (package.Equals("Main"))
            return AssetConst.DefaultPackage;
        return package;
    }

    public static List<string> TryParseStringList(string text, char[] split)
    {
        List<string> list = new List<string>();
        if (string.IsNullOrEmpty(text))
            return list;

        string[] strArry = text.Split(split);
        for (int i = 0, len = strArry.Length; i < len; ++i)
        {
            if (string.IsNullOrEmpty(strArry[i]))
            {
                continue;
            }
            list.Add(strArry[i]);
        }
        return list;
    }

    public static List<float> TryParseFloatList(string text, char[] split)
    {
        List<float> list = new List<float>();
        if (string.IsNullOrEmpty(text))
            return list;

        string[] strArry = text.Split(split);
        for (int i = 0, len = strArry.Length; i < len; ++i)
        {
            if (string.IsNullOrEmpty(strArry[i]))
            {
                list.Add(0);
                continue;
            }
            float f;
            float.TryParse(strArry[i], out f);
            list.Add(f);
        }
        return list;
    }

    public static List<int> TryParseIntList(string text, char[] split)
    {
        List<int> list = new List<int>();
        if (string.IsNullOrEmpty(text))
            return list;

        string[] strArry = text.Split(split);
        for (int i = 0, len = strArry.Length; i < len; ++i)
        {
            if (string.IsNullOrEmpty(strArry[i]))
            {
                list.Add(0);
                continue;
            }
            int f;
            int.TryParse(strArry[i], out f);
            list.Add(f);
        }
        return list;
    }
    
    public static void LogMessageMembers<T>(this T val, string tag)
    {
        bool isLog = Boot.GetValue<bool>("LogMessageMembers");
        if (!isLog) return;
        Debuger.Log($"[{tag}]: {val.LogMembers()}");
    }

    public static string LogMembers<T>(this T val)
    {
        return LogMembers(val, typeof(T));
    }

    public static string LogMembers(this object val, Type t, int indent = 0)
    {
        if (val == null) return "null";
        var fs = t.GetFields();
        string s = $"{t.FullName}\n";
        bool first = true;
        foreach (var f in fs)
        {
            if("UnityEngine".Equals(f.FieldType.Namespace))
                continue;
            s += first ? "" : "\n";
            first = false;
            for (int i = 0; i < indent; i++)
            {
                s += "    ";
            }

            if (f.FieldType.IsArray || f.FieldType.IsEnum ||
                (f.FieldType.Namespace != null && f.FieldType.Namespace.Contains("System")))
            {
                var c = f.FieldType.IsArray ? $", length={((Array)f.GetValue(val))?.Length.ToString() ?? ("null")}" : "";
                var v = f.GetValue(val);
                s += $"{f.Name}: {v ?? "null"}{c}";
            }
            else
            {
                s += $"{f.Name}: \n" +
                     $"{f.GetValue(val)?.LogMembers(f.FieldType, indent + 4) ?? "null"}";
            }
        }

        var ps = t.GetProperties();
        foreach (var p in ps)
        {
            s += "\n";
            for (int i = 0; i < indent; i++)
            {
                s += "    ";
            }

            if (p.PropertyType.IsArray || p.PropertyType.IsEnum ||
                (p.PropertyType.Namespace != null && p.PropertyType.Namespace.Contains("System")))
            {
                var c = p.PropertyType.IsArray ? $", length={((Array)p.GetValue(val)).Length}" : "";
                var v = p.GetValue(val);
                s += $"{p.Name}: {v ?? "null"}{c}";
            }
            else
            {
                s += $"{p.Name}: \n" +
                     $"{p.GetValue(val)?.LogMembers(p.PropertyType, indent + 4) ?? "null"}";
            }
        }

        return s;
    }

    public static string Size(this object str, int size)
    {
        return $"<size={size}>{str.ToString()}</size>";
    }

    public static string AppendColor(this object str, string color)
    {
        if (!color.StartsWith("#"))
            color = $"#{color}";
        return $"<color={color}>{str.ToString()}</color>";
    }
    
    public static string ToMoney(this long rCount)
    {
        //if rCount > 10000 then format('%.2f万', [rCount / 10000]);
        if (rCount > 10000)
        {
            return $"{(rCount / 10000f):F2}万";
        }
        return $"{rCount}";
    }

    public static string ToWord(this int rCount)
    {
        if (rCount > 10000)
        {
            return $"{(rCount / 10000):F0}万";
        }
        return $"{rCount}";
    }

    public static string ToWord(this long rCount)
    {
        if (rCount > 10000)
        {
            return $"{(rCount / 10000):F0}万";
        }
        return $"{rCount}";
    }

    private static int Seed = 0;
    
    /// <summary>
    /// 生成单个随机数字
    /// </summary>
    public static int CreateRadmonNum()
    {
        Random random = new Random(Guid.NewGuid().GetHashCode());
        int num = random.Next(10);
        return num;
    }
 
    /// <summary>
    /// 生成单个大写随机字母
    /// </summary>
    public static string CreateRadmonBigAbc()
    {
        //A-Z的 ASCII值为65-90
        Random random = new Random(Guid.NewGuid().GetHashCode());
        int num = random.Next(65, 91);
        string abc = Convert.ToChar(num).ToString();
        return abc;
    }
 
    /// <summary>
    /// 生成单个小写随机字母
    /// </summary>
    public static string CreateRadmonSmallAbc()
    {
        //a-z的 ASCII值为97-122
        Random random = new Random(Guid.NewGuid().GetHashCode());
        int num = random.Next(97, 123);
        string abc = Convert.ToChar(num).ToString();
        return abc;
    }
 
    /// <summary>
    /// 生成随机字符串
    /// </summary>
    /// <param name="length">字符串的长度</param>
    /// <returns></returns>
    public static string CreateRadmonStr(int length)
    {
        // 创建一个StringBuilder对象存储密码
        StringBuilder sb = new StringBuilder();
        //使用for循环把单个字符填充进StringBuilder对象里面变成14位密码字符串
        for (int i = 0; i < length; i++)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode() + ++Seed);
            //随机选择里面其中的一种字符生成
            switch (random.Next(3))
            {
                case 0:
                    //调用生成生成随机数字的方法
                    sb.Append(CreateRadmonNum());
                    break;
                case 1:
                    //调用生成生成随机小写字母的方法
                    sb.Append(CreateRadmonSmallAbc());
                    break;
                case 2:
                    //调用生成生成随机大写字母的方法
                    sb.Append(CreateRadmonBigAbc());
                    break;
            }
        }
        return sb.ToString();
    }

    
}

