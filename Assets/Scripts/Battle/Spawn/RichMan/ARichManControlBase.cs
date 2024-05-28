using UnityEngine;

/// <summary>
/// 控制大哥模型的脚本
/// </summary>
public abstract class ARichManControlBase : APoolGameObjectBase
{
    /// <summary> 结算点阵营 /// </summary>
    protected abstract CampType camp { get; }

    public abstract void Recycle();
    
    private Animator _animator;
    public override void setObj(GameObject gameObject)
    {
        base.setObj(gameObject);
        _animator = UIUtility.GetComponent<Animator>(Trans, "dage@skin");
    }

    public void OnInit(Vector3 point)
    {
        SetRotate();
        Trans.position = point;
    }

    /// <summary> 播放大哥的动画 /// </summary>
    public void PlayerAnimator(int type)
    {
        switch (type)
        {
            case 1:
                var tranPoint = Trans.position;
                Trans.position = new Vector3(tranPoint.x, 0, tranPoint.z);
                PlayAnimator("qitiaorenqiu");
                break;
            case 2:
                PlayAnimator("jifei");
                break;
            case 3:
                PlayAnimator("renqiu");
                break;
        }
    }
    
    private void PlayAnimator(string animatorName)
    {
        _animator?.Play(animatorName);
    }
    
    private void SetRotate()
    {
        Trans.eulerAngles = camp == CampType.红 ? Vector3.zero : new Vector3(0, 180, 0);
    }
    
}