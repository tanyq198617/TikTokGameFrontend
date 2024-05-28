using UnityEngine;

public abstract class AHeiDongEffectBase : ABallEffectBase
{
    public override void OnInit(Vector3 point, float effectTime)
    {
        base.OnInit(point, effectTime);
        AudioMgr.Instance.PlaySoundName(11);
    }
}