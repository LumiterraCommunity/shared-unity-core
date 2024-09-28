
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 种子表记录数据的扩展 有些实时遍历解析比较耗性能 需要缓存
/// </summary>
public static class DRSeedExtension
{
    //缓存RequiredFertilizer最终属性值 减少遍历解析 key:seedCid
    private static Dictionary<int, int> s_requiredFertilizerMap;

    /// <summary>
    /// 获取表中对应的RequiredFertilizer属性值 已经完成了解析 没配置会返回0
    /// <param name="extraLv">额外等级 一般是关卡等级</param>
    /// </summary>
    public static int GetRequiredFertilizer(this DRSeed drSeed, int extraLv)
    {
        s_requiredFertilizerMap ??= new Dictionary<int, int>();

        if (s_requiredFertilizerMap.TryGetValue(drSeed.Id, out int value))
        {
            return value;
        }
        else
        {
            //解析基础属性配置
            int fertilizerBaseValue = 0;
            bool fertilizerAffectByPotential = false;
            TableUtil.ForeachAttribute(drSeed.InitialAttribute, (type, baseValue, affectByPotential) =>
            {
                if (type == eAttributeType.RequiredFertilizer)
                {
                    fertilizerBaseValue = baseValue;
                    fertilizerAffectByPotential = affectByPotential;
                    return true;
                }
                return false;
            });

            if (fertilizerBaseValue <= 0)
            {
                Log.Error($"GetRequiredFertilizer id={drSeed.Id} RequiredFertilizer<=0");
                return int.MaxValue;
            }

            if (drSeed.FarmPotentiality.Length != 2)
            {
                Log.Error($"GetRequiredFertilizer id={drSeed.Id} FarmPotentiality.Length!=2");
                return int.MaxValue;
            }


            if (drSeed.FarmPotentiality[0] != drSeed.FarmPotentiality[1])
            {
                Log.Error($"GetRequiredFertilizer id={drSeed.Id} FarmPotentiality[0] != FarmPotentiality[1]");
            }

            //计算潜力值
            int potentialityBaseValue = drSeed.FarmPotentiality[0];
            float potentialityRealValue = potentialityBaseValue * TableUtil.GetAttributeCoefficient(eAttributeType.FarmPotentiality);
            int lv = drSeed.Lv + extraLv;

            //根据等级、潜力值、基础属性计算出对应等级的最终属性base值
            if (fertilizerAffectByPotential)//判断是否受潜力值影响
            {
                fertilizerBaseValue = AttributeUtilCore.GetValueByPotentiality(fertilizerBaseValue, potentialityRealValue, lv);
            }

            //转最终实际值单位
            int fertilizerRealValue = Mathf.FloorToInt(fertilizerBaseValue * TableUtil.GetAttributeCoefficient(eAttributeType.RequiredFertilizer));

            s_requiredFertilizerMap.Add(drSeed.Id, fertilizerRealValue);
            return fertilizerRealValue;
        }
    }
}