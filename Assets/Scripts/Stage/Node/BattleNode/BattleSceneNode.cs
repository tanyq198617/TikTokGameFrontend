using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 战斗场景初始化
/// </summary>
public class BattleSceneNode : AStateNode
{
    private readonly List<UniTask> _tasks = new List<UniTask>();
    
    protected override void Begin()
    {
        _tasks.Clear();
        Initialize().Forget();
    }

    public async UniTask Initialize()
    {
        //创建角色
        
        //创建地图
        _tasks.Add(MapMgr.Instance.LoadMap());
        
        _tasks.Add(GameObjectFactory.PreLoadAsync<RedRichManControl>(PathConst.GetBattleItemPath(BattleConst.RedRichManModel),3));
        _tasks.Add(GameObjectFactory.PreLoadAsync<BlueRichManControl>(PathConst.GetBattleItemPath(BattleConst.BlueRichManModel),3));
      
        //等待所有任务完成
        await UniTask.WhenAll(_tasks);

        CameraMgr.Instance.initialize();
        MapMgr.Instance.Initialize();
        
        //实例化UI
        IsComplete = true;
    }

    public override void SysUpdate()
    {
        if (IsComplete)
            _machine.RunNextNode(this);
    }
}
