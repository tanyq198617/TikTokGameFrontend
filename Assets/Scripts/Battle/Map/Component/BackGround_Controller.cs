using UnityEngine;

public class BackGround_Controller : MonoBehaviour
{
    private Material material;
    private Camera playerCamera;
    private Vector3 scale;
    
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private bool isHorizontal;
    [SerializeField] private bool isVertical;
    
    //摄像机平滑
    [SerializeField] private Vector3 vt = Vector3.zero;
    [SerializeField] private Vector3 target = Vector3.zero;
    [SerializeField] private bool lockedMove = false;
    
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    void Awake()
    {
        material = GetComponent<Renderer>().material;
        playerCamera = Camera.main;
        this.scale = this.transform.localScale;
    }
    
    public void initialize(Camera camera, Texture2D texture2D, bool isHorizontal, bool isVertical)
    {
        this.playerCamera = camera;
        initialize(texture2D, isHorizontal, isVertical);
    }

    public void initialize(Texture2D texture2D, bool isHorizontal, bool isVertical)
    {
        this.material.SetTexture(MainTex, texture2D);
        this.material.mainTextureOffset = Vector2.zero;
        this.isHorizontal = isHorizontal;
        this.isVertical = isVertical;
        this.transform.position = Vector3.zero;
        this.scale = new Vector3(texture2D.width * 0.01f, texture2D.height * 0.01f, 1);
        this.transform.localScale = scale;
    }

    /// <summary> 移动多少个单位 </summary>
    public void MoveTo(float x, float y)
    {
        target = target != vt ? playerCamera.transform.position : vt;
        target.x += x;
        target.y += y;
        lockedMove = true;
    }

    /// <summary> 设置到某地 </summary>
    public void SetTo(Vector3 pos)
    {
        target = pos;
        lockedMove = true;
    } 

    private void Update()
    {
        if (playerCamera == null)
            return;
        
        var position =
            lockedMove
                ? playerCamera.transform.position =
                    Vector3.SmoothDamp(playerCamera.transform.position, target, ref vt, speed)
                : playerCamera.transform.position;
        var pos = position;
        var cornerPos = playerCamera.ViewportToWorldPoint(new Vector3(1f, 1f, Mathf.Abs(position.z)));
        var leftBorder = position.x - (cornerPos.x - position.x);
        var rightBorder = cornerPos.x;
        var topBorder = cornerPos.y;
        var downBorder = position.y - (cornerPos.y - position.y);
            
        if (!isHorizontal)
        {
            if (leftBorder < -scale.x * 0.5f)
            {
                pos.x = -scale.x * 0.5f + (rightBorder - leftBorder) * 0.5f;
            }
            else if (rightBorder > scale.x * 0.5f)
            {
                pos.x = scale.x * 0.5f - (rightBorder - leftBorder) * 0.5f;
            }
        }

        if (!isVertical)
        {
            if (downBorder < -scale.y * 0.5f)
            {
                pos.y = -scale.y * 0.5f + (topBorder - downBorder) * 0.5f;
            }
            else if (topBorder > scale.y * 0.5f)
            {
                pos.y = scale.y * 0.5f - (topBorder - downBorder) * 0.5f;
            }
        }

        playerCamera.transform.position = pos;
    }

    void LateUpdate()
    {
        var position = playerCamera.transform.position;
        var cornerPos = playerCamera.ViewportToWorldPoint(new Vector3(1f, 1f, Mathf.Abs(position.z)));

        var leftBorder = position.x - (cornerPos.x - position.x);
        var rightBorder = cornerPos.x;
        var topBorder = cornerPos.y;
        var downBorder = position.y - (cornerPos.y - position.y);
        
        var pos = transform.position;
        Vector3 offset = material.mainTextureOffset;
        
        if (leftBorder < -scale.x * 0.5f + transform.position.x)
        {
            var sx = -scale.x * 0.5f;
            var ox = leftBorder - (sx + transform.position.x);
            pos.x += ox;
            offset.x += (ox * speed);
        }
        else if (rightBorder > scale.x * 0.5f + transform.position.x)
        {
            var sx = scale.x * 0.5f;
            var ox = rightBorder - (sx + transform.position.x);
            pos.x += ox;
            offset.x += (ox * speed);
        }


        if (downBorder < -scale.y * 0.5f + transform.position.y)
        {
            var sy = -scale.y * 0.5f;
            var oy = downBorder - (sy + transform.position.y);
            pos.y += oy;
            offset.y += (oy * speed);
        }
        else if (topBorder > scale.y * 0.5f + transform.position.y)
        {
            var sy = scale.y * 0.5f;
            var oy = topBorder - (sy + transform.position.y);
            pos.y += oy;
            offset.y += (oy * speed);
        }
        
        transform.position = pos;
        material.mainTextureOffset = offset;
    }
}