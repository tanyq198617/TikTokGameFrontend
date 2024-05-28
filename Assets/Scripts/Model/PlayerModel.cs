using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Text;
using Cysharp.Threading.Tasks;
using HotUpdateScripts;
using SprotoType;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 玩家数据
/// </summary>
public class PlayerModel : Singleton<PlayerModel>
{
    /// <summary> key=openId, value={角色数据} </summary>
    private readonly Dictionary<string, PlayerInfo> playerDict = new Dictionary<string, PlayerInfo>();

    /// <summary> key=阵营,value=角色信息 </summary>
    private readonly Dictionary<int, List<PlayerInfo>> campDict = new Dictionary<int, List<PlayerInfo>>();

    /// <summary> 用户集合 </summary>
    private readonly List<PlayerInfo> playerList = new List<PlayerInfo>();

    /// <summary> 缓存的用户集合 </summary>
    private readonly List<PlayerInfo> tempList = new List<PlayerInfo>();

    /// <summary> 缓存的红蓝方的连胜数， key=阵营，1=红,2=蓝 </summary>
    private readonly Dictionary<int, long> winningTreakDic = new Dictionary<int, long>();
    
    /// <summary> 缓存的红蓝方的阵营总积分， key=阵营，1=红,2=蓝 </summary>
    private readonly Dictionary<int, long> campScoreDic = new Dictionary<int, long>();

    /// <summary> 排行榜大哥，最多3个 </summary>
    private readonly Dictionary<int, List<PlayerInfo>> richMainDict = new Dictionary<int, List<PlayerInfo>>();
    
    /// <summary> 起始积分, 为上一局的新增积分 </summary>
    public long originalScore { get; private set; }

    /// <summary> 总积分，角色刷了多少礼物，则获得对应积分 </summary>
    public long totalScore { get; private set; }

    //神龙炸弹数据结构
    private FairyDragonBallCfg fairyDragonBallCfg;
    private FairyDragonBallGiveGiftDataStruct _FairyDragonBallGiveGiftData;

    public void initialize()
    {
        totalScore = originalScore = LocalDataMgr.GetInt(nameof(originalScore));
        int redKey = CampType.红.ToInt();
        int blueKey = CampType.蓝.ToInt();
        winningTreakDic[redKey] = 0;
        winningTreakDic[blueKey] = 0;
        richMainDict[redKey] = new List<PlayerInfo>();
        richMainDict[blueKey] = new List<PlayerInfo>();
        campScoreDic[redKey] = 0;
        campScoreDic[blueKey] = 0;
        fairyDragonBallCfg = TGlobalDataManager.Instance.fairyDragonBallCfg;
        _FairyDragonBallGiveGiftData =  TGlobalDataManager.Instance._FairyDragonBallGiveGiftData;
    }

    /// <summary>
    /// 创建玩家
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public PlayerInfo GetOrCreate(douyin_choose_side msg, bool isfake)
    {
        if (!playerDict.TryGetValue(msg.openid, out var info))
        {
            info = PlayerInfo.Create(msg, isfake);
            playerList.Add(info);
            playerDict.Add(msg.openid, info);
            
            //存阵营字典
            var camp = info.campType.ToInt();
            if (!campDict.TryGetValue(camp, out _))
                campDict[camp] = new List<PlayerInfo>();
            campDict[camp].Add(info);
            
            //玩家位置序列 index
            int sitIndex = campDict[camp].Count;
            
            //异步初始化
            OnIntPlayer(camp, info, msg.win_combo, sitIndex).Forget();
        }
        return info;
    }

    private async UniTask OnIntPlayer(int camp, PlayerInfo info, long win_combo, int sitIndex)
    {
        //等待加载头像
        await info.CheckTextureAsync();
        
        if(CityGlobal.Instance.IsOver) return;

        //连胜数
        if (info.win_lock == 0)
            ChangeCombo(info.campType, win_combo, null);
            
        //存总价值
        ChangeMoney(info,0, 0);

        //添加到阵营字典
        PlayerInfoAddCampDict(sitIndex, camp, info);
        
        //坐下
        EventMgr.Dispatch(UIEvent.GiftSideView_side_tip, info);
    }

    /// <summary> 添加玩家数据到阵营字典中,小弟头像显示30个,取玩家坐下的第4位到34位,前3名是3个大哥的数据,大哥的数据会变,小弟的不会 /// </summary>
    private void PlayerInfoAddCampDict(int sitIndex, int camp, PlayerInfo info)
    {
        if (sitIndex > 3 && sitIndex <= TGlobalDataManager.YoungerBrotherHeadCount)
        {
            info.SitIndex = sitIndex;
            EventMgr.Dispatch(BattleEvent.Battle_Add_YoungerBrother, sitIndex, info);
        }
    }
    
    /// <summary>
    /// 查找某个角色
    /// </summary>
    public PlayerInfo FindPlayer(string openid)
    {
        playerDict.TryGetValue(openid, out var info);
        return info;
    }

    /// <summary>
    /// 获得集合列表，先后加入顺序排序
    /// </summary>
    public List<PlayerInfo> GetPlayerList() => playerList;

    /// <summary>
    /// 获得一种排序的集合
    /// </summary>
    public List<PlayerInfo> GetPlayerListBySort(Comparison<PlayerInfo> comparison)
    {
        tempList.Clear();
        tempList.AddRange(playerDict.Values);
        tempList.Sort(comparison);
        return tempList;
    }

    /// <summary>
    /// 通过皇帝排名排序
    /// 名次小的排前面，名字大的排后面
    /// </summary>
    public static int SortByking(PlayerInfo p1, PlayerInfo p2)
    {
        if (p1.king < p2.king)
            return -1;
        if (p1.king > p2.king)
            return 1;
        return 1;
    }
    
    /// <summary>
    /// 通过赠送礼物价值排序
    /// 钱多的大哥排前面，钱少的大哥排后面
    /// </summary>
    public static int SortByMoney(PlayerInfo p1, PlayerInfo p2)
    {
        if (p1.totalMoney > p2.totalMoney)
            return -1;
        if (p1.totalMoney < p2.totalMoney)
            return 1;
        return 1;
    }

    /// <summary>
    /// 通过小皇帝排名排序
    /// 名次小的排前面，名字大的排后面
    /// </summary>
    public static int SortBySmallKing(PlayerInfo p1, PlayerInfo p2)
    {
        if (p1.small_king < p2.small_king)
            return -1;
        if (p1.small_king > p2.small_king)
            return 1;
        return 1;
    }

    /// <summary>
    /// 通过刷礼物多少排名排序
    /// 多的排前面，少的排后面
    /// </summary>
    public static int SortByTotalGift(PlayerInfo p1, PlayerInfo p2)
    {
        if (p1.totalgift > p2.totalgift)
            return -1;
        if (p1.totalgift < p2.totalgift)
            return 1;
        return 1;
    }

    /// <summary>
    /// 通过持有的连胜次数排序
    /// 多的排前面，少的排后面
    /// </summary>
    public static int SortByWinCombo(PlayerInfo p1, PlayerInfo p2)
    {
        if (p1.win_combo > p2.win_combo)
            return -1;
        if (p1.win_combo < p2.win_combo)
            return 1;
        return 1;
    }

    /// <summary>
    /// 增加击杀数据
    /// </summary>
    public void AddKillHP(string openid, int killhp)
    {
        if (playerDict.TryGetValue(openid, out var info))
        {
            info.AddKillHP(killhp);
            EventMgr.Dispatch(BattleEvent.Battle_Player_ValueChanged, openid);
        }
    }

    /// <summary>
    /// 获得上报数据
    /// </summary>
    public List<Record> GetRecords()
    {
        CampType winCampType = LevelGlobal.Instance.IsRedWin ? CampType.红 : CampType.蓝;
        var list = new List<Record>();
        foreach (var kv in playerDict)
        {
            var playerInfo = kv.Value;
           
            if (playerInfo.campType == winCampType)
            {
                //设置游戏结束时获得积分池里面的积分数量(临时缓存用)
                var campScore = campScoreDic[playerInfo.campType.ToInt()];
                var poolScore = campScore > 0 ? (playerInfo.totalMoney * 1.0f / campScore) * totalScore : 0;
                playerInfo.poolScore = (long)Math.Round(poolScore, 0);
                Debuger.Log($"玩家单局瓜分积分池数据,玩家名字:{playerInfo.nickname},瓜分积分:{playerInfo.poolScore}, poolScore={poolScore}");
            }

            /*var _score = playerInfo.SelfScore + playerInfo.poolScore;
            var value = _score < 0 ? 0 : _score;
            if (value <= 0) value = 0;*/

            var value = CalculateScore(playerInfo);
            if(playerInfo.isFake)
                continue;
            list.Add(new Record()
            {
                openid = kv.Key,
                count = value
            });
        }

        return list;
    }

    /// <summary>计算玩家的上报积分/// </summary>
    private long CalculateScore(PlayerInfo info)
    {
        long _score = info.SelfScore + info.poolScore; 
        Debuger.Log($"玩家名字:{info.nickname},玩家的初始上报积分;{_score}");
        
        if (_score <= 0)
            return 0;
             
        var winCombo = info.init_win_combo * TGlobalDataManager.WinningStreakLiftStruct.scoerLift;
        if (winCombo >= TGlobalDataManager.WinningStreakLiftStruct.maxScoerLift)
            winCombo = TGlobalDataManager.WinningStreakLiftStruct.maxScoerLift;
      
        var kongTouScore = info.GetTotal("5") * TGlobalDataManager.KongTouLiftStruct.scoerLift;
        if (kongTouScore >= TGlobalDataManager.KongTouLiftStruct.maxScoerLift)
            kongTouScore = TGlobalDataManager.KongTouLiftStruct.maxScoerLift;
        
        info.LogMessageMembers($"[{info.nickname}({info.openid})]");
        
        Debuger.Log($"玩家名字:{info.nickname}, 玩家的计算后积分;{(long)(_score * (1 + ((winCombo + kongTouScore) / 100)))}," +
                        $"连胜数:{info.init_win_combo}, 连胜单个比例:{ TGlobalDataManager.WinningStreakLiftStruct.scoerLift}," +
                        $"空投数量:{info.GetTotal("5")}, 空投单个比例:{TGlobalDataManager.KongTouLiftStruct.scoerLift}," +
                        $"点赞次数：{info.like_num}，赠送礼物总个数：{info.totalgift} " +
                        $"自身积分:{info.SelfScore}, 算法：({_score} * (1 + ({winCombo}+{kongTouScore}) /100))");
        
        return (long)(_score * (1 + ((winCombo + kongTouScore) / 100)));
    }

    public void Clear()
    {
        foreach (var kv in playerDict)
        {
            kv.Value.Clear();
        }

        playerDict.Clear();
        campDict.Clear();
        playerList.Clear();
        tempList.Clear();
        winningTreakDic.Clear();
        campScoreDic.Clear();
        richMainDict.Clear();
        LocalDataMgr.SetInt(nameof(originalScore), (int)((totalScore - originalScore) * 0.5f));
    }

    /// <summary>
    /// 随机一个角色
    /// </summary>
    public PlayerInfo RandomPlayer(int camp)
    {
        return campDict.TryGetValue(camp, out var list) ? list[Random.Range(0, list.Count)] : null;
    }

    /// <summary>
    /// 获取阵营所有玩家角色
    /// </summary>
    public List<PlayerInfo> GetCampPlayers(int camp)
    {
        return campDict.TryGetValue(camp, out var list) ? list : new List<PlayerInfo>();
    }
    /// <summary>
    /// 连胜数发生改变
    /// </summary>
    private void ChangeCombo(CampType camp, long win_combo, PlayerInfo info)
    {
        //锁连胜不加
        if(info != null && info.win_lock != 0)
            return;
        var key = camp.ToInt();
        var combo = info?.win_combo ?? 0;
        var changed = win_combo - combo;
        // Debuger.Log($"刷新连胜数：win_combo={win_combo}"); 
        var isDispatch = changed != 0 || info == null;
        if (changed != 0)
        {
            //大于0为增，小于0为减,等于0则是不变 
            long total = winningTreakDic[key] + changed;
            if (total < 0) total = 0;
            winningTreakDic[key] = total;
            // Debuger.Log($"连胜数发生改变：total={total}"); 
        };
        
        if (isDispatch)
        {
            EventMgr.Dispatch(BattleEvent.Battle_WinningTreak_Changed, camp, GetWinningTreak(camp));
            Debuger.Log(info == null
                ? $"新角色[入场] 消息推送win_combo={win_combo}  当前={combo}  增加数={changed}  总连胜数={winningTreakDic[key]} 显示连胜数={GetWinningTreak(camp)}"
                : $"角色[{info.openid}] 消息推送win_combo={win_combo}  当前={combo}  增加数={changed}  总连胜数={winningTreakDic[key]} 显示连胜数={GetWinningTreak(camp)}");
        }
    }

    //玩家连胜数发生改变
    private bool IsChangeCombo(CampType camp, long win_combo, PlayerInfo info)
    {
        //锁连胜不加
        if(info != null && info.win_lock != 0)
            return false;
        var combo = info?.win_combo ?? 0;
        Debuger.Log($"连胜数发生改变 消息推送win_combo={win_combo} 当前={combo}玩家openid:{info.openid}");
        return combo != win_combo;
    }
    /// <summary> 检查阵营前3名的大哥连胜变化 /// </summary>
    private void CheckRichManWinning(PlayerInfo info)
    {
        if (info == null)
            return;
        if (info.SitIndex >= 3) return;
        if (richMainDict.TryGetValue(info.campType.ToInt(), out var list))
            EventMgr.Dispatch(BattleEvent.Battle_RickMan_Changed, info.campType, list);
    }
    
    /// <summary>
    /// 获得连胜数,四舍五入
    /// </summary>
    public long GetWinningTreak(CampType camp)
    {
        var key = camp.ToInt();
        winningTreakDic.TryGetValue(key, out long value);
        return Mathf.RoundToInt(value * 0.3f); 
    }

    /// <summary>
    /// 积分发生改变
    /// </summary>
    private void ChangeScore(long score)
    {
        this.totalScore += score;
        // Debuger.Log($"当前总积分：{totalScore}");
        EventMgr.Dispatch(UIEvent.BattleView_RestScore, totalScore);
    }


    /// <summary>
    /// 价格发生改变
    /// </summary>
    private void ChangeMoney(PlayerInfo info, long number, int price)
    {
        int campType = info.campType.ToInt();
        int money = (int)(number * price);
        
        campScoreDic[campType] += money;
        info.UpdateMoney(money);
        var originalIndex = info.SitIndex;
        
        if (richMainDict.TryGetValue(campType, out var list))
        {
            list ??= new List<PlayerInfo>();

            int index = -1;
            bool isChanged = false;
            var si = list.FindIndex(t => t.openid.Equals(info.openid));
            if (si == 0) return;  //榜一大哥在刷，不用排序
            bool has = si > 0;
            if (has)
            {
                //当前在第几位，只和前面的比
                for (int i = 0; i <= si; i++)
                {
                    if (list[i].totalMoney < info.totalMoney)
                    {
                        isChanged = true;
                        break;
                    }
                }
                //进行一次排序
                if (isChanged)
                {
                    list.Sort(SortByMoney);
                    for (int i = 0; i < list.Count; i++)
                        list[i].SitIndex = i;
                }
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].totalMoney < info.totalMoney)
                    {
                        index = i;
                    }
                }
                
                if (list.Count < 3) 
                {
                    if (index >= 0)
                    {
                        info.SitIndex = index;
                        list.Insert(index, info);
                    }
                    else
                    {
                        info.SitIndex = list.Count;
                        list.Add(info);
                    }
                    isChanged = true;
                }
                else
                {
                    if (index >= 0)
                    {
                        isChanged = true;
                        info.SitIndex = index;
                        list.Insert(index, info);
                    }
                }
            }

            while (list.Count > 3)
            {
                list.Sort(SortByMoney);
                for (int i = 0; i < list.Count; i++)
                    list[i].SitIndex = i;
                var removed = list[list.Count - 1];
                removed.SitIndex = originalIndex;
                list.RemoveAt(list.Count - 1);
                if (originalIndex > 3)
                {
                    //把要移除的下阵
                    EventMgr.Dispatch(BattleEvent.Battle_Changed_YoungerBrother, originalIndex, removed);
                }
            }
            
            if (isChanged)
            {
                //派发消息：有大哥变化
                EventMgr.Dispatch(BattleEvent.Battle_RickMan_Changed, info.campType, list);

#if UNITY_EDITOR
                return;
                var builder = new StringBuilder();
                builder.AppendLine($"大哥排序：");
                for (int i = 0; i < list.Count; i++)
                {
                    builder.AppendLine($"{i + 1}: OpenID={list[i].openid}, 总价值：{list[i].totalMoney}");
                }
                Debuger.LogError(builder.ToString());
#endif
            }
        }
    }

    /// <summary> 存子弹,发射子弹 /// </summary>
    private void ShotFactoryPlayerShot(PlayerInfo info, TOperateData operateData, long gift_num)
    {
        // 添加小爆炸功能
        if (operateData.Id == TGlobalDataManager.smallExplosionGiftId)
            SmallExplosionController.Instance.AddSmallExplosionData(info,gift_num);

        // 添加雷霆一击功能
        if (operateData.Id == TGlobalDataManager.thunderboltGiftId)
            ThunderboltController.Instance.AddSmallExplosionData(info,gift_num);
        
        if (operateData.Id == fairyDragonBallCfg.giftId)
        {   ShotFairyDragonBall(info, operateData, gift_num);}
        else
        {
            //判断是后显示timeLine
            PlayTimeLine(info, operateData);
            ShotFactory.OnPlayerShot(info, operateData, gift_num);
        }
    }

    /// <summary> 发射神龙炸弹 /// </summary>
    private void ShotFairyDragonBall(PlayerInfo info, TOperateData operateData, long gift_num)
    {
        //兑换后剩余刷炸弹的数量
        int ballCount = (int)gift_num % fairyDragonBallCfg.conversionRation;
        ShotFactory.OnPlayerShot(info, operateData, ballCount);

        //神龙炸弹数量
        int fairyDragonBallCount = (int)gift_num / fairyDragonBallCfg.conversionRation;
        
        //神龙炸弹赠送球球数据
        var _operateData = TOperateDataManager.Instance.GetItem(_FairyDragonBallGiveGiftData.giftId);
        
        //刷新神龙炸弹
        for (int i = 0; i < fairyDragonBallCount; i++)
        {
            FairyDragonBallFactory.GetOrCreate(info);
            ShotFactory.OnPlayerShot(info, _operateData, _FairyDragonBallGiveGiftData.giftNum);
        }

        /// <summary>在播神龙炸弹的时候不播出场timeline/// </summary>
        if (fairyDragonBallCount <= 0)
        {
            //判断是后显示timeLine
            PlayTimeLine(info, operateData);
        }
    }

    /// <summary> 播放TimeLine /// </summary>
    private void PlayTimeLine(PlayerInfo info,TOperateData operateData)
    {
        // 礼物id等于5,就是神秘空投
        if (operateData.Id == 5)
        {
            GiftTimeLineFactory.GetOrCreate(info,1);
        }
        else
        {
            //屏蔽球球的入场,升级timeline(后去打开)
            if(BuffMgr.IsHaveBuff(info.campType) && operateData.jinJieTimeLine > 0)
               BallTimeLineFactory.GetOrCreate(info,operateData.jinJieTimeLine);
            else if(operateData.ruChangTimeLine > 0)
                BallTimeLineFactory.GetOrCreate(info,operateData.ruChangTimeLine);
        }
    }

    /// <summary> 判断玩家是否播点赞提示 /// </summary>    
    private void IsPlayLikeTip(PlayerInfo info)
    {
        if (info.temporary_like_num >= TGlobalDataManager.LikeCount) 
        {
            info.temporary_like_num = 0;
            EventMgr.Dispatch(BattleEvent.Battle_PlayLikeTip, info);
        }
    }
    
    #region 服务器网络通信

    /// <summary>
    /// 收到服务器消息
    /// 创建玩家 
    /// </summary>
    public void S2C_Choose_Side(List<douyin_choose_side> list, bool isfake)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var info = GetOrCreate(list[i], isfake);
            list[i].LogMessageMembers("send_douyin_choose_side.request");
        }
    }

    /// <summary>
    /// 收到服务器消息
    /// 玩家评论
    /// 更新玩家数据 
    /// </summary>
    public void S2C_OnComment(List<douyin_comment> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var msg = list[i];
            if (playerDict.TryGetValue(msg.openid, out var info))
            {
                //连胜数
               var isWinningChange =  IsChangeCombo(info.campType, msg.win_combo, info);
                
                info.UpdateByComment(msg);
                
                //判断玩家连胜有没有变化
                if(isWinningChange) 
                    CheckRichManWinning(info);
                
                //666
                if (msg.content.Equals("666"))
                {
                    //刷礼物表
                    var operateData = TOperateDataManager.Instance.GetItem($"4");
                
                    if (!info.isFake)
                    {
                        //增加积分
                        ChangeScore(operateData.score);
                    }
                    
                    //存角色积分
                    info.AddKillHP((int)(operateData.playerScore * 1));
                
                    //压入弹夹
                    ShotFactory.OnPlayerShot(info, operateData, 1);
                }

                WinningPointExpend(info, msg.content);
                //广播用户数据改变
                EventMgr.Dispatch(BattleEvent.Battle_Player_ValueChanged, msg.openid);
                
                list[i].LogMessageMembers("send_douyin_comment.request");
            }
        }

        //广播收到【玩家评论】
        EventMgr.Dispatch(BattleEvent.Battle_S2C_Comment);
    }
    
    /// <summary> 使用连胜点刷礼物 /// </summary>
    private void WinningPointExpend(PlayerInfo info, string content)
    {
        TWinningPointExpend winningPointData = TWinningPointExpendManager.Instance.GetWinningPointExpend(content);

        if (winningPointData == null)
        {
            Debuger.Log($"没有配置指令数据:玩家openid:{info.openid},评论内容:{content}");
            return;
        }
        S2C_PlayGiftShow(info, winningPointData.giftId, winningPointData.giftNum);
        EventMgr.Dispatch(BattleEvent.Battle_WinningPoint_ExpendTip,
            info,winningPointData);
    }
    

    /// <summary>
    /// 收到服务器消息
    /// 用户刷礼物
    /// 更新玩家数据 
    /// </summary>
    public void S2C_OnGift(List<douyin_gift> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var msg = list[i];
            if (playerDict.TryGetValue(msg.openid, out var info))
            {
                S2C_OnGift(msg, info).Forget();
            }
        }

        //广播收到【用户刷礼物】
        EventMgr.Dispatch(BattleEvent.Battle_S2C_Gift);
    }

    private async UniTask S2C_OnGift(douyin_gift msg, PlayerInfo info)
    {
        //检测图片是否加载成功
        await info.CheckTextureAsync();
        
        if(CityGlobal.Instance.IsOver) return;
        
        //连胜数
       // ChangeCombo(info.campType, msg.win_combo, info);

        //更新角色礼物数据
        info.UpdateByGift(msg);

        //球球先不用这个方法,避免影响到以前的功能,下个版本再说
       // S2C_PlayGiftShow(info, msg.gift_id, msg.gift_num);
       
        //刷礼物表
        var operateData = TOperateDataManager.Instance.GetItem(msg.gift_id);
        

        if (!info.isFake)
        {
            //增加积分
            ChangeScore(operateData.score * msg.gift_num);
        }

        //存总价值
        ChangeMoney(info, msg.gift_num, operateData.price);

        //存角色积分
        info.AddKillHP((int)(operateData.playerScore * msg.gift_num));

        //自动刷礼物
        if (TGlobalDataManager.AutoShot_GiftTotalNum > 0 &&
            msg.gift_id.Equals(TGlobalDataManager.AutoShot_CompareId) &&
            info.GetTotal(TGlobalDataManager.AutoShot_CompareId) >= TGlobalDataManager.AutoShot_GiftTotalNum)
        {
            AutoShotControl.EnterToAutoShot(info);
        }

        //加BUFF
        if (operateData.buff > 0)
        {
            BuffHandler handler = null;
            for (int k = 0; k < msg.gift_num; k++)
            {
                handler = BuffMgr.AddBuff(info.campType, operateData.buff);
            }

            //派发所有BUFF
            var balls = BallFactory.GetAllBall(info.campType);
            for (int j = balls.Count - 1; j >= 0; j--)
                balls[j].OnBuffChanged(handler);
        }

        //存子弹，发射
        //ShotFactory.OnPlayerShot(info, operateData, msg.gift_num);
        ShotFactoryPlayerShot(info, operateData, msg.gift_num);
        
        //礼物影响发射器
        GiftEffectMgr.OnEffect(info.campType, msg.gift_id, msg.gift_num);

        //检查是否首充
        FirstGiftMgr.Instance.CheckFirstly(info);

        //增加护盾
        if (operateData.shield > 0)
        {
            EventMgr.Dispatch(BattleEvent.Battle_AddShield, info.campType, (int)(operateData.shield * msg.gift_num));
        }
        
        //额外送礼物
        GiftExtraMgr.OnExtra(info, msg.gift_id, msg.gift_total);
        
        //广播用户数据改变
        EventMgr.Dispatch(BattleEvent.Battle_Player_ValueChanged, msg.openid);
        EventMgr.Dispatch(UIEvent.GiftSideView_gift_tip, msg.openid, msg.gift_id, msg.gift_num);

        // Debuger.Log($"[{info.campType.ToString()}方] [{msg.openid}] 用户刷礼物：{msg.gift_id}={msg.gift_num}个, 共：{msg.gift_total}个");
        msg.LogMessageMembers("send_douyin_gift.request");
    }

    /// <summary> 播放刷礼物的表现 /// </summary>
    private void S2C_PlayGiftShow(PlayerInfo info,string gift_id,long gift_num)
    {
        //刷礼物表
        var operateData = TOperateDataManager.Instance.GetItem(gift_id);
        

        if (!info.isFake)
        {
            //增加积分
            ChangeScore(operateData.score * gift_num);
        }

        //存总价值
        ChangeMoney(info, gift_num, operateData.price);

        //存角色积分
        info.AddKillHP((int)(operateData.playerScore * gift_num));

        //自动刷礼物
        if (TGlobalDataManager.AutoShot_GiftTotalNum > 0 &&
            gift_id.Equals(TGlobalDataManager.AutoShot_CompareId) &&
            info.GetTotal(TGlobalDataManager.AutoShot_CompareId) >= TGlobalDataManager.AutoShot_GiftTotalNum)
        {
            AutoShotControl.EnterToAutoShot(info);
        }

        //加BUFF
        if (operateData.buff > 0)
        {
            BuffHandler handler = null;
            for (int k = 0; k < gift_num; k++)
            {
                handler = BuffMgr.AddBuff(info.campType, operateData.buff);
            }

            //派发所有BUFF
            var balls = BallFactory.GetAllBall(info.campType);
            for (int j = balls.Count - 1; j >= 0; j--)
                balls[j].OnBuffChanged(handler);
        }

        //存子弹，发射
        //ShotFactory.OnPlayerShot(info, operateData, msg.gift_num);
        ShotFactoryPlayerShot(info, operateData, gift_num);

        //礼物影响发射器
        GiftEffectMgr.OnEffect(info.campType, gift_id, gift_num);

        //检查是否首充
        FirstGiftMgr.Instance.CheckFirstly(info);

        //增加护盾
        if (operateData.shield > 0)
        {
            EventMgr.Dispatch(BattleEvent.Battle_AddShield, info.campType, (int)(operateData.shield * gift_num));
        }
    }
    
    /// <summary>
    /// 收到服务器消息
    /// 用户点赞
    /// 更新玩家数据 
    /// </summary>
    public void S2C_OnLike(List<douyin_like> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var msg = list[i];
            if (playerDict.TryGetValue(msg.openid, out var info))
            {
                S2C_OnLike(msg, info).Forget();
            }
        }

        //广播收到【用户点赞】
        EventMgr.Dispatch(BattleEvent.Battle_S2C_Like);
    }

    private async UniTask S2C_OnLike(douyin_like msg, PlayerInfo info)
    {
        //检测图片是否加载成功
        await info.CheckTextureAsync();
        
        if(CityGlobal.Instance.IsOver) return;
        
        //连胜数
        //ChangeCombo(info.campType, msg.win_combo, info);
                
        //点赞
        info.UpdateByLike(msg);
        //是否播放点赞提示
        IsPlayLikeTip(info);
                
        //刷礼物表
        var operateData = TOperateDataManager.Instance.GetItem(BattleConst.gift_likeid);
                
        if (!info.isFake)
        {
            //增加积分
            ChangeScore(operateData.score * msg.like_num);
        }

        //存角色积分
        info.AddKillHP((int)(operateData.playerScore * msg.like_num));
                
        //压入弹夹
        ShotFactory.OnPlayerShot(info, operateData, msg.like_num);
                
        //额外送礼物
        GiftExtraMgr.OnExtra(info, BattleConst.gift_likeid, info.like_num);
                
        //广播用户数据改变
        EventMgr.Dispatch(BattleEvent.Battle_Player_ValueChanged, msg.openid);
                
        msg.LogMessageMembers("send_douyin_like.request");
    }
    
    /// <summary>
    /// 收到服务器消息
    /// 连胜内容
    /// 更新玩家数据 
    /// </summary>
    public void S2C_WinComboContent(string openid, long wincombo)
    {
        UpdateWinCombo(openid, wincombo);
        //广播收到【连胜内容】
        EventMgr.Dispatch(BattleEvent.Battle_S2C_WinComboContent);
    }

    /// <summary>
    /// 收到服务器消息
    /// 连胜内容
    /// 更新玩家数据 
    /// </summary>
    public void S2C_RetGameResultWinCombo(List<_res_win_combo> wincombos)
    {
        for (int i = 0; i < wincombos.Count; i++)
        {
            var item = wincombos[i];
            UpdateWinCombo(item.openid, item.win_combo);
        }
    }

    /// <summary>
    /// 更新连胜数据
    /// </summary>
    private void UpdateWinCombo(string openid, long wincombo)
    {
        if (playerDict.TryGetValue(openid, out var info))
        {
            info.win_combo = wincombo;
            //广播用户数据改变
            EventMgr.Dispatch(BattleEvent.Battle_Player_ValueChanged, openid);
        }
    }

    #endregion
}