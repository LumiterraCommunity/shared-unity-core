using UnityEngine;

public static class RectExtension
{
    /// <summary>
    /// 包含另外一个矩形 目标矩形的任意点都在本矩形内 包括边界
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static bool Contains(this Rect rect, Rect target)
    {
        if (target == null)
        {
            return false;
        }

        if (target.xMin >= rect.xMin && target.xMax <= rect.xMax && target.yMin >= rect.yMin && target.yMax <= rect.yMax)
        {
            return true;
        }

        return false;
    }
}