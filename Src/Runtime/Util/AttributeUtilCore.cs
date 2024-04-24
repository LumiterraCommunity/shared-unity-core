/// <summary>
/// 属性工具类
/// </summary>
public static class AttributeUtilCore
{
    /// <summary>
    /// 根据潜力获取最终数值
    /// </summary>
    /// <param name="baseValue"></param>
    /// <param name="potential"></param>
    /// <param name="lv"></param>
    /// <returns></returns>
    public static int GetValueByPotentiality(int baseValue, float potential, int lv)
    {
        return baseValue + (int)(baseValue * potential * System.Math.Max(lv - 1, 0));
    }
}