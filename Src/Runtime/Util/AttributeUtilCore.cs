using System.Collections.Generic;
using GameMessageCore;
using UnityGameFramework.Runtime;


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
    public static int GetValueByPotentiality(int baseValue, float potential, float lv)
    {
        return baseValue + (int)(baseValue * potential * System.Math.Max(lv - 1, 0));
    }

    /// <summary>
    /// 获取某个属性的装备增益值
    /// </summary>
    /// <returns></returns>
    public static int GetEquipmentGainAttr(IEnumerable<AvatarAttribute> avatars, eAttributeType attrType)
    {
        int value = 0;
        foreach (AvatarAttribute item in avatars)
        {
            //遍历部件的所有属性值
            foreach (AttributeData attr in item.Data)
            {
                if ((int)attrType == attr.Id)
                {
                    value += attr.Value;
                }
            }
        }

        return value;
    }

    /// <summary>
    /// 获取实体的完整等级 由整数部分（需要参数指定类型）和小数部分两个属性拼接而成
    /// </summary>
    /// <param name="attributeDataCpt"></param>
    /// <param name="lvType">整数部分的lv类型 每个专精都有独自的</param>
    /// <returns></returns>
    public static float GetEntityCompleteLv(AttributeDataCpt attributeDataCpt, eAttributeType lvType)
    {
        if (attributeDataCpt == null)
        {
            return EntityDefineCore.PROTECT_LEVEL;
        }

        return attributeDataCpt.GetRealValue(lvType) + attributeDataCpt.GetRealValue(eAttributeType.ExtThousLv);
    }

    /// <summary>
    /// 计算精力消耗 异常时返回-1
    /// </summary>
    /// <param name="killedEntityType">击杀的实体类型</param>
    /// <param name="killNum">当日该实体种类已击杀数量</param>
    /// <param name="dRSceneArea"></param>
    /// <param name="worldDropRate">世界掉落率 副本也要传</param>
    /// <returns></returns>
    public static float CalculateEnergyCost(EntityType killedEntityType, int killNum, DRSceneArea dRSceneArea, float worldDropRate)
    {
        if (dRSceneArea == null || killedEntityType == EntityType.All || worldDropRate < 0)
        {
            Log.Error($"CalculateEnergyCost Error: killedEntityType = {killedEntityType} sceneId = {dRSceneArea?.Id} worldDropRate = {worldDropRate}");
            return -1;
        }

        //非怪物和资源其他按照原来世界掉落率计算
        if (killedEntityType is not EntityType.Monster and not EntityType.Resource)
        {
            return worldDropRate;
        }

        int numThreshold;
        try
        {
            if (killedEntityType == EntityType.Monster)
            {
                numThreshold = dRSceneArea.EnergyIEBA[0];
            }
            else if (killedEntityType == EntityType.Resource)
            {
                numThreshold = dRSceneArea.EnergyIEBA[1];
            }
            else
            {
                Log.Error($"CalculateEnergyCost Error: killedEntityType = {killedEntityType}");
                numThreshold = -1;
            }
        }
        catch (System.Exception e)
        {
            Log.Error($"CalculateEnergyCost Error,killedEntityType:{killedEntityType} killNum:{killNum} sceneId:{dRSceneArea.Id} worldDropRate:{worldDropRate}:e:\n {e}");
            return -1;
        }

        //阈值配置的数量小于等于0时直接使用世界掉落率
        if (numThreshold <= 0)
        {
            return worldDropRate;
        }

        //没到阈值时直接使用世界掉落率
        if (killNum <= numThreshold)
        {
            return worldDropRate;
        }

        float rate = (float)(killNum - numThreshold) / (numThreshold * 4);
        rate = (float)System.Math.Round(rate, 1, System.MidpointRounding.AwayFromZero);//  - （击杀数-阈值）/ (阈值 * 4 )  的计算结果通过四舍五入保留1位小数
        //  - 扣除值 = 0.185 * (1+（击杀数-阈值）/ (阈值 * 4 ))
        return worldDropRate * (1 + rate);
    }
}