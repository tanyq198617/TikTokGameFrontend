using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SprotoType;
using UnityEngine;

/// <summary>
/// 玩家数据
/// </summary>
public class PlayerInfo
{
    /// <summary> 用户openid </summary>
    public string openid;

    /// <summary> 选边内容 </summary>
    public string content;

    /// <summary> 头像 </summary>
    public string avatar_url;

    /// <summary> 昵称 </summary>
    public string nickname;

    /// <summary> 皇帝排名（不是皇帝为-1）</summary>
    public long king;

    /// <summary> 小皇帝排名（不是小皇帝为-1） </summary>
    public long small_king;

    /// <summary> 世界榜分数 </summary>
    public long world_score;

    /// <summary> 世界榜排名 </summary>
    public long world_rank;

    /// <summary> 连胜的次数,冲冲里面叫开始连胜次数 </summary>
    public long win_combo;

    /// <summary> 家族 </summary>
    public string family;

    /// <summary> 连胜锁定 0否1是 </summary>
    public long win_lock;

    /// <summary> 回流账号 0否1是 </summary>
    public long rejoin_acc;
    

    /// <summary> 点赞数量 </summary>
    public long like_num;
    
    /// <summary> 礼物总数 </summary>
    public long totalgift;
    
    /// <summary> 礼物总价值 </summary>
    public long totalMoney;
    
    /// <summary> 用户刷礼物数量, key=礼物ID, value=礼物总数量 </summary>
    private readonly Dictionary<string, long> giftDict = new Dictionary<string, long>();
    
    /// <summary> 玩家当前阵营 </summary>
    public CampType campType;

    /// <summary> 游戏结束时获得积分池里面的积分数量(临时缓存用) </summary>
    public long poolScore;
    
    /// <summary> 玩家当前击杀血量(积分) </summary>
    public long SelfScore;
    
    /// <summary> 玩家初始连胜数 </summary>
    public long init_win_combo;
    
    /// <summary> 是否是假数据 </summary>
    public bool isFake;

    /// <summary> 大哥位置 </summary>
    public int SitIndex;


    /// <summary> 头像 </summary>
    public Texture TexHead = null;

    /// <summary> 临时点赞数量 /// </summary>
    public long temporary_like_num;

    
    /// <summary>
    /// 创建角色
    /// </summary>
    public static PlayerInfo Create(douyin_choose_side msg, bool isfake)
    {
        var info = ClassFactory.GetOrCreate<PlayerInfo>();
        info.isFake = isfake;
        info.openid = msg.openid;
        info.content = msg.content;
        info.avatar_url = msg.avatar_url;
        info.nickname = msg.nickname;
        info.king = msg.king;
        info.small_king = msg.small_king;
        info.world_score = msg.world_score;
        info.world_rank = msg.world_rank;
        info.win_combo = msg.win_combo;
        info.init_win_combo = msg.win_combo;
        info.family = msg.family;
        info.win_lock = msg.win_lock;
        info.rejoin_acc = msg.rejoin_acc;
        info.giftDict.Clear();
        info.like_num = 0;
        info.totalgift = 0;
        info.totalMoney = 0;
        info.campType = msg.content == "1" ? CampType.蓝 : CampType.红;
        info.poolScore = 0;
        info.SelfScore = 0;
        info.SitIndex = -1;
        info.temporary_like_num = 0;
        info.TexHead = null;
        // Debuger.Log($"玩家名字:{info.nickname},玩家头像url:{info.avatar_url}");
        return info;
    }
    
    /// <summary>
    /// 通过用户评论进行数据更新
    /// </summary>
    public void UpdateByComment(douyin_comment msg)
    {
        this.content = msg.content;
        //this.avatar_url = msg.avatar_url;
        this.nickname = msg.nickname;
        this.king = msg.king;
        this.small_king = msg.small_king;
        this.family = msg.family;
        this.win_combo = msg.win_combo;
    }
    
    /// <summary>
    /// 通用户刷礼物进行数据更新
    /// </summary>
    public void UpdateByGift(douyin_gift msg)
    {
        //this.avatar_url = msg.avatar_url;
        this.nickname = msg.nickname;
        this.king = msg.king;
        this.small_king = msg.small_king;
        this.family = msg.family;
        this.win_combo = msg.win_combo;
        this.totalgift += msg.gift_num;
        this.giftDict[msg.gift_id] = msg.gift_total;
    }
    
    /// <summary>
    /// 通用[用户点赞]进行数据更新
    /// </summary>
    public void UpdateByLike(douyin_like msg)
    {
        //this.avatar_url = msg.avatar_url;
        this.nickname = msg.nickname;
        this.like_num += msg.like_num;
        this.king = msg.king;
        this.small_king = msg.small_king;
        this.family = msg.family;
        this.win_combo = msg.win_combo;
        this.temporary_like_num += msg.like_num;
    }

    /// <summary>
    /// 通用[系统自动点赞]进行数据更新
    /// </summary>
    public void UpdateByAutoLike(int like_num)
    {
        this.like_num += like_num;
        this.temporary_like_num += like_num;
    }

    /// <summary>
    /// 增加总价值
    /// </summary>
    public void UpdateMoney(int money)
    {
        this.totalMoney += money;
    }

    /// <summary>
    /// 记录当前玩家击杀血量
    /// </summary>
    public void AddKillHP(int hp)
    {
        this.SelfScore += hp;
    }

    public long GetTotal(string gift_id)
    {
        giftDict.TryGetValue(gift_id, out var total);
        return total;
    }

    private async UniTask LoadHeadAsync()
    {
        isloading = true;
        TexHead = await TextureMgr.Instance.LoadTextureAsync(avatar_url);
        isloading = false;
    }

    private bool isloading = false;
    public async UniTask CheckTextureAsync()
    {
        if(TexHead != null)
            return;

        if (isloading)
            await UniTask.WaitUntil(() => !isloading);
        else
            await LoadHeadAsync();
    }

    private void RecycleTexture()
    {
        TextureMgr.Instance.Recycle(avatar_url);
        TexHead = null;
    }

    public void Clear()
    {
        RecycleTexture();
        openid = string.Empty;
        content = string.Empty;
        avatar_url = string.Empty;
        nickname = string.Empty;
        king = -1;
        small_king = -1;
        world_score = 0;
        world_rank = 0;
        win_combo = 0;
        family = string.Empty;
        win_lock = 0;
        rejoin_acc = 0;
        giftDict.Clear();
        like_num = 0;
        totalgift = 0;
        poolScore = 0;
        campType = CampType.NUll;
        SelfScore = 0;
        totalMoney = 0;
        init_win_combo = 0;
        SitIndex = -1;
        temporary_like_num = 0; 
        ClassFactory.Recycle(this);
    }
}

