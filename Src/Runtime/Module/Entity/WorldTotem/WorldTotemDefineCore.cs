/// <summary>
/// 世界图腾定义
/// </summary>
public static class WorldTotemDefineCore
{
    /// <summary>
    /// 间隔范围半径 米 每次会读表
    /// </summary>
    public static float IntervalRange
    {
        get
        {
            float range = 5;//米
            if (TableUtil.TryGetGameValue(eGameValueID.WorldTotemIntervalDistance, out DRGameValue drGameValue))
            {
                range = MathUtilCore.CM2M * drGameValue.Value;
            }
            return range;
        }
    }

    /// <summary>
    /// 密度范围半径 米 每次会读表
    /// </summary>
    public static float DensityRange
    {
        get
        {
            float range = 10;//米
            if (TableUtil.TryGetGameValue(eGameValueID.WorldTotemDensityRange, out DRGameValue drGameValue))
            {
                range = MathUtilCore.CM2M * drGameValue.Value;
            }
            return range;
        }
    }
}