using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Cysharp.Threading.Tasks;
using GameNetwork;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

/// <summary>
/// 全局图片管理器
/// </summary>
public class TextureMgr : Singleton<TextureMgr>
{
    private readonly Dictionary<string, Texture> _textureDict = new Dictionary<string, Texture>();
    private readonly Dictionary<string, Sprite> _spriteDict = new Dictionary<string, Sprite>();

    /// <summary> 正在异步加载中 /// </summary>
    private HashSet<string> loading = new HashSet<string>();

    public const string editorUrl =
        "https://p26.douyinpic.com/aweme/100x100/aweme-avatar/tos-cn-i-0813_c8424a86838a4b9c886f98f7ba8b0120.jpeg?from=3067671334";

    public async UniTask<Texture> LoadTextureAsync(string url)
    {
        var key = UEncrypt.MD5Encrypt(url);
        if (!_textureDict.TryGetValue(key, out var texture))
        {
            loading.Add(key);
            texture = await HttpUtility.HttpGetTextureAsync(url);
            _textureDict[key] = texture;
            loading.Remove(key);
        }

        return texture;
    }
    
    public async UniTask<Sprite> LoadSpriteAsync(string url)
    {
        if (!_spriteDict.TryGetValue(url, out var sprite))
        {
            loading.Add(url);
            var texture = await LoadTextureAsync(url);
            sprite = Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            _spriteDict[url] = sprite;
            loading.Remove(url);
        }
        return sprite;
    }

    public void Set(ref RawImage image, string url)
    {
        if(image == null) return;
        if (string.IsNullOrEmpty(url)) 
            url = editorUrl;
        SetAsync(image, url).Forget();
    }

    public async UniTask<Texture> GetTexture(string headurl)
    {
        string url = headurl;
        if (loading.Contains(url))
            await UniTask.WaitUntil(() => !loading.Contains(url));
        var texture = await LoadTextureAsync(url);
        return texture;
    }

    public void Set(ref Image image, string url)
    {
#if UNITY_EDITOR
        url = editorUrl;
#endif
        SetAsync(image, url).Forget();
    }

    public async UniTask SetAsync(RawImage image, string headurl)
    {
        string url = headurl;
        if (loading.Contains(url))
            await UniTask.WaitUntil(() => !loading.Contains(url));
        var texture = await LoadTextureAsync(url);
        if (image == null)
        {
            Debuger.Log($"加载头像失败:{url}");
            return;
        }
        image.texture = texture;
    }

    public async UniTask SetAsync(Image image, string headurl)
    {
        string url = headurl;
        if (loading.Contains(url))
            await UniTask.WaitUntil(() => !loading.Contains(url));
        var sprite = await LoadSpriteAsync(url);
        if (image == null) return;
        image.sprite = sprite;
    }

    public void Release()
    {
        var list = _textureDict.Values.ToList();
        for (int i = 0; i < list.Count; i++)
        {
            Object.Destroy(list[i]);
        }
        
        var sprites = _spriteDict.Values.ToList();
        for (int i = 0; i < sprites.Count; i++)
        {
            Object.Destroy(sprites[i]);
        }

        _textureDict.Clear();
        _spriteDict.Clear();
        list.Clear();
    }

    public void Recycle(string url)
    {
        if (_textureDict.TryGetValue(url, out var texture))
        {
            Object.Destroy(texture);
            _textureDict.Remove(url);
        }
    }
}