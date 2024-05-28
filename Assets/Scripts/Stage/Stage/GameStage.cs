using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public delegate void LoadFinish(System.Action evt = null);

public abstract class GameStage : IStateNode, IEvent, IProgress<float>
{
    protected StateMachine stageMachine;

    public LoadFinish LoadFinish = null;

    public void OnCreate(StateMachine machine) => stageMachine = machine;


    /// <summary> 进入场景 </summary>
    public void OnEnter()
    {
        Begin();
        AddEventListener();
    }

    /// <summary> 退出场景 </summary>
    public void OnExit()
    {
        RemoveEventListener();
        End();
    }

    public virtual void OnLoadSceneSucceed(string sceneName) { }

    public virtual void Begin() { }
    public virtual void End() { }

    public virtual void SysUpdate() { }
    public virtual void FixedUpdate() { }
    public virtual void LateUpdate() { }
    public virtual void Report(float value) { }

    public abstract string GetSceneName();

    public virtual UniTask LoadSceneAsync(string scendName) => UniTask.CompletedTask;

    public virtual void RendGUI() { }
    public virtual void OnDrawGizmos() { }

    public virtual void AddEventListener() { }
    public virtual void RemoveEventListener() { }
}
