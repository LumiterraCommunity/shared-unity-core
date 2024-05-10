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
    public static int GetValueByPotentiality(int baseValue, float potential, int lv)
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
}