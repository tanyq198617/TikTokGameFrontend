using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HudImageRenderOrder
{
    Back = 0,
    Normal = 2,
    Front = 3
};

public class HudSpriteInfo
{
    public float x = 0f;
    public float y = 0f;
    public float width = float.Epsilon;
    public float height = float.Epsilon;
    public float widthInPixel = 1;
    public float heightInPixel = 1;
    public Sprite sprite;
}

public class HudSpriteMgr : Singleton<HudSpriteMgr>
{
    // 图集路径
    string _hudAtlasAssetId = "";
    
    // sprites
    Dictionary<string, HudSpriteInfo> _spriteDic = new Dictionary<string, HudSpriteInfo>();

    // 绘制最底层
    Material _backMaterial = null;
    public Material backMaterial
    {
        get
        {
            if (_backMaterial == null)
            {
                _backMaterial = new Material(Shader.Find("Mobile/Particles/Alpha Blended"));
                _backMaterial.renderQueue = 3101; 
                _backMaterial.name = "Back";
            }

            return _backMaterial;
        }
    }

    // 绘制正常层
    Material _normalMaterial = null;
    public Material normalMaterial
    {
        get
        {
            if (_normalMaterial == null)
            {
                _normalMaterial = new Material(Shader.Find("Mobile/Particles/Alpha Blended"));
                _normalMaterial.renderQueue = 3102;
                _normalMaterial.name = "Normal";
            }

            return _normalMaterial;
        }
    }

    // 绘制最前
    Material _frontMaterial = null;
    public Material frontMaterial
    {
        get
        {
            if (_frontMaterial == null)
            {
                _frontMaterial = new Material(Shader.Find("Mobile/Particles/Alpha Blended"));
                _frontMaterial.renderQueue = 3103;
                _frontMaterial.name = "Front";
            }

            return _frontMaterial;
        }
    }
/*
    public void LoadSpriteInfos(string assetId)
    {
        _hudAtlasAssetId = assetId;

        var atlas = SpriteMgr.Instance.Load(_hudAtlasAssetId);
        var sprites = new Sprite[atlas.spriteCount];
        atlas.GetSprites(sprites);

        if (sprites.Length > 0)
        {
            backMaterial.SetTexture("_MainTex", sprites[0].texture);
            normalMaterial.SetTexture("_MainTex", sprites[0].texture);
            frontMaterial.SetTexture("_MainTex", sprites[0].texture);
        }

        for (int i = 0; i < sprites.Length; ++i)
        {
            var sprite = sprites[i];
            var config = CreateNewSpriteInfo(sprite);
            var id = sprite.name.Replace("(Clone)", "");
            _spriteDic.Add(id, config);
        }
    }
*/
    // 提前计算好每个sprite的uv、长宽
    HudSpriteInfo CreateNewSpriteInfo(Sprite sprite)
    {
        var spriteInfo = new HudSpriteInfo();

        spriteInfo.x = sprite.textureRect.x / sprite.texture.width;
        spriteInfo.y = sprite.textureRect.y / sprite.texture.height;
        spriteInfo.width = sprite.textureRect.width / sprite.texture.width;
        spriteInfo.height = sprite.textureRect.height / sprite.texture.height;
        spriteInfo.widthInPixel = sprite.textureRect.width;
        spriteInfo.heightInPixel = sprite.textureRect.height;
        spriteInfo.sprite = sprite;

        return spriteInfo;
    }

    public HudSpriteInfo FindSpriteInfo(string name)
    {
        HudSpriteInfo sprite = null;
        if (_spriteDic.TryGetValue(name, out sprite))
            return sprite;

        return sprite;
    }
}