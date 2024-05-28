using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class BDir 
{
    public const string Assets_HotUpdateResources_Codes = "Assets/HotUpdateResources/Codes";
    public const string Assets_HotUpdateResources_Prefabs = "Assets/HotUpdateResources/Prefabs";
    public const string Assets_HotUpdateResources_Prefabs_UI = "Assets/HotUpdateResources/Prefabs/UI";
    public const string Assets_HotUpdateResources_Prefabs_Item = "Assets/HotUpdateResources/Prefabs/Item";
    public const string Assets_HotUpdateResources_Prefabs_Battle = "Assets/HotUpdateResources/Prefabs/Battle";
    public const string Assets_HotUpdateResources_Other = "Assets/HotUpdateResources/Other";
    public const string Assets_HotUpdateResources_TextAsset = "Assets/HotUpdateResources/TextAsset";
    public const string Assets_ArtAssets_UI_Texture = "Assets/ArtAssets/UI/Texture";
    public const string Assets_HotUpdateResources_Texture_Map = "Assets/HotUpdateResources/Texture/Map";
    public const string Assets_HotUpdateResources_Scenes = "Assets/HotUpdateResources/Scenes";
    public const string Assets_ArtAssets_Map_Scenes = "Assets/ArtAssets/Map/Scenes";
    public const string Assets_ArtAssets_SpriteAtlas = "Assets/ArtAssets/SpriteAtlas";

    public const string Assets_HotUpdateResources_Audio_Sound = "Assets/HotUpdateResources/Audio";
}

public class BPath
{
    public const string MapItem = "MapItem";
}

/// <summary>
/// 路径常亮
/// </summary>
public class PathConst
{
    public static string GetAtlasPath(string atlasName) => $"{BDir.Assets_ArtAssets_SpriteAtlas}/{atlasName}.spriteatlas";
    public static string GetNinoPath(string path, string name) => Boot.IsEnableAddressable ? name : $"{BDir.Assets_HotUpdateResources_TextAsset}/{path}/{name}.bytes";
    public static string GetScenePath(string scendName) => Boot.IsEnableAddressable ? scendName : $"{BDir.Assets_HotUpdateResources_Scenes}/{scendName}.unity";
    public static string GetArtScenePath(string scendName) => Boot.IsEnableAddressable ? scendName : $"{BDir.Assets_ArtAssets_Map_Scenes}/{scendName}.unity";
    public static string GetUIPath(string uiName) => Boot.IsEnableAddressable ? uiName : $"{BDir.Assets_HotUpdateResources_Prefabs_UI}/{uiName}.prefab";
    public static string GetMapPath(string uiName) => Boot.IsEnableAddressable ? uiName : $"{BDir.Assets_HotUpdateResources_Prefabs_Item}/{uiName}.prefab";
    public static string GetItemPath(string uiName) => Boot.IsEnableAddressable ? uiName : $"{BDir.Assets_HotUpdateResources_Prefabs_Item}/{uiName}.prefab";
    public static string GetBattleItemPath(string uiName) => Boot.IsEnableAddressable ? uiName : $"{BDir.Assets_HotUpdateResources_Prefabs_Battle}/{uiName}.prefab";
    public static string GetMapTexturePath(string uiName) => Boot.IsEnableAddressable ? uiName : $"{BDir.Assets_HotUpdateResources_Texture_Map}/{uiName}.png";
    public static string GetSoundPath(string uiName) => Boot.IsEnableAddressable ? uiName : $"{BDir.Assets_HotUpdateResources_Audio_Sound}/{uiName}.ogg";
}
