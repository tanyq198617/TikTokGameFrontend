using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FormatHelper
{ 
    static string[] Numbers = new string[] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };
    static string[] Units = new string[] { "", "十", "百", "千", "万", "十", "百", "千", "亿", "十","百","千" };

    public static string Number2Chinese(long n)
    {
        if (n >=1000000000000||n<0)
        {
            return n.ToString();    
        }

        string result = "";
        string s = n.ToString();
        int zero = 0;//连续出现的零的个数
        int length = s.Length;
        for (int i = 0; i < length; i++)
        {
            int number = s[i] - '0';
            int unit = length - i - 1;
            //一十X、一十X万、一十X亿，一十X万亿开头时,省略开头的一
            if ((length + 2) % 4 == 0 && i == 0 && number == 1 && zero == 0)
            {
                result += Units[unit];
            }
            //不为零时直接输出数字+单位
            else if (number != 0)
            {
                result += Numbers[number] + Units[unit];
                zero = 0;
            }
            //为零且在最后一位时，如果前面有连续零，去掉前面的零，否则不操作
            else if (unit == 0)
            {
                if (zero > 0) result = result.Substring(0, result.Length - 1);
                else if (length == 1) result += Numbers[0];
            }
            //是亿等级时，必然打出亿。
            //是万等级时，如果没有亿则必然打出万，如果有亿但整个万级别不都是零也打出万
            else if (unit == 8 || (unit == 4 && (length <= 8 || zero < 3)))
            {
                if (zero > 0)
                {
                    result = result.Substring(0, result.Length - 1);
                }
                zero = 0;//重置zero
                result += Units[length - i - 1];
            }
            //中间普通的零
            else
            {
                if (zero == 0)
                {
                    result += Numbers[0];
                }
                zero++;
            }
        }
        return result;
    }

    /// <summary>
    /// 1.最多显示4个数字
    /// 过万转万,过亿转亿
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static string Number4Chinese(this long longNumber)
    {
        if (longNumber < 10000)
        {
            return longNumber.ToString();
        }
        else if(longNumber <= 100000000)
        {
            var a = longNumber / (double)10000;
            return StringSplit1(a) + "万";
        }else
        {
            var a = longNumber / (double)100000000;
            return StringSplit1(a) + "亿";
        }
    }
    private static string StringSplit1(double _number)
    {
        var _number1 = _number.ToString();
        var arr = _number.ToString().Split('.');
        var str = _number1;
        if(arr.Length == 1)
            return arr[0];
        else if (arr[0].Length >= 4)
        {
            return arr[0];
        }
        else
        {
            var xiaoShuWeiShu = 4 - arr[0].Length;
            if (arr[1].Length <= 0)
                str = arr[0];
            else if (arr[1].Length >= xiaoShuWeiShu)
                str = arr[0] + "." + arr[1].Substring(0, xiaoShuWeiShu);
            else
                str = arr[0] + "." + arr[1];
        }
        
        return str;
    }
    
    /// <summary>
    /// 1.最多显示3个数字 2.超过千则转换为中文单位万
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static string Number3Chinese(long longNumber)
    {
        float number = (float)longNumber;
        if (number < 1000)
        {
            return number.ToString();
        }
        else if(number < 10000000)
        {
            float a = number / 10000;
            return SetString(a) + "万";
        }else
        {
            float a = number / 100000000;
            return SetString(a) + "亿";
        }
    }

    /// <summary>
    /// 不四舍五入保留两位小数
    /// </summary>
    /// <param name="money"></param>
    /// <returns></returns>
    private static string SetString(float _number)
    {
        if (_number < 10)
           return StringSplit(_number, 2);
        else if(_number < 100)
            return StringSplit(_number, 1);
        else
            return ((int) _number).ToString();
    }
  
    private static string StringSplit(float _number, int weishu)
    {
        string _number1 = _number.ToString();
        var arr = _number.ToString().Split('.');
        string str = _number1;
        if (arr.Length == 2)
        {
            if (arr[1].Length <= 0)
                str = arr[0];
            else if (arr[1].Length >= weishu)
                str = arr[0] + "." + arr[1].Substring(0, weishu);
            else
                str = arr[0] + "." + arr[1];
        }
        return str;
    }
}
