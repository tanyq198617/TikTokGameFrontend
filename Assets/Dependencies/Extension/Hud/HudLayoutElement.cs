using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudLayoutElement : MonoBehaviour
{
    protected Bounds _bounds = new Bounds();
    public Bounds bounds
    {
        get
        {
            return _bounds;
        }
    }

    [SerializeField, SetProperty("layoutDirty")]
    protected bool _layoutDirty = true;
    public bool layoutDirty
    {
        get { return _layoutDirty; }
        set { _layoutDirty = value; }
    }

    [SerializeField, SetProperty("minHeight")]
    protected float _minHeight;
    public float minHeight
    {
        get { return _minHeight; }
        set { _minHeight = value; }
    }

    public void UpdateLayoutDirty()
    {
        if (_layoutDirty)
        {
            UpdateSelfLayout();
            UpdateParentLayout();

            _layoutDirty = false;
        }
    }

    public float GetHeight()
    {
        return Mathf.Max(_bounds.size.y, minHeight);
    }

    public virtual void UpdateSelfLayout()
    {
    }

    public virtual void UpdateParentLayout()
    {
        if (transform.parent == null)
            return;

        var layout = transform.parent.GetComponent<HudLayoutElement>();
        if (layout != null)
            layout.layoutDirty = true;
    }
}