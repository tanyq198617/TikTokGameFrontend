using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class CreateVariantEditor
{
    [MenuItem("Assets/创建变体到Battle文件夹")]
    public static void CreateBulletVariants()
    {
        var savePath = Application.dataPath + "/HotUpdateResources/Prefabs/Battle/";
        var gas = Selection.gameObjects;
        if (gas.Length > 0)
        {
            for (int i = 0; i < gas.Length; i++)
            {
                CreateVariant(gas[i], savePath + gas[i].name + ".prefab");
            }
        }
    }
    public static void CreateVariant(GameObject go, string savePath)
    {
        System.Type PrefabUtilityType =
            Assembly.GetAssembly(typeof(PrefabUtility)).GetType("UnityEditor.PrefabUtility", true);
        var CreateVariant =
            PrefabUtilityType.GetMethod("CreateVariant", BindingFlags.NonPublic | BindingFlags.Static);
        //string filePath = Path.Combine(dir, go.name + "_variant.prefab");
        //filePath = Application.dataPath.Replace("Assets", "") + filePath;
        //filePath = filePath.Replace('\\', '/');
        CreateVariant.Invoke(null, new object[] {go, savePath});
    }
}
