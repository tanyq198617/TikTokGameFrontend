using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FSG.MeshAnimator.ShaderAnimated;
using UnityEngine;

/// <summary>
/// 皮肤控制器
/// </summary>
public class SkinComponent : MonoBehaviour
{
    /// <summary> 皮肤 </summary>
    [Header("皮肤")] [SerializeField] public GameObject[] skins;
    
    private GameObject defaultSkin;
    private GameObject advanceSkin;

    /// <summary> 当前使用皮肤 </summary>
    public GameObject CurSkin;

    private Sequence _tween;
    private Material _sharedMaterial;
    private Material _effectMaterial;
    private ShaderMeshAnimator _animator;

    private static readonly Queue<Material> materialFactory = new Queue<Material>();
    private static readonly HashSet<Material> usingList = new HashSet<Material>();
    
    private static readonly int OutLightStrengh = Shader.PropertyToID("_OutLightStrengh");

    private const string KeyWord_USE_OUT_LIGHT = "USE_OUT_LIGHT"; 

    private void Awake()
    {
        CurSkin = defaultSkin = skins[0];
        advanceSkin = skins.Length > 1 ? skins[1] : null;
        CurSkin.SetActiveEX(true);
        advanceSkin?.SetActiveEX(false);
    }
    
    public void FlashOutLine()
    {
        if (_effectMaterial != null)
            return;
        if (_tween != null)
            return;
        if (CurSkin == null)
            return;
        var render = CurSkin.GetComponent<MeshRenderer>();
        _animator = CurSkin.GetComponent<ShaderMeshAnimator>();
        if (render == null)
            return;
        Clear();
        ClearMaterial();
        
        _sharedMaterial = render.sharedMaterial;
        _effectMaterial = GetOrCreate(_sharedMaterial);
        _effectMaterial.SetFloat(OutLightStrengh, 4);
        render.material = _effectMaterial;
        _animator.ReloadMaterial(true);
   
        _tween = DOTween.Sequence();
        _tween.AppendInterval(0.5f);
        _tween.AppendCallback(() =>
        {
            CloseOutLine();
            Clear();
        });
    }

    public void Clear()
    {
        _tween?.Kill(false);
        _tween = null;
    }

    public void CloseOutLine()
    {
        if(_effectMaterial == null)
            return;
        if(CurSkin == null)
            return;
        var render = CurSkin.GetComponent<MeshRenderer>();
        _animator = CurSkin.GetComponent<ShaderMeshAnimator>();
        if(render == null)
            return;
        render.sharedMaterial = _sharedMaterial;
        _animator.ReloadMaterial(true);
        ClearMaterial();
        
    }

    private void ClearMaterial()
    {
        if (_effectMaterial != null)
        {
            if (usingList.Remove(_effectMaterial))
                materialFactory.Enqueue(_effectMaterial);
            _effectMaterial = null;
        }
    }

    public static Material GetOrCreate(Material material)
    {
        if (materialFactory.Count > 0)
        {
            var result = materialFactory.Dequeue();
            result.CopyPropertiesFromMaterial(material);
            usingList.Add(result);
            return result;
        }
        else
        {
            var result = new Material(material);
            usingList.Add(result);
            return result;
        }
    }

    public void SetAdvanceSkin()
    {
        if(advanceSkin == null) 
            return;
        if (CurSkin != advanceSkin)
        {
            CloseOutLine();
            CurSkin.SetActiveEX(false);
            CurSkin = advanceSkin;
            CurSkin.SetActiveEX(true);
        }
    }

    public void ResetDefaultSkin()
    {
        if (CurSkin != defaultSkin)
        {
            CloseOutLine();
            CurSkin.SetActiveEX(false);
            CurSkin = defaultSkin;
            CurSkin.SetActiveEX(true);
        }
    }
}
