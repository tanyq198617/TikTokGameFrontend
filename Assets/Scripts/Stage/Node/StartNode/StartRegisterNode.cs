using DebugSystem;

public class StartRegisterNode : AStateNode
{
    protected override void Begin()
    {
        Register();
        IsComplete = true;
    }

    private void Register()
    {
        Game.Scene.Register<AtlasMgr>();
        Game.Scene.Register<NetMgr>();
        Game.Scene.Register<SDKMgr>();
        Game.Scene.Register<InputControl>();
        Game.Scene.Register<AudioMgr>(true);

        //注册开发者接口
        RegisterDevelop();
    }

    private void RegisterDevelop()
    {
#if UNITY_EDITOR
        Game.Scene.Register<DevelopHelper>();
        Game.Scene.Register<DebugComponent>();
#else
        bool isDevelopModel = Boot.GetValue<bool>("DevelopModel");
        if (isDevelopModel) Game.Scene.Register<DevelopHelper>();
        
        bool openDebugPanel = Boot.GetValue<bool>("OpenDebugPanel");
        if (openDebugPanel)  Game.Scene.Register<DebugComponent>();
#endif
    }

    public override void SysUpdate()
    {
        if (IsComplete)
            _machine.RunNextNode(this);
    }
}
