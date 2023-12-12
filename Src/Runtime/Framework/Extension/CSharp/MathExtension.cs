using System;
public static class MathExtension
{
    /// <summary>
    /// 近似相等另外一个float unity官方的Mathf.Approximately 不大可靠 有时对比精度超级高
    /// </summary>
    /// <param name="cur"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static bool ApproximatelyEquals(this float cur, float target)
    {
        return MathF.Abs(cur - target) < 0.0001;
    }

    /// <summary>
    /// 将一个double，向下取整保留多少位
    /// </summary>
    /// <param name="value"></param>
    /// <param name="place"></param>
    /// <param name="baseNum"></param>
    /// <returns></returns>
    public static double FloorTo(this double value, int place, int baseNum)
    {
        if (place == 0)
        {
            return Math.Floor(value);
        }
        else
        {
            double p = Math.Pow(baseNum, place);
            return Math.Floor(value * p) / p;
        }
    }
}