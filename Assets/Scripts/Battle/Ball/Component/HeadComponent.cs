using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 球球显示玩家头像
/// </summary>
public class HeadComponent : AItemPageBase
{
    protected MeshRenderer _renderer;
    
    private static readonly Queue<Material> materialFactory = new Queue<Material>();
    private static readonly HashSet<Material> usingList = new HashSet<Material>();
    
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    private static Material _sharedMaterial = null;
    private Material _effectMaterial;
    
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        _renderer = UIUtility.GetComponent<MeshRenderer>(Trans, "fx_TouXiang_yuan");
        if (_sharedMaterial == null)
            _sharedMaterial = _renderer.sharedMaterial;
    }

    /// <summary>
    /// 渲染头像
    /// </summary>
    public void RenderHead(Texture texture)
    {
        ClearMaterial();
        _effectMaterial = GetOrCreate(_sharedMaterial);
        _effectMaterial.SetTexture(MainTex, texture);
        _renderer.material = _effectMaterial;
    }

    public override void Close()
    {
        base.Close();
        ClearMaterial();
        _renderer.sharedMaterial = _sharedMaterial;
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
    
    private void ClearMaterial()
    {
        if (_effectMaterial != null)
        {
            if (usingList.Remove(_effectMaterial))
                materialFactory.Enqueue(_effectMaterial);
            _effectMaterial = null;
        }
    }
}
