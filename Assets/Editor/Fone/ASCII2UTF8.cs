using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ASCII2UTF8 : Editor
{
    [MenuItem("脚本处理/脚本文件ASCII转UTF8")]
    public static void TryASCII2UTF8()
    {
        int index = 0;
        var dir = Application.dataPath;
        foreach (var f in new DirectoryInfo(dir).GetFiles("*.cs", SearchOption.AllDirectories))
        {
            if (GetTextFileEncodingType(f.FullName).Equals(Encoding.UTF8))
            {
                continue;
            }
            string content = File.ReadAllText(f.FullName, Encoding.GetEncoding("gb2312"));
            File.WriteAllText(f.FullName, content, Encoding.UTF8);
            index++;
        }

        Debug.LogError($"成处理{index}个脚本文件");
    }
    
    /// <summary>
    /// 获取文本文件的字符编码类型
    /// </summary>
    private static Encoding GetTextFileEncodingType(string fileName)
    {
        Encoding encoding = Encoding.Default;
        FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        BinaryReader binaryReader = new BinaryReader(fileStream, encoding);
        byte[] buffer = binaryReader.ReadBytes((int)fileStream.Length);
        binaryReader.Close();
        fileStream.Close();
        if (buffer.Length >= 3 && buffer[0] == 239 && buffer[1] == 187 && buffer[2] == 191)
        {
            encoding = Encoding.UTF8;
        }
        else if (buffer.Length >= 3 && buffer[0] == 254 && buffer[1] == 255 && buffer[2] == 0)
        {
            encoding = Encoding.BigEndianUnicode;
        }
        else if (buffer.Length >= 3 && buffer[0] == 255 && buffer[1] == 254 && buffer[2] == 65)            {
            encoding = Encoding.Unicode;
        }
        else if (IsUTF8Bytes(buffer))
        {
            encoding = Encoding.UTF8;
        }
        return encoding;
    }  


    /// <summary> 
    /// 判断是否是不带 BOM 的 UTF8 格式 
    /// </summary> 
    /// <param name=“data“></param> 
    /// <returns></returns> 
    private static bool IsUTF8Bytes(byte[] data)
    {
        int charByteCounter = 1; //计算当前正分析的字符应还有的字节数 
        byte curByte; //当前分析的字节. 
        for (int i = 0; i < data.Length; i++)
        {
            curByte = data[i];
            if (charByteCounter == 1)
            {
                if (curByte >= 0x80)
                {
                    //判断当前 
                    while (((curByte <<= 1) & 0x80) != 0)
                    {
                        charByteCounter++;
                    }

                    //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
                    if (charByteCounter == 1 || charByteCounter > 6)
                    {
                        return false;
                    }
                }
            }
            else
            {
                //若是UTF-8 此时第一位必须为1 
                if ((curByte & 0xC0) != 0x80)
                {
                    return false;
                }

                charByteCounter--;
            }
        }

        if (charByteCounter > 1)
        {
            throw new Exception("非预期的byte格式");
        }

        return true;
    }
}
