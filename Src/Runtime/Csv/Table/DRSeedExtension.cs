
using System.Collections.Generic;

/// <summary>
/// 种子表记录数据的扩展 有些实时遍历解析比较耗性能 需要缓存
/// </summary>
public static class DRSeedExtension
{
    private static Dictionary<int, int> s_requiredFertilizerMap;

    /// <summary>
    /// 获取表中对应的RequiredFertilizer属性值 已经完成了解析 没配置会返回0
    /// </summary>
    public static int GetRequiredFertilizer(this DRSeed drSeed)
    {
        s_requiredFertilizerMap ??= new Dictionary<int, int>();

        if (s_requiredFertilizerMap.TryGetValue(drSeed.Id, out int value))
        {
            return value;
        }
        else
        {
            int requiredFertilizer = 0;
            TableUtil.ForeachAttribute(drSeed.InitialAttribute, (type, baseValue, affectByPotential) =>
            {
                if (type == eAttributeType.requiredFertilizer)
                {
                    requiredFertilizer = baseValue;
                }
                return false;
            });

            s_requiredFertilizerMap.Add(drSeed.Id, requiredFertilizer);
            return requiredFertilizer;
        }
    }
}