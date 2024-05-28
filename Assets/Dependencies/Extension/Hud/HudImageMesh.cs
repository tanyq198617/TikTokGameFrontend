using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HudImageMesh : HudMeshBase//, IAsyncSpriteModifier
{
    public enum Type
    {
        Simple,
        FillRaial360
    }

    [SerializeField, SetProperty("spriteName")]
    private string _spriteName = "";
    public string spriteName
    {
        get { return _spriteName; }
        set
        {
#if !UNITY_EDITOR
                if (_spriteName != value)
#endif
            {
                _spriteName = value;
                _dirty = true;
            }
        }
    }

    [SerializeField, SetProperty("height")]
    private float _height = 1;
    public float height
    {
        get { return _height; }
        set
        {
#if !UNITY_EDITOR
                if (_height != value)
#endif
            {
                _height = value;
                _dirty = true;
            }
        }
    }

    [SerializeField, SetProperty("imageType")]
    private Type _imageType = Type.Simple;
    public Type imageType
    {
        get { return _imageType; }
        set
        {
#if !UNITY_EDITOR
                if (_imageType != value)
#endif
            {
                _imageType = value;
                _dirty = true;
            }
        }
    }

    [SerializeField, SetProperty("percent"), Range(0, 1)]
    private float _percent = 1f;
    public float percent
    {
        get { return Mathf.Clamp01(_percent); }
        set
        {
#if !UNITY_EDITOR
                if (_percent != value)
#endif
            {
                _percent = value;
                _dirty = true;
            }
        }
    }

    private Material material
    {
        get
        {
            if (_renderOrder == HudImageRenderOrder.Back)
                return HudSpriteMgr.Instance.backMaterial;

            if (_renderOrder == HudImageRenderOrder.Normal)
                return HudSpriteMgr.Instance.normalMaterial;

            if (_renderOrder == HudImageRenderOrder.Front)
                return HudSpriteMgr.Instance.frontMaterial;

            return default(Material);
        }
    }

    [SerializeField, SetProperty("renderOrder")]
    private HudImageRenderOrder _renderOrder = HudImageRenderOrder.Normal;
    public HudImageRenderOrder renderOrder
    {
        get { return _renderOrder; }
        set
        {
#if !UNITY_EDITOR
                if (_renderOrder != value)
#endif
            {
                _renderOrder = value;

                meshRenderer.sharedMaterial = material;
            }
        }
    }

    void Start()
    {
        meshRenderer.sharedMaterial = material;
        _dirty = true;
    }

    public override void UpdateGraphics()
    {
        if (percent < 0.001f)
        {
            meshRenderer.enabled = false;
            return;
        }
        if (!meshRenderer.enabled)
        {
            meshRenderer.enabled = true;
        }
        DoGenerateMesh();
    }

    void DoGenerateMesh()
    {
        toFill.Clear();

        var spriteInfo = HudSpriteMgr.Instance.FindSpriteInfo(_spriteName);
        if (spriteInfo == null)
            return;
        
        _bounds.center = Vector3.zero;
        _bounds.size = new Vector3(spriteInfo.widthInPixel * height / spriteInfo.heightInPixel, height, 0);
        var extents = _bounds.extents;

        if (_imageType == Type.Simple)
        {
            s_Xy[0] = new Vector3(-extents.x, extents.y, extents.z);
            s_Xy[1] = new Vector3(-extents.x + _bounds.size.x * percent, extents.y, extents.z);
            s_Xy[2] = new Vector3(-extents.x + _bounds.size.x * percent, -extents.y, extents.z);
            s_Xy[3] = new Vector3(-extents.x, -extents.y, extents.z);

            s_Uv[0] = new Vector2(spriteInfo.x, spriteInfo.y + spriteInfo.height); // 左上角
            s_Uv[1] = new Vector2(spriteInfo.x + spriteInfo.width * _percent, spriteInfo.y + spriteInfo.height); // 右上角
            s_Uv[2] = new Vector2(spriteInfo.x + spriteInfo.width * percent, spriteInfo.y); // 右下角
            s_Uv[3] = new Vector2(spriteInfo.x, spriteInfo.y); // 左下角
            AddQuad(toFill, s_Xy, color, s_Uv);
        }
        else if (_imageType == Type.FillRaial360)
        {
            for (int corner = 0; corner < 4; ++corner)
            {
                float fx0, fx1, fy0, fy1;

                if (corner < 2)
                {
                    fx0 = 0f;
                    fx1 = 0.5f;
                }
                else
                {
                    fx0 = 0.5f;
                    fx1 = 1f;
                }

                if (corner == 0 || corner == 3)
                {
                    fy0 = 0f;
                    fy1 = 0.5f;
                }
                else
                {
                    fy0 = 0.5f;
                    fy1 = 1f;
                }
                s_Xy[0].x = Mathf.Lerp(-extents.x, -extents.x + _bounds.size.x, fx0);
                s_Xy[1].x = s_Xy[0].x;
                s_Xy[2].x = Mathf.Lerp(-extents.x, -extents.x + _bounds.size.x, fx1);
                s_Xy[3].x = s_Xy[2].x;

                s_Xy[0].y = Mathf.Lerp(-extents.y, -extents.y + _bounds.size.y, fy0);
                s_Xy[1].y = Mathf.Lerp(-extents.y, -extents.y + _bounds.size.y, fy1);
                s_Xy[2].y = s_Xy[1].y;
                s_Xy[3].y = s_Xy[0].y;

                s_Uv[0].x = Mathf.Lerp(spriteInfo.x, spriteInfo.x + spriteInfo.width, fx0);
                s_Uv[1].x = s_Uv[0].x;
                s_Uv[2].x = Mathf.Lerp(spriteInfo.x, spriteInfo.x + spriteInfo.width, fx1);
                s_Uv[3].x = s_Uv[2].x;

                s_Uv[0].y = Mathf.Lerp(spriteInfo.y, spriteInfo.y + spriteInfo.height, fy0);
                s_Uv[1].y = Mathf.Lerp(spriteInfo.y, spriteInfo.y + spriteInfo.height, fy1);
                s_Uv[2].y = s_Uv[1].y;
                s_Uv[3].y = s_Uv[0].y;

                float val = percent * 4f - ((corner + 2) % 4);
                if (RadialCut(s_Xy, s_Uv, Mathf.Clamp01(val), true, ((corner + 2) % 4)))
                {
                    AddQuad(toFill, s_Xy, color, s_Uv);
                }
            }
        }
        toFill.FillMesh(mesh);
    }

    private bool RadialCut(Vector3[] xy, Vector3[] uv, float fill, bool invert, int corner)
    {
        // Nothing to fill
        if (fill < 0.001f) return false;

        // Even corners invert the fill direction
        if ((corner & 1) == 1) invert = !invert;

        // Nothing to adjust
        if (!invert && fill > 0.999f) return true;

        // Convert 0-1 value into 0 to 90 degrees angle in radians
        float angle = Mathf.Clamp01(fill);
        if (invert) angle = 1f - angle;
        angle *= 90f * Mathf.Deg2Rad;

        // Calculate the effective X and Y factors
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);

        RadialCut(xy, cos, sin, invert, corner);
        RadialCut(uv, cos, sin, invert, corner);
        return true;
    }

    /// <summary>
    /// Adjust the specified quad, making it be radially filled instead.
    /// </summary>

    private void RadialCut(Vector3[] xy, float cos, float sin, bool invert, int corner)
    {
        int i0 = corner;
        int i1 = ((corner + 1) % 4);
        int i2 = ((corner + 2) % 4);
        int i3 = ((corner + 3) % 4);

        if ((corner & 1) == 1)
        {
            if (sin > cos)
            {
                cos /= sin;
                sin = 1f;

                if (invert)
                {
                    xy[i1].x = Mathf.Lerp(xy[i0].x, xy[i2].x, cos);
                    xy[i2].x = xy[i1].x;
                }
            }
            else if (cos > sin)
            {
                sin /= cos;
                cos = 1f;

                if (!invert)
                {
                    xy[i2].y = Mathf.Lerp(xy[i0].y, xy[i2].y, sin);
                    xy[i3].y = xy[i2].y;
                }
            }
            else
            {
                cos = 1f;
                sin = 1f;
            }

            if (!invert) xy[i3].x = Mathf.Lerp(xy[i0].x, xy[i2].x, cos);
            else xy[i1].y = Mathf.Lerp(xy[i0].y, xy[i2].y, sin);
        }
        else
        {
            if (cos > sin)
            {
                sin /= cos;
                cos = 1f;

                if (!invert)
                {
                    xy[i1].y = Mathf.Lerp(xy[i0].y, xy[i2].y, sin);
                    xy[i2].y = xy[i1].y;
                }
            }
            else if (sin > cos)
            {
                cos /= sin;
                sin = 1f;

                if (invert)
                {
                    xy[i2].x = Mathf.Lerp(xy[i0].x, xy[i2].x, cos);
                    xy[i3].x = xy[i2].x;
                }
            }
            else
            {
                cos = 1f;
                sin = 1f;
            }

            if (invert) xy[i3].y = Mathf.Lerp(xy[i0].y, xy[i2].y, sin);
            else xy[i1].x = Mathf.Lerp(xy[i0].x, xy[i2].x, cos);
        }
    }

    private void AddQuad(VertexHelper vertexHelper, Vector3[] quadPositions, Color32 color, Vector3[] quadUVs)
    {
        int startIndex = vertexHelper.currentVertCount;

        for (int i = 0; i < 4; ++i)
            vertexHelper.AddVert(quadPositions[i], color, quadUVs[i]);

        vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
        vertexHelper.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
    }

    private void AddQuad(VertexHelper vertexHelper, Vector2 posMin, Vector2 posMax, Color32 color, Vector2 uvMin, Vector2 uvMax)
    {
        int startIndex = vertexHelper.currentVertCount;

        vertexHelper.AddVert(new Vector3(posMin.x, posMin.y, 0), color, new Vector2(uvMin.x, uvMin.y));
        vertexHelper.AddVert(new Vector3(posMin.x, posMax.y, 0), color, new Vector2(uvMin.x, uvMax.y));
        vertexHelper.AddVert(new Vector3(posMax.x, posMax.y, 0), color, new Vector2(uvMax.x, uvMax.y));
        vertexHelper.AddVert(new Vector3(posMax.x, posMin.y, 0), color, new Vector2(uvMax.x, uvMin.y));

        vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
        vertexHelper.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
    }
}