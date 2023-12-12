using UnityEngine;

public static class VectorExtension
{
    /// <summary>
    /// 两个三维向量近似相等
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool ApproximatelyEquals(this Vector3 a, Vector3 b)
    {
        return a.x.ApproximatelyEquals(b.x) && a.y.ApproximatelyEquals(b.y) && a.z.ApproximatelyEquals(b.z);
    }

    public static bool ApproximatelyEquals(this Vector2 a, Vector2 b)
    {
        return a.x.ApproximatelyEquals(b.x) && a.y.ApproximatelyEquals(b.y);
    }

    /// <summary>
    /// 返回一个新的向量，但是只有当前向量的Y分量
    /// </summary>
    public static Vector3 OnlyY(this Vector3 vector3)
    {
        Vector3 res = vector3;
        res.x = 0.0f;
        res.z = 0.0f;

        return res;
    }


    /// <summary>
    /// 返回一个新的向量，但是只有当前向量的XZ分量
    /// </summary>
    public static Vector3 OnlyXZ(this Vector3 vector3)
    {
        Vector3 res = vector3;
        res.y = 0.0f;

        return res;
    }
}