using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HudNumberMesh : HudMeshBase
{
    public enum Alignment
    {
        Left,
        Center
    }

    [SerializeField, SetProperty("alignment")]
    private Alignment _alignment = Alignment.Center;
    public Alignment alignment
    {
        get { return _alignment; }
        set
        {
#if !UNITY_EDITOR
                if (_alignment != value)
#endif
            {
                _alignment = value;
                _dirty = true;
            }
        }
    }

    [SerializeField, SetProperty("prefix")]
    private string _prefix = "";
    public string prefix
    {
        get { return _prefix; }
        set
        {
#if !UNITY_EDITOR
                if (_prefix != value)
#endif
            {
                _prefix = value;
                _dirty = true;
            }
        }
    }

    [SerializeField, SetProperty("number")]
    private string _number = "";
    public string number
    {
        get { return _number; }
        set
        {
#if !UNITY_EDITOR
                if (_number != value)
#endif
            {
                _number = value;
                _dirty = true;
            }
        }
    }

    [SerializeField, SetProperty("characterSpace")]
    private int _characterSpace = 0;
    public int characterSpace
    {
        get { return _characterSpace; }
        set
        {
#if !UNITY_EDITOR
                if (_characterSpace != value)
#endif
            {
                _characterSpace = value;
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

    private StringBuilder sb = new StringBuilder();
    private List<HudSpriteInfo> hudSpriteInfos = new List<HudSpriteInfo>();

    void Start()
    {
        meshRenderer.sharedMaterial = material;
        _dirty = true;
    }

    public override void UpdateGraphics()
    {
        if (string.IsNullOrEmpty(prefix))
            return;
        DoGenerateMesh();
    }

    void DoGenerateMesh()
    {
        toFill.Clear();
        hudSpriteInfos.Clear();
        _bounds.center = Vector3.zero;

        float width = 0;
        float height = 0;
        float xOff = 0;
        if (alignment == Alignment.Left)
        {
            for (int i = 0; i < number.Length; ++i)
            {
                sb.Clear();
                sb.Append(prefix);
                sb.Append(number[i]);
                HudSpriteInfo spriteInfo = HudSpriteMgr.Instance.FindSpriteInfo(sb.ToString());
                if (spriteInfo == null)
                    continue;
                width += spriteInfo.widthInPixel;
                if (height == 0)
                    height = spriteInfo.heightInPixel;

                float yOff = spriteInfo.heightInPixel / 2;
                s_Xy[0] = new Vector3(xOff, yOff, 0);
                s_Xy[1] = new Vector3(xOff + spriteInfo.widthInPixel, yOff, 0);
                s_Xy[2] = new Vector3(xOff + spriteInfo.widthInPixel, -yOff, 0);
                s_Xy[3] = new Vector3(xOff, -yOff, 0);

                var outer = UnityEngine.Sprites.DataUtility.GetOuterUV(spriteInfo.sprite);
                s_Uv[0] = new Vector2(outer.x, outer.w);// 左上角
                s_Uv[1] = new Vector2(outer.z, outer.w);// 右上角
                s_Uv[2] = new Vector2(outer.z, outer.y);// 右下角
                s_Uv[3] = new Vector2(outer.x, outer.y);// 左下角
                xOff += spriteInfo.widthInPixel + characterSpace;
                AddQuad(toFill, s_Xy, color, s_Uv);
            }
        }
        else if (alignment == Alignment.Center)
        {
            for (int i = 0; i < number.Length; ++i)
            {
                sb.Clear();
                sb.Append(prefix);
                sb.Append(number[i]);
                HudSpriteInfo spriteInfo = HudSpriteMgr.Instance.FindSpriteInfo(sb.ToString());
                if (spriteInfo == null)
                    continue;
                hudSpriteInfos.Add(spriteInfo);
                width += spriteInfo.widthInPixel;
                if (height == 0)
                    height = spriteInfo.heightInPixel;
            }
            width += characterSpace * (number.Length - 1);

            xOff = -width / 2;
            for (int i = 0; i < hudSpriteInfos.Count; i++)
            {
                HudSpriteInfo spriteInfo = hudSpriteInfos[i];
                float yOff = spriteInfo.heightInPixel / 2;
                s_Xy[0] = new Vector3(xOff, yOff, 0);
                s_Xy[1] = new Vector3(xOff + spriteInfo.widthInPixel, yOff, 0);
                s_Xy[2] = new Vector3(xOff + spriteInfo.widthInPixel, -yOff, 0);
                s_Xy[3] = new Vector3(xOff, -yOff, 0);

                var outer = UnityEngine.Sprites.DataUtility.GetOuterUV(spriteInfo.sprite);
                s_Uv[0] = new Vector2(outer.x, outer.w);// 左上角
                s_Uv[1] = new Vector2(outer.z, outer.w);// 右上角
                s_Uv[2] = new Vector2(outer.z, outer.y);// 右下角
                s_Uv[3] = new Vector2(outer.x, outer.y);// 左下角
                xOff += spriteInfo.widthInPixel + characterSpace;
                AddQuad(toFill, s_Xy, color, s_Uv);
            }
        }
        _bounds.size = new Vector3(width, height, 0);
        toFill.FillMesh(mesh);
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