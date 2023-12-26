using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 自定义的一些数学工具
/// </summary>
public static class MathUtilCore
{
    /// <summary>
    /// 米转厘米
    /// </summary>
    public static readonly float M2CM = 100;
    /// <summary>
    /// 厘米转米
    /// </summary>
    public static readonly float CM2M = 1 / M2CM;

    /// <summary>
    /// 千分位转整形
    /// </summary>
    public static readonly int T2I = 1000;
    /// <summary>
    /// 整形转千分位
    /// </summary>
    public static readonly float I2T = 1f / T2I;
    /// <summary>
    /// 千分率
    /// </summary>
    public static readonly float PM = 1 / 1000f;
    /// <summary>
    /// 百分率
    /// </summary>
    public static readonly float PC = 1 / 100f;
    /// <summary>
    /// 两个int转成一个ulong 方便将二维坐标转成一个key 类似对之前egret中的r_c的字符串优化
    /// </summary>
    public static ulong TwoIntToUlong(int a, int b)
    {
        return ((ulong)a << 32) | (uint)b;
    }

    /// <summary>
    /// 二维转的key还原回之前的二维坐标
    /// </summary>
    public static (int a, int b) UlongToTwoInt(ulong value)
    {
        return ((int)(value >> 32), (int)value);
    }

    /// <summary>
    /// string转long
    /// </summary>
    public static long StringToLong(string valueStr)
    {
        if (long.TryParse(valueStr, out long value))
        {
            return value;
        }
        return 0;
    }

    /// <summary>
    /// 不同权重的列表中随机一个索引
    /// </summary>
    public static int RandomWeightListIndex(List<int> weightList)
    {

        int total = 0;

        foreach (int elem in weightList)
        {
            total += elem;
        }

        int randomNum = UnityEngine.Random.Range(0, total);

        for (int i = 0; i < weightList.Count; i++)
        {
            if (randomNum < weightList[i])
            {
                return i;
            }
            else
            {
                randomNum -= weightList[i];
            }
        }
        return weightList.Count - 1;
    }

    /// <summary>
    /// 区域ID转土地格子ID
    /// </summary>
    public static ulong AreaToSoil(int areaId, int x, int z)
    {
        return ((ulong)areaId << 32) | ((ulong)x << 16) | (uint)z;
    }

    /// <summary>
    /// 土地格子ID转区域ID
    /// </summary>
    public static int SoilToArea(ulong pointId)
    {
        return (int)(pointId >> 32);
    }

    /// <summary>
    /// 随机一个概率 参数为0~1 返回这次随机是否命中
    /// </summary>
    /// <param name="probability"></param>
    /// <returns></returns>
    public static bool RandomFromProbability1(float probability)
    {
        return UnityEngine.Random.Range(0f, 1f) < probability;
    }

    /// <summary>
    /// 随机一个概率 参数为0~100 返回这次随机是否命中
    /// </summary>
    /// <param name="probability"></param>
    /// <returns></returns>
    public static bool RandomFromProbability100(float probability)
    {
        return UnityEngine.Random.Range(0f, 100f) < probability;
    }
    public static List<T> RandomSortList<T>(List<T> listT)
    {
        for (int i = 0; i < listT.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, listT.Count);
            (listT[randomIndex], listT[i]) = (listT[i], listT[randomIndex]);
        }
        return listT;
    }
    /// <summary>
    /// 线经过球体的交点
    /// </summary>
    /// <param name="sphereCentre">球心位置</param>
    /// <param name="sphereRadius">球半径</param>
    /// <param name="lineRay">直线射线</param>
    /// <returns></returns>
    public static (Vector3?, Vector3?) LineIntersectSphere(Vector3 sphereCentre, float sphereRadius, Ray lineRay)
    {
        Vector3 rayDirection = lineRay.direction;
        Vector3 rayOrigin = lineRay.origin;
        Vector3 oc = rayOrigin - sphereCentre;

        float b = 2 * Vector3.Dot(rayDirection, oc);
        float c = oc.sqrMagnitude - (sphereRadius * sphereRadius);
        float discriminant = (b * b) - (4 * c);

        if (discriminant >= 0)
        {
            float t1 = (-b + Mathf.Sqrt(discriminant)) / 2;
            float t2 = (-b - Mathf.Sqrt(discriminant)) / 2;
            Vector3 intersection1 = rayOrigin + (rayDirection * t1);
            Vector3 intersection2 = rayOrigin + (rayDirection * t2);

            // Debug.Log("Intersection 1: " + intersection1 + " Intersection 2: " + intersection2);
            // Debug.DrawLine(sphereCentre, (Vector3)intersection1, Color.blue);
            // Debug.DrawLine(sphereCentre, (Vector3)intersection2, Color.green);
            return (intersection1, intersection2);
        }
        else
        {
            // Debug.Log("No intersection.");
            return (null, null);
        }
    }

    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Vector3 mid = Vector3.Lerp(start, end, t);
        float y = 4 * ((-height * t * t) + (height * t)) + Mathf.Lerp(start.y, end.y, t);
        return new Vector3(mid.x, y, mid.z);
    }
    public static float BigInteger2Float(System.Numerics.BigInteger bigInteger, int exponent, int decimals)
    {
        double num = (double)(bigInteger / System.Numerics.BigInteger.Pow(10, exponent - decimals));
        float numValue = (float)(num / Math.Pow(10, decimals));
        return numValue;
    }
}