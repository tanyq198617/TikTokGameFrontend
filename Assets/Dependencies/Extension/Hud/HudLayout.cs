using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HudLayout : HudLayoutElement
{
    public enum LayoutType
    {
        Horizontal = 0,
        Vertical = 1,
    }

    [SerializeField, SetProperty("layoutType")]
    private LayoutType _layoutType = 0;
    public LayoutType layoutType
    {
        get { return _layoutType; }
        set
        {
            _layoutType = value;
            _layoutDirty = true;
        }
    }

    [SerializeField, SetProperty("space")]
    private float _space = 0;
    public float space
    {
        get { return _space; }
        set
        {
            _space = value;
            _layoutDirty = true;
        }
    }

    [SerializeField, SetProperty("offset")]
    protected Vector2 _offset = Vector2.zero;
    public Vector2 offset
    {
        get { return _offset; }
        set
        {
            _offset = value;
            _layoutDirty = true;
        }
    }

    [SerializeField, SetProperty("lookWidth")]
    protected bool _lookWidth = false;
    public bool lookWidth
    {
        get { return _lookWidth; }
        set
        {
            _lookWidth = value;
            _layoutDirty = true;
        }
    }

    [SerializeField, SetProperty("lookNum")]
    private float _lookNum = 0;
    public float lookNum
    {
        get { return _lookNum; }
        set
        {
            _lookNum = value;
            _layoutDirty = true;
        }
    }

    void LateUpdate()
    {
        UpdateLayoutDirty();
    }

    public override void UpdateSelfLayout()
    {
        switch (_layoutType)
        {
            case LayoutType.Horizontal:
                DoHorizontalLayout();
                break;
            case LayoutType.Vertical:
                DoVirticalLayout();
                break;
        }
    }

    void DoHorizontalLayout()
    {
        if (lookWidth)
        {
            DoLookWidthLayout();
            return;
        }

        float width = 0;
        float height = 0;

        for (int i = 0; i < transform.childCount; ++i)
        {
            var hudItem = transform.GetChild(i).GetComponent<HudLayoutElement>();

            if (hudItem && hudItem.gameObject.activeSelf)
            {
                width += hudItem.bounds.size.x + _space; // 多算一次space
                height = Mathf.Max(height, hudItem.GetHeight());// 取最大值
            }
        }

        // 多算了一次space，减掉
        width -= _space;
        _bounds.size = new Vector3(width + _offset.x, height + _offset.y, 0);

        float offX = -width / 2f + _offset.x; // 水平布局时是居中对齐
        float offY = _offset.y;
        for (int i = 0; i < transform.childCount; ++i)
        {
            var hudItem = transform.GetChild(i).GetComponent<HudLayoutElement>();
            if (hudItem && hudItem.gameObject.activeSelf)
            {
                hudItem.transform.localPosition = new Vector3(offX + hudItem.bounds.extents.x, offY, 0);
                offX += hudItem.bounds.size.x + _space;
            }
        }
    }

    void DoLookWidthLayout()
    {
        float itemWidth = 0;
        float height = 0;
        float spaceTotal = 0;

        for (int i = 0; i < transform.childCount; ++i)
        {
            var hudItem = transform.GetChild(i).GetComponent<HudLayoutElement>();

            if (hudItem && hudItem.gameObject.activeSelf)
            {
                itemWidth += hudItem.bounds.size.x;
                height = Mathf.Max(height, hudItem.GetHeight());// 取最大值
                spaceTotal += _space;// 多算一次space
            }
        }
        spaceTotal -= _space;
        float xScale = itemWidth == 0 ? 1 : (lookNum - spaceTotal) / itemWidth;
        _bounds.size = new Vector3(lookNum, height + _offset.y, 0);

        float offX = -lookNum / 2f; // 水平布局时是居中对齐
        float offY = _offset.y;
        for (int i = 0; i < transform.childCount; ++i)
        {
            var hudItem = transform.GetChild(i).GetComponent<HudLayoutElement>();
            if (hudItem && hudItem.gameObject.activeSelf)
            {
                hudItem.transform.localScale = new Vector3(xScale, hudItem.transform.localScale.y, hudItem.transform.localScale.z);
                float hudWidth = hudItem.bounds.size.x * xScale;
                hudItem.transform.localPosition = new Vector3(offX + hudWidth / 2, offY, 0);
                offX += hudWidth + _space;
            }
        }
    }

    void DoVirticalLayout()
    {
        float width = 0;
        float height = 0;

        for (int i = 0; i < transform.childCount; ++i)
        {
            var hudItem = transform.GetChild(i).GetComponent<HudLayoutElement>();

            if (hudItem && hudItem.gameObject.activeSelf)
            {
                width = Mathf.Max(width, hudItem.bounds.size.x);// 取最大值
                height += hudItem.GetHeight() + _space;// 多算一次space
            }
        }

        // 多算了一次space，减掉
        height -= _space;
        _bounds.size = new Vector3(width + _offset.x, height + _offset.y, 0);

        float offX = _offset.x;
        float offY = _offset.y; // 垂直布局时是底部对齐
        for (int i = 0; i < transform.childCount; ++i)
        {
            var hudItem = transform.GetChild(i).GetComponent<HudLayoutElement>();
            if (hudItem && hudItem.gameObject.activeSelf)
            {
                hudItem.transform.localPosition = new Vector3(offX , offY + hudItem.GetHeight() / 2, 0);
                offY += hudItem.GetHeight() + _space;
            }
        }
    }
}