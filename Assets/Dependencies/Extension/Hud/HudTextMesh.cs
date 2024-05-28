using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HudTextMesh : HudMeshBase
{
    [SerializeField, SetProperty("text")]
    private string _text = "";
    public string text
    {
        get { return _text.Length > 0 ? _text : " "; }
        set
        {
#if !UNITY_EDITOR
                if (_text != value)
#endif
            {
                text = value;
                _dirty = true;
            }
        }
    }

    [SerializeField, SetProperty("font")]
    private Font _font;
    public Font font
    {
        get { return _font; }
        set
        {
#if !UNITY_EDITOR
                if (_font != value)
#endif
            {
                _font = value;
                _dirty = true;
            }
        }
    }

    [SerializeField, SetProperty("fontSize")]
    private int _fontSize = 32;
    public int fontSize
    {
        get { return _fontSize; }
        set
        {
#if !UNITY_EDITOR
                if (_fontSize != value)
#endif
            {
                _fontSize = value;
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

    [SerializeField, SetProperty("showOutline")]
    private bool _showOutline = false;
    public bool showOutline
    {
        get { return _showOutline; }
        set
        {
#if !UNITY_EDITOR
                if (_showOutline != value)
#endif
            {
                _showOutline = value;
                _dirty = true;
            }
        }
    }

    [SerializeField, SetProperty("outlineColor")]
    private Color _outlineColor = new Color(0.3f, 0.3f, 0.3f, 1.0f);
    public Color outlineColor
    {
        get { return _outlineColor; }
        set
        {
#if !UNITY_EDITOR
                if (_outlineColor != value)
#endif
            {
                _outlineColor = value;
                _dirty = true;
            }
        }
    }

    [SerializeField, SetProperty("outlineDistance")]
    private float _outlineDistance = 0.8f;
    public float outlineDistance
    {
        get { return _outlineDistance; }
        set
        {
#if !UNITY_EDITOR
                if (_outlineDistance != outlineDistance)
#endif
            {
                _outlineDistance = value;
                _dirty = true;
            }
        }
    }

    private int GetRealFontSize()
    {
        return font.dynamic ? fontSize : font.fontSize;
    }

    // when the font is dynamic ,the scale is 1;else the scale is fontsize/font.fontsize
    private float GetFontScale()
    {
        return font.dynamic ? 1f : (float)fontSize / font.fontSize;
    }

    private List<Vector3> _verts = new List<Vector3>();
    private List<Vector2> _uvs = new List<Vector2>();
    private List<int> _indices = new List<int>();
    private List<Color> _colors = new List<Color>();

    void Start()
    {
        Font.textureRebuilt += OnFontTextureRebuilt;

        _dirty = true;
    }

    void OnFontTextureRebuilt(Font changedFont)
    {
        if (changedFont != font)
            return;

        UpdateGraphics();
    }

    public override void UpdateGraphics()
    {
        if (font)
        {
            if (font.dynamic)
            {
                font.RequestCharactersInTexture(text, GetRealFontSize());
            }

            DoGenerateMesh();
        }
    }

    void DoGenerateMesh()
    {
        _verts.Clear();
        _uvs.Clear();
        _indices.Clear();
        _colors.Clear();
        _bounds.size = Vector3.zero;
        _bounds.center = Vector3.zero;

        float fontScale = GetFontScale();
        float xOff = 0;
        float yOff = 0;
        CharacterInfo ch;
        Vector3 vector = new Vector3();

        for (int i = 0; i < text.Length; ++i)
        {
            bool ret = font.GetCharacterInfo(text[i], out ch, GetRealFontSize());
            if (ret == false)
                ch = default;

            if (showOutline)
            {
                FillSingleWord(fontScale, xOff + outlineDistance, yOff, ref ch, ref vector, outlineColor, 5 * i + 0);
                FillSingleWord(fontScale, xOff, yOff + outlineDistance, ref ch, ref vector, outlineColor, 5 * i + 1);
                FillSingleWord(fontScale, xOff - outlineDistance, yOff, ref ch, ref vector, outlineColor, 5 * i + 2);
                FillSingleWord(fontScale, xOff, yOff - outlineDistance, ref ch, ref vector, outlineColor, 5 * i + 3);

                FillSingleWord(fontScale, xOff, yOff, ref ch, ref vector, color, 5 * i + 4);
            }
            else
                FillSingleWord(fontScale, xOff, yOff, ref ch, ref vector, color, i);

            xOff += ch.advance * fontScale + characterSpace;
        }

        for (int i = 0; i < _verts.Count; ++i)
        {
            _verts[i] = _verts[i] - _bounds.center;
        }

        mesh.Clear();
        mesh.SetVertices(_verts);
        mesh.SetUVs(0, _uvs);
        mesh.SetColors(_colors);
        mesh.SetTriangles(_indices, 0);
    }

    private void FillSingleWord(float fontScale, float xOff, float yOffset, ref CharacterInfo ch, ref Vector3 vector, Color color, int chIndex)
    {
        vector.Set(xOff + ch.minX * fontScale, (ch.maxY + yOffset) * fontScale, 0);
        _verts.Add(vector);
        _bounds.Encapsulate(vector);

        vector.Set(xOff + ch.maxX * fontScale, (ch.maxY + yOffset) * fontScale, 0);
        _verts.Add(vector);
        _bounds.Encapsulate(vector);

        vector.Set(xOff + ch.maxX * fontScale, (ch.minY + yOffset) * fontScale, 0);
        _verts.Add(vector);
        _bounds.Encapsulate(vector);

        vector.Set(xOff + ch.minX * fontScale, (ch.minY + yOffset) * fontScale, 0);
        _verts.Add(vector);
        _bounds.Encapsulate(vector);

        _uvs.Add(ch.uvTopLeft);
        _uvs.Add(ch.uvTopRight);
        _uvs.Add(ch.uvBottomRight);
        _uvs.Add(ch.uvBottomLeft);

        for (int k = 0; k < _colors.Count; ++k)
        {
            _colors[k] = color;
        }

        _indices.Add(chIndex * 4);
        _indices.Add(chIndex * 4 + 1);
        _indices.Add(chIndex * 4 + 2);
        _indices.Add(chIndex * 4);
        _indices.Add(chIndex * 4 + 2);
        _indices.Add(chIndex * 4 + 3);
    }

    void OnDestroy()
    {
        Font.textureRebuilt -= OnFontTextureRebuilt;
    }
}