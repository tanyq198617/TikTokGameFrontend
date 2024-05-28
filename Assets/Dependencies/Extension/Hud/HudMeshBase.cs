using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HudMeshBase : HudLayoutElement
{
    [SerializeField]
    protected MeshRenderer _renderer = null;
    protected MeshRenderer meshRenderer
    {
        get
        {
            if (_renderer == null)
            {
                _renderer = GetComponent<MeshRenderer>();
            }

            return _renderer;
        }
    }

    [SerializeField]
    private MeshFilter _meshFilter = null;
    public MeshFilter meshFilter
    {
        get
        {
            if (_meshFilter == null)
            {
                _meshFilter = GetComponent<MeshFilter>();
            }

            return _meshFilter;
        }
    }

    protected Mesh _mesh = null;
    protected Mesh mesh
    {
        get
        {
            if (_mesh == null)
            {
                _mesh = new Mesh();
                _mesh.name = name;
            }

            return _mesh;
        }
    }
    protected VertexHelper toFill;
    protected Vector3[] s_Xy = new Vector3[4];
    protected Vector3[] s_Uv = new Vector3[4];

    [SerializeField, SetProperty("dirty")]
    protected bool _dirty = true;
    public bool dirty
    {
        get { return _dirty; }
        set { _dirty = value; }
    }

    [SerializeField, SetProperty("color")]
    private Color _color = Color.white;
    public Color color
    {
        get { return _color; }
        set
        {
            _color = value;

            _dirty = true;
        }
    }

    void Awake()
    {
        meshFilter.mesh = mesh;
        toFill = new VertexHelper();
        _dirty = true;
    }

    void LateUpdate()
    {
        UpdateGraphicsDirty();
        UpdateLayoutDirty();
    }

    void UpdateGraphicsDirty()
    {
        if (_dirty)
        {
            UpdateGraphics();
            _dirty = false;
            _layoutDirty = true;
        }
    }

    public virtual void UpdateGraphics()
    {

    }
}