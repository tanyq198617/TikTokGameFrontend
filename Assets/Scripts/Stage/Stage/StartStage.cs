/// <summary>
/// 热更开始舞台
/// </summary>
public class StartStage : GameStage
{
    public override string GetSceneName() => string.Empty;

    public override void SysUpdate() => _machine?.SysUpdate();

    private StateMachine _machine;

    public override void Begin()
    {
        if (_machine == null)
        {
            _machine = new StateMachine(this);
            _machine.AddNode<StartAudioNode>();
            _machine.AddNode<StartRegisterNode>();
            _machine.AddNode<StartInitUINode>();
            _machine.AddNode<StartNetNode>();
            _machine.AddNode<StartOverNode>();
        }
        _machine.Run<StartAudioNode>();
        TableRegister.PreLoadAll().Forget();
        EventMgr.Dispatch(GameEvent.Version_NewMessage, $"准备解析文件");
    }
}
