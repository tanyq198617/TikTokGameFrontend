using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Layer常量
/// </summary>
public class Layer
{
    /// <summary> 红方球体层 </summary>
    public static readonly int Red = LayerMask.NameToLayer("red");
    
    /// <summary> 蓝方球体层 </summary>
    public static readonly int Blue = LayerMask.NameToLayer("blue");
    
    /// <summary> 红方简单小球层 </summary>
    public static readonly int Red_SmallBall = LayerMask.NameToLayer("redsmall");
    
    /// <summary> 蓝方简单小球层 </summary>
    public static readonly int Blue_SmallBall = LayerMask.NameToLayer("bluesmall");
    
    /// <summary> 红方简单大球层 </summary>
    public static readonly int Red_BigBall = LayerMask.NameToLayer("redbigball");
    
    /// <summary> 蓝方简单大球层 </summary>
    public static readonly int Blue_BigBall = LayerMask.NameToLayer("bluebigball");
    
    /// <summary> 红方药丸球层 </summary>
    public static readonly int Red_PillBall = LayerMask.NameToLayer("redpillball");
    
    /// <summary> 蓝方药丸球层 </summary>
    public static readonly int Blue_PillBall = LayerMask.NameToLayer("bluepillball");
    
    /// <summary> 红方爆炸/黑洞球层 </summary>
    public static readonly int Red_NoColliderBall = LayerMask.NameToLayer("rednocollider");
    
    /// <summary> 蓝方爆炸/黑洞球层 </summary>
    public static readonly int Blue_NoColliderBall = LayerMask.NameToLayer("bluenocollider");
    
    /// <summary> 红方碾压球层 </summary>
    public static readonly int Red_GrindBall = LayerMask.NameToLayer("redgrindball");
    
    /// <summary> 蓝方碾压球层 </summary>
    public static readonly int Blue_GrindBall = LayerMask.NameToLayer("bluegrindball");
    
    
    /// <summary> 红方墙体层 </summary>
    public static readonly int RedWall = LayerMask.NameToLayer("redwall");
    
    /// <summary> 蓝方墙体层 </summary>
    public static readonly int BlueWall = LayerMask.NameToLayer("bluewall");
    
    /// <summary> 红方结算 </summary>
    public static readonly int RedReslut = LayerMask.NameToLayer("redresult");
    
    /// <summary> 蓝方结算 </summary>
    public static readonly int BlueResult = LayerMask.NameToLayer("blueresult");

    
    public static readonly int Wall = 1 << BlueWall | 1 << RedWall;
    public static readonly int Reslut = 1 << BlueResult | 1 << RedReslut;
    
    public static readonly int BlueBall = 1 << Blue | 1 << Blue_SmallBall | 1 << Blue_BigBall | 1 << Blue_PillBall | 1 << Blue_NoColliderBall | 1 << Blue_GrindBall;
    public static readonly int RedBall  = 1 << Red  | 1 << Red_SmallBall  | 1 << Red_BigBall  | 1 << Red_PillBall  | 1 << Red_NoColliderBall  | 1 << Red_GrindBall;
    
    public static readonly int Ball = 1 << Blue | 1 << Blue_SmallBall | 1 << Blue_BigBall | 1 << Blue_PillBall | 1 << Blue_NoColliderBall | 1 << Blue_GrindBall 
                                    | 1 << Red  | 1 << Red_SmallBall  | 1 << Red_BigBall  | 1 << Red_PillBall  | 1 << Red_NoColliderBall  | 1 << Red_GrindBall;

    public static bool IsWall(int layer) => (Wall & 1 << layer) != 0;
    public static bool IsReslut(int layer) => (Reslut & 1 << layer) != 0;
    public static bool IsBall(int layer) => (Ball & 1 << layer) != 0;
    public static bool IsNotBall(bool isred, int layer) => isred ? 
        (Wall & 1 << layer) != 0 || (BlueResult & 1 << layer) != 0 : 
        (Wall & 1 << layer) != 0 || (RedReslut & 1 << layer) != 0;

    public static bool IsReslut(bool isRed, int layer) => isRed ? (1 << BlueResult & 1 << layer) != 0 : (1 << RedReslut & 1 << layer) != 0;
}
