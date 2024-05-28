using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SpriteMgr
{
    public const string atlas_admission = "atlas_admission";
    public const string atlas_battle = "atlas_battle";
    public const string atlas_cameraguidance = "atlas_cameraguidance";
    public const string atlas_common = "atlas_common";
    public const string atlas_loading = "atlas_loading";
    public const string atlas_login = "atlas_login";
    public const string atlas_mainview = "atlas_mainview";
    public const string atlas_mapselection = "atlas_mapselection";
    public const string atlas_rankview = "atlas_rankview";
    public const string atlas_settingview = "atlas_settingview";
    public const string atlas_start = "atlas_settingview";

    public Sprite LoadSpriteFromAdmission(string spriteName) => GetAtlasSprite(atlas_admission, spriteName);
    public Sprite LoadSpriteFromBattle(string spriteName) => GetAtlasSprite(atlas_battle, spriteName);
    public Sprite LoadSpriteFromCameraGuidance(string spriteName) => GetAtlasSprite(atlas_cameraguidance, spriteName);
    public Sprite LoadSpriteFromCommon(string spriteName) => GetAtlasSprite(atlas_common, spriteName);
    public Sprite LoadSpriteFromLoading(string spriteName) => GetAtlasSprite(atlas_loading, spriteName);
    public Sprite LoadSpriteFromLogin(string spriteName) => GetAtlasSprite(atlas_login, spriteName);
    public Sprite LoadSpriteFromMainView(string spriteName) => GetAtlasSprite(atlas_mainview, spriteName);
    public Sprite LoadSpriteFromMapSelection(string spriteName) => GetAtlasSprite(atlas_mapselection, spriteName);
    public Sprite LoadSpriteFromRankView(string spriteName) => GetAtlasSprite(atlas_rankview, spriteName);
    public Sprite LoadSpriteFromSettingView(string spriteName) => GetAtlasSprite(atlas_settingview, spriteName);
    public Sprite LoadSpriteFromstart(string spriteName) => GetAtlasSprite(atlas_start, spriteName);
}
