using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace HotUpdateScripts
{
    public static class TNinoParser
    {
        public static T[] Parse<T>(string name, string path, Func<string, string, byte[]> loadBytes, Action<int, T> onSet)
        {
            byte[] bytes = loadBytes?.Invoke(name, path);

            if (bytes == null || bytes.Length <= 0)
            {
                Debug.LogError("无法加载表格文件：" + name);
                return null;
            }            

            List<T> array = null;
            try
            {
                //WatchTime.Start();
                array = Nino.Serialization.Deserializer.Deserialize<List<T>>(bytes);
                //WatchTime.ShowTime($"[Nino] 解析 [{name}], Count={bytes.Length}(bytes), 行数={array.Count}(Line), 耗时：");
            }
            catch (Exception e)
            {
                Debug.LogError($"无法读取表格文件：name={name} 错误信息e={e}");
            }

            for (int i = 0; i < array.Count; i++)
            {
                array[i] = array[i];
                onSet(i, array[i]);
            }
            return array.ToArray();
        }

        public static async UniTask<T[]> ParseAsync<T>(string name, string path, Func<string, string, UniTask<byte[]>> loadBytes, Action<int, T> onSet, CancellationTokenSource source)
        {
            byte[] bytes = await loadBytes.Invoke(name, path);

            if (bytes == null || bytes.Length <= 0)
            {
                Debug.LogError("无法加载表格文件：" + name);
                return null;
            }

            T[] result = null;

            try
            {
                result = await Task.Run<T[]>(() =>
                {
                    List<T> array = null;

                    try
                    {
                        //WatchTime.Start();
                        array = Nino.Serialization.Deserializer.Deserialize<List<T>>(bytes);
                        //WatchTime.ShowTime($"[Nino] 解析{name}, Len(bytes)={bytes.Length}, 行数={array.Count}, 耗时：");
                        return array.ToArray();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"无法读取表格文件：name={name}, Length={bytes.Length}, 错误信息e={e}");
                        return null;
                    }
                }, source.Token);
            }
            catch (Exception e)
            {
                if (!(e is TaskCanceledException))
                {
                    Debug.LogError($"[Task.Run] 无法读取表格文件：name={name}, Length={bytes.Length} 错误信息e={e}");
                }
            }

            for (int i = 0; i < result.Length; i++)
            {
                onSet(i, result[i]);
            }
            return result;
        }
    }

}