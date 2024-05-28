using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using YooAsset;

public partial class SpriteMgr : Singleton<SpriteMgr>
{
    private Dictionary<string, SpriteAtlas> allAtlasDic = null;
    private bool isInit = false;

    public void Init()
    {
        if (null == YooAssets.GetPackage(AssetConst.DefaultPackage))
        {
            Debug.LogError("请先初始化 YooAssets");
            return;
        }

        allAtlasDic = new Dictionary<string, SpriteAtlas>();
        isInit = true;
    }

    /// <summary>
    /// ui界面通过imageId获取图片
    /// </summary>
    public Sprite GetSpriteByImgId(string imageId)
    {
        if (!isInit)
        {
            Init();
        }
        if (string.IsNullOrEmpty(imageId))
            return null;

        string[] splitStr = imageId.Split('@');
        string spriteName = splitStr[0];
        string atlasName = splitStr[1];

        return GetAtlasSprite(atlasName, spriteName);
    }

    protected Sprite GetSpriteByName(string spriteName)
    {
        if (string.IsNullOrEmpty(spriteName))
            return null;

        string atlasName = ParseSpriteName(spriteName);
        return GetAtlasSprite(atlasName, spriteName);
    }

    protected Sprite GetAtlasSprite(string atlasName, string spriteName)
    {
        SpriteAtlas spriteAtlas = Load(atlasName);

        if (spriteAtlas == null)
            return null;

        return spriteAtlas.GetSprite(spriteName);
    }


    private string ParseSpriteName(string spriteName)
    {
        string atlasName = string.Empty;
        int index = spriteName.LastIndexOf('_');
        if (index > 0 && index < spriteName.Length - 1)
        {
            atlasName = spriteName.Substring(0, index);
        }
        return atlasName;
    }

    protected SpriteAtlas Load(string atlasName)
    {
        if (!isInit)
        {
            Init();
        }
        SpriteAtlas spriteAtlas;
        if (allAtlasDic.TryGetValue(atlasName, out spriteAtlas))
        {
            return spriteAtlas;
        }
        string path = string.Format("Assets/ArtAssets/SpriteAtlas/{0}.spriteatlas", atlasName);
        spriteAtlas = YooAssets.LoadAssetSync<SpriteAtlas>(path).GetAssetObject<SpriteAtlas>();
        //spriteAtlas = (SpriteAtlas)req.asset;
        allAtlasDic.Add(atlasName, spriteAtlas);
        return spriteAtlas;
    }
}
