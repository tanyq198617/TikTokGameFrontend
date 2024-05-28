using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class Z_SpineText : MonoBehaviour
{
    private SkeletonAnimation _animation;

    public Dictionary<string, Spine.Animation> AnimationDict = new Dictionary<string, Spine.Animation>();

    public List<string> _keys = new List<string>();

    [OnValueChanged("OnSetAnimation")]
    public int index;
    
    void Start()
    {
        // _animation = GetComponent<SkeletonAnimation>();
        // var animations =  _animation.state.Data.skeletonData.animations;
        // foreach (var anin in animations)
        // {
        //     AnimationDict[anin.name] = anin;
        //     _keys.Add(anin.name);
        // }
    }

    [ContextMenu("SetAnimation")]
    public void OnSetAnimation()
    {
        if (index < 0 || index >= _keys.Count)
            return;
        string name = _keys[index];
        if (AnimationDict.TryGetValue(name, out var animation))
        {
            _animation.AnimationName = name;
        }
    }

}
