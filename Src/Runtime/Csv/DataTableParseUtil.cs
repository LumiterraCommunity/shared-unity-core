/*
 * @Author: xiang huan
 * @Date: 2022-05-20 10:05:57
 * @LastEditTime: 2022-08-12 20:30:01
 * @LastEditors: Please set LastEditors
 * @Description: 
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Aot/Csv/DataTableParseUtil.cs
 * 
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public static class DataTableParseUtil
{
    /// <summary>
    /// 取消 air table 等外部中的一些无效字符 使之有效
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ValidString(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        if (value[0] == '[' && value[^1] == ']')
        {
            return value[1..^1];
        }

        return value;
    }

    public static string ParseString(string value)
    {
        value = ValidString(value);
        return value;
    }

    public static bool ParseBool(string value)
    {
        value = ValidString(value);
        return !(string.IsNullOrEmpty(value) || value.Equals("0"));
    }

    public static short ParseShort(string value)
    {
        value = ValidString(value);
        bool isParse = short.TryParse(value, out short result);
        return isParse ? result : (short)0;
    }

    public static int ParseInt(string value)
    {
        value = ValidString(value);
        if (string.IsNullOrEmpty(value) || value == "-")//- 是江哥要求的有的 我们不用
        {
            return 0;
        }

        if (int.TryParse(value, out int result))
        {
            return result;
        }
        else
        {
            if (float.TryParse(value, out float resultFloat))//air table 不特殊处理会int会输出 100.0这种小数
            {
                return (int)resultFloat;
            }
            else
            {
                Log.Error($"ParseInt error, value:{value}");
                return 0;
            }
        }
    }

    public static long ParseLong(string value)
    {
        value = ValidString(value);
        bool isParse = long.TryParse(value, out long result);
        return isParse ? result : 0;
    }

    public static double ParseDouble(string value)
    {
        value = ValidString(value);
        bool isParse = double.TryParse(value, out double result);
        return isParse ? result : 0;
    }


    public static Color32 ParseColor32(string value)
    {
        string[] splitValue = value.Split(',');
        return new Color32(byte.Parse(splitValue[0]), byte.Parse(splitValue[1]), byte.Parse(splitValue[2]), byte.Parse(splitValue[3]));
    }

    public static Color ParseColor(string value)
    {
        string[] splitValue = value.Split(',');
        return new Color(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]), float.Parse(splitValue[3]));
    }

    public static Quaternion ParseQuaternion(string value)
    {
        string[] splitValue = value.Split(',');
        return new Quaternion(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]), float.Parse(splitValue[3]));
    }

    public static Rect ParseRect(string value)
    {
        string[] splitValue = value.Split(',');
        return new Rect(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]), float.Parse(splitValue[3]));
    }

    public static Vector2 ParseVector2(string value)
    {
        string[] splitValue = value.Split(',');
        return new Vector2(float.Parse(splitValue[0]), float.Parse(splitValue[1]));
    }

    public static Vector3 ParseVector3(string value)
    {
        string[] splitValue = value.Split(',');
        return new Vector3(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]));
    }

    public static Vector4 ParseVector4(string value)
    {
        string[] splitValue = value.Split(',');
        return new Vector4(float.Parse(splitValue[0]), float.Parse(splitValue[1]), float.Parse(splitValue[2]), float.Parse(splitValue[3]));
    }

    public static object ChangeType<T>(string value)
    {
        switch (typeof(T).Name)
        {
            case nameof(String): return value;
            case nameof(Boolean): return ParseBool(value);
            case nameof(Int16): return ParseShort(value);
            case nameof(Int32): return ParseInt(value);
            case nameof(Int64): return ParseLong(value);
            case nameof(Double): return ParseDouble(value);
            default:
                return Convert.ChangeType(value, typeof(T));
        }
    }
    public static T[] ParseArray<T>(string value)
    {
        string[] valueList = value.Split(',');
        List<T> values = new();
        if (!string.IsNullOrEmpty(value) && valueList != null)
        {
            for (int i = 0; i < valueList.Length; i++)
            {
                values.Add((T)ChangeType<T>(valueList[i]));
            }
        }
        return values.ToArray();
    }

    public static T[][] ParseArrayList<T>(string value)
    {
        string[] valuesList = value.Split(';');
        List<T[]> arrayList = new();
        if (!string.IsNullOrEmpty(value) && valuesList != null)
        {
            for (int i = 0; i < valuesList.Length; i++)
            {
                T[] values = ParseArray<T>(valuesList[i]);
                arrayList.Add(values);
            }
        }
        return arrayList.ToArray();
    }
}

