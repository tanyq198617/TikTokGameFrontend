#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class DevelopSceneChange
{
    private const string initScenePath = "Assets/Scenes/Init.unity";
    private const string hotfixScenePath = "Assets/HotUpdateResources/Scenes";

    static DevelopSceneChange() => ChangeDevelopScene();

    public static void ChangeDevelopScene()
    {
        Dictionary<string, EditorBuildSettingsScene> editorBuildSettingsScenes = new Dictionary<string, EditorBuildSettingsScene>();

        string[] scenes = Directory.GetFiles(hotfixScenePath, "*.unity", SearchOption.AllDirectories);

        editorBuildSettingsScenes.Add(initScenePath, new EditorBuildSettingsScene(initScenePath, true));

        var config = GameObject.FindObjectOfType<BootConfig>();
        if (config == null || config.PlayMode == YooAsset.EPlayMode.EditorSimulateMode)
        {
            foreach (string scene in scenes)
                editorBuildSettingsScenes.Add(scene, new EditorBuildSettingsScene(scene, true));
        }
        EditorBuildSettings.scenes = editorBuildSettingsScenes.Values.ToArray();
    }
}
#endif
