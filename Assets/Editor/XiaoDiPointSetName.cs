using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary> 给子物体设置名字.按阿拉伯数字命名 /// </summary>
public class XiaoDiPointSetName
{
    [MenuItem("GameObject/设置子物体名字")]
    public static void CreateBulletVariants()
    {
        var gas = Selection.gameObjects;
        if (gas.Length > 0)
        {
            for (int i = 0; i < gas.Length; i++)
            {
                int index = 0;
                Transform tran = gas[i].transform;
                for (int j = 0; j < tran.childCount; j++)
                {
                    index++;
                    tran.GetChild(j).name = index.ToString();
                }
            }
        }
    }
}
