using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace HotUpdateScripts
{
    public static class TableParser
    {
        static void ParsePropertyValue<T>(T obj, FieldInfo fieldInfo, string valueStr)
        {
            System.Object value;
            if (fieldInfo.FieldType.IsEnum)
                value = Enum.Parse(fieldInfo.FieldType, valueStr);
            else
            {
                if (fieldInfo.FieldType == typeof(int))
                    value = int.Parse(valueStr);
                else if (fieldInfo.FieldType == typeof(byte))
                    value = byte.Parse(valueStr);
                else if (fieldInfo.FieldType == typeof(float))
                    value = float.Parse(valueStr);
                else if (fieldInfo.FieldType == typeof(long))
                    value = long.Parse(valueStr);
                else if (fieldInfo.FieldType == typeof(uint))
                    value = uint.Parse(valueStr);
                else if (fieldInfo.FieldType == typeof(ulong))
                    value = ulong.Parse(valueStr);

                else if (fieldInfo.FieldType == typeof(double))
                    value = double.Parse(valueStr);
                else
                {
                    if (valueStr.Contains("\"\""))
                        valueStr = valueStr.Replace("\"\"", "\"");

                    if (valueStr.Length > 2 && valueStr[0] == '\"' && valueStr[valueStr.Length - 1] == '\"')
                        valueStr = valueStr.Substring(1, valueStr.Length - 2);

                    value = valueStr;
                }
            }

            fieldInfo.SetValue(obj, value);
        }

        static T ParseObject<T>(string[] lines, int idx, Dictionary<int, FieldInfo> propertyInfos, ref Dictionary<string, string> keys)
        {
            T obj = Activator.CreateInstance<T>();
            string line = lines[idx];
            string[] descs = lines[0].Split('\t');
            string[] values = line.Split('\t');
            foreach (KeyValuePair<int, FieldInfo> pair in propertyInfos)
            {
                if (pair.Key >= values.Length)
                    continue;

                string value = values[pair.Key];
                if (string.IsNullOrEmpty(value))
                    continue;

                try
                {
                    if (!keys.ContainsKey(pair.Value.Name))
                        keys.Add(pair.Value.Name, descs[pair.Key]);
                    ParsePropertyValue(obj, pair.Value, value);
                }
                catch (Exception)
                {
                    Debug.LogError(string.Format("ParseError:TTable={0} Row={1} Column={2} Name={3} Want={4} Get=[{5}]",
                        typeof(T).ToString(),
                        idx + 2,
                        pair.Key + 1,
                        pair.Value.Name,
                        pair.Value.FieldType.Name,
                        value));
                    throw;
                }
            }
            return obj;
        }

        static Dictionary<int, FieldInfo> GetPropertyInfos<T>(string memberLine)
        {
            Type objType = typeof(T);

            string[] members = memberLine.Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            Dictionary<int, FieldInfo> propertyInfos = new Dictionary<int, FieldInfo>();
            for (int i = 0; i < members.Length; i++)
            {
                FieldInfo fieldInfo = objType.GetField(members[i]);
                if (fieldInfo == null)
                    continue;
                propertyInfos[i] = fieldInfo;
            }

            return propertyInfos;
        }

        //static T[] LoadTable<T>(string name)
        //{
        //    TextAsset textAsset = UpdateHelper.Instance.GetAsset("Tables.unity3d", name);
        //    MemoryStream stream = new MemoryStream(textAsset.bytes);
        //    return ProtoBuf.Serializer.Deserialize<T[]>(stream);
        //}

        public static byte[] LoadFromCacheDirect(string url)
        {
            using (System.IO.Stream s = System.IO.File.OpenRead(url))
            {
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                return b;
            }
        }

        public static string LoadTable(string name)
        {
            return "";// FileUtility.LoadConfig(name, ConfigType.Table);
        }

        public static T[] Parse<T>(string name, ref Dictionary<string, string> keys)
        {
            if (keys != null && keys.Count != 0)
                keys.Clear();
            string curStr = LoadTable(name);


            if (string.IsNullOrEmpty(curStr))
            {
                Debug.LogError("无法加载表格文件：" + name);
                return null;
            }

            string[] lines = curStr.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 3)
            {
                Debug.LogError("表格文件行数错误，【1】属性名称【2】数据类型 【3】变量名称【4-...】值：" + name);
                return null;
            }

            Dictionary<int, FieldInfo> propertyInfos = GetPropertyInfos<T>(lines[0]);

            T[] array = new T[lines.Length - 3];
            for (int i = 0; i < lines.Length - 3; i++)
                array[i] = ParseObject<T>(lines, i + 3, propertyInfos, ref keys);

            return array;
        }

        public static T CloneItem<T>(T source) where T : ITableItem, new()
        {
            T obj = new T();
            FieldInfo[] fields = typeof(T).GetFields();
            for (int i = 0; i < fields.Length; ++i)
            {
                fields[i].SetValue(obj, fields[i].GetValue(source));
            }
            return obj;
        }
    }
}