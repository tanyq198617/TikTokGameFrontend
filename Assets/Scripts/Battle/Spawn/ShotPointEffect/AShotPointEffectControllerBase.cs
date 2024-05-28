using UnityEngine;

public abstract class AShotPointEffectControllerBase : AItemPageBase
{
    /// <summary> 结算点阵营 /// </summary>
    protected abstract int campType { get; }

    protected Vector3 transformPoint;
    protected bool isPlayEffectIng;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        transformPoint = Trans.position;
    }

    public override void Refresh()
    {
        base.Refresh();
        isPlayEffectIng = false;
    }

    public override void Close()
    {
        base.Close();
        isPlayEffectIng = true;
    }

    protected virtual void AddShotPointEffect(int camp, int ballType, int ballCount)
    {
    }

    protected void EffectBackAction()
    {
        isPlayEffectIng = false;
    }

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener<int, int, int>(BattleEvent.Battle_Ball_ValueChanged, AddShotPointEffect);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener<int, int, int>(BattleEvent.Battle_Ball_ValueChanged, AddShotPointEffect);
    }
}