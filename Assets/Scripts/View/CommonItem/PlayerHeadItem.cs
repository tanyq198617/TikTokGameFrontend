using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 通用玩家头像脚本 /// </summary>
public class PlayerHeadItem : AItemPageBase
{
    private RawImage rawImage_head;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        rawImage_head = UIUtility.GetComponent<RawImage>(m_gameobj, "rawImage_head");
    }

    /// <summary> 设置玩家头像 /// </summary>
    public void SetPlayerHead(PlayerInfo info)
    {
        IsActive = true;
        if (info.TexHead == null)
            LoadHeadTexture(info.avatar_url).Forget();
        else
            UIUtility.Safe_UGUI(ref rawImage_head, info.TexHead, true);
    }

    /// <summary> 排行榜加载头像 /// </summary>
    public async UniTask LoadHeadTexture(string url)
    {
        UIUtility.Safe_UGUI(ref rawImage_head, null, true);
        if (rawImage_head == null)
        {
            Debug.LogError($"[当前排行榜] 头像组件为空 playIcon == null");
            return;
        }

        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError($"[当前排行榜] 取头像传递url为空,使用系统头像！");
            url = TextureMgr.editorUrl;
        }

        var texture = await TextureMgr.Instance.GetTexture(url);
        UIUtility.Safe_UGUI(ref rawImage_head, texture);
    }
}