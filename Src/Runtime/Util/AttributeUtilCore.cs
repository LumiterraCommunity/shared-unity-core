using System.Collections.Generic;
using GameMessageCore;


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
    /// 计算精力消耗
    /// </summary>
    /// <param name="killedEntityType">击杀的实体类型</param>
    /// <param name="killNum">当日该实体种类已击杀数量</param>
    /// <param name="dRSceneArea"></param>
    /// <param name="worldDropRate">世界掉落率 副本也要传</param>
    /// <returns></returns>
    public static float CalculateEnergyCost(EntityType killedEntityType, int killNum, DRSceneArea dRSceneArea, float worldDropRate)
    {
        return -1;
    }
}