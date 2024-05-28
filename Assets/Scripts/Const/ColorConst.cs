using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorConst
{
    public const string FitnessTargetColor = "#6B7685";

    public static Color ChoiseImageColor = GetColorByHex("#77B4FF");
    public static Color UnChoiseImageColor = GetColorByHex("#F5F5F5");

    public static Color ChoiseTextColor = Color.white;
    public static Color UnChoiseTextColor = GetColorByHex("#6B7984");


    public static Color ActiveTextColor = GetColorByHex("#193451");
    public static Color UnActiveTextColor = GetColorByHex("#CCCCCC");

    /// <summary> 根据16进制颜色值获取RGB </summary>
    public static Color GetColorByHex(string hexColr)
    {
        Color newColor;
        ColorUtility.TryParseHtmlString(hexColr, out newColor);
        return newColor;
    }
}
