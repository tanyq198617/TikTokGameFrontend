using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UITools : Editor
{
    private static string[] prefabPaths = {
        "Assets/HotUpdateResources/Prefab/ui"
    };
    private static string[] allPrefabPaths = {
        "Assets/HotUpdateResources/Prefab/item",
        "Assets/HotUpdateResources/Prefab/ui"
    };
    private static List<Image> images = new List<Image>();
    //[MenuItem("UITools/SpriteAtlas/给所有界面添加图集脚本", priority = 1)]
    //public static void AddAtlasImgScriptAll()
    //{
    //    string[] guids = AssetDatabase.FindAssets("t:Prefab", prefabPaths);
    //    for (int i = 0; i < guids.Length; ++i)
    //    {
    //        string path = AssetDatabase.GUIDToAssetPath(guids[i]);
    //        GameObject prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
    //        EditorUtility.DisplayProgressBar("AddAtlasImg", path, i / guids.Length);
    //        List<AtlasImageData> atlasImageDatas = new List<AtlasImageData>();
    //        CollectAllImages(prefab.transform);
    //        for (int j = 0; j < images.Count; j++)
    //        {
    //            string imageId = ToolsUtility.GetImageIdInAtlas(images[j].sprite);
    //            if (string.IsNullOrEmpty(imageId))
    //                continue;
    //            AtlasImageData atlasImageData = new AtlasImageData();
    //            atlasImageData.imageId = imageId;
    //            atlasImageData.image = images[j];
    //            atlasImageDatas.Add(atlasImageData);
    //        }
    //        AtlasImage atlasImage = prefab.GetComponent<AtlasImage>();
    //        if (atlasImageDatas.Count <= 0)
    //        {
    //            if (atlasImage != null)
    //                DestroyImmediate(atlasImage, true);
    //            continue;
    //        }
    //        if (atlasImage == null)
    //            atlasImage = prefab.AddComponent<AtlasImage>();
    //        atlasImage.atlasImageDatas = atlasImageDatas;
    //        PrefabUtility.SavePrefabAsset(prefab);
    //    }
    //    EditorUtility.ClearProgressBar();
    //    AssetDatabase.SaveAssets();
    //}

    //[MenuItem("UITools/SpriteAtlas/给当前选中的界面添加图集脚本", priority = 0)]
    //public static void AddAtlasImgScriptSelect()
    //{
    //    GameObject prefab = Selection.activeGameObject;
    //    if (prefab == null)
    //    {
    //        EditorUtility.DisplayDialog("错误", "请先选中一个界面", "确定");
    //        return;
    //    }
    //    List<AtlasImageData> atlasImageDatas = new List<AtlasImageData>();
    //    CollectAllImages(prefab.transform);
    //    for (int i = 0; i < images.Count; i++)
    //    {
    //        string imageId = ToolsUtility.GetImageIdInAtlas(images[i].sprite);
    //        if (string.IsNullOrEmpty(imageId))
    //            continue;
    //        AtlasImageData atlasImageData = new AtlasImageData();
    //        atlasImageData.imageId = imageId;
    //        atlasImageData.image = images[i];
    //        atlasImageDatas.Add(atlasImageData);
    //    }
    //    AtlasImage atlasImage = prefab.GetComponent<AtlasImage>();
    //    if (atlasImageDatas.Count <= 0)
    //    {
    //        if (atlasImage != null)
    //            DestroyImmediate(atlasImage, true);
    //    }
    //    else
    //    {
    //        if (atlasImage == null)
    //            atlasImage = prefab.AddComponent<AtlasImage>();
    //        atlasImage.atlasImageDatas = atlasImageDatas;
    //    }
    //    PrefabUtility.SavePrefabAsset(prefab);
    //    AssetDatabase.SaveAssets();
    //}

    //[MenuItem("UITools/SpriteAtlas/删除所有界面图集脚本", priority = 2)]
    //public static void DelAtlasImgScriptAll()
    //{
    //    string[] guids = AssetDatabase.FindAssets("t:Prefab", prefabPaths);
    //    for (int i = 0; i < guids.Length; ++i)
    //    {
    //        string path = AssetDatabase.GUIDToAssetPath(guids[i]);
    //        GameObject prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
    //        EditorUtility.DisplayProgressBar("DelAtlasImg", path, i / guids.Length);
    //        AtlasImage atlasImage = prefab.GetComponent<AtlasImage>();
    //        if (atlasImage != null)
    //            DestroyImmediate(atlasImage, true);
    //        PrefabUtility.SavePrefabAsset(prefab);
    //    }
    //    EditorUtility.ClearProgressBar();
    //    AssetDatabase.SaveAssets();
    //}

    [MenuItem("UITools/把Toggle改成AsToggle")]
    public static void ChangeToggleToAsToggle()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab", allPrefabPaths);
        for (int i = 0; i < guids.Length; ++i)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            GameObject prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            EditorUtility.DisplayProgressBar("ChangeToggle", path, i / guids.Length);
            bool changed = false;
            Toggle[] allToggles = prefab.GetComponentsInChildren<Toggle>(true);
            for (int j = 0; j < allToggles.Length; j++)
            {
                if (allToggles[j].GetType() == typeof(AsToggle))
                    continue;
                GameObject toggleObj = allToggles[j].gameObject;
                ToggleGroup toggleGroup = allToggles[j].group;
                Graphic graphic = allToggles[j].graphic;
                DestroyImmediate(allToggles[j], true);
                AsToggle asToggle = toggleObj.AddComponent<AsToggle>();
                asToggle.graphic = graphic;
                asToggle.group = toggleGroup;
                changed = true;
            }
            if (changed)
                PrefabUtility.SavePrefabAsset(prefab);
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.SaveAssets();
    }

    [MenuItem("UITools/修改button按钮点击效果")]
    public static void ChangeButtonTransition()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab", allPrefabPaths);
        for (int i = 0; i < guids.Length; ++i)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            GameObject prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            EditorUtility.DisplayProgressBar("修改button按钮点击效果", path, i / guids.Length);
            Button[] allButtons = prefab.GetComponentsInChildren<Button>(true);
            if (allButtons.Length <= 0)
                continue;
            bool changed = false;
            for (int j = 0; j < allButtons.Length; j++)
            {
                if (allButtons[j].transition != Selectable.Transition.None)
                {
                    allButtons[j].transition = Selectable.Transition.None;
                    changed = true;
                }
            }
            if (changed)
                PrefabUtility.SavePrefabAsset(prefab);
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.SaveAssets();
    }
}
