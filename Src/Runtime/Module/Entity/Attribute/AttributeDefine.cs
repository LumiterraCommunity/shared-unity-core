/*
 * @Author: xiang huan
 * @Date: 2023-01-11 14:44:39
 * @Description: 属性定义
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Attribute/AttributeDefine.cs
 * 
 */
using System.Collections.Generic;
using GameMessageCore;

public static class AttributeDefine
{
    /// <summary>
    /// 潜力值属性集合
    /// </summary>
    public static readonly HashSet<eAttributeType> PotentialAttrSet = new()
    {
        eAttributeType.CombatPotentiality,
        eAttributeType.GatherPotentiality,
        eAttributeType.FarmPotentiality,
    };

    /// <summary>
    /// 天赋类型对应的潜力属性类型
    /// </summary>
    public static readonly Dictionary<TalentType, eAttributeType> TalentType2PotentialAttr = new()
    {
        {TalentType.Battle,eAttributeType.CombatPotentiality},
        {TalentType.Gather,eAttributeType.GatherPotentiality},
        {TalentType.Farming,eAttributeType.FarmPotentiality},
    };

    /// <summary>
    /// 天赋类型对应的属性等级属性类型
    /// </summary>
    public static readonly Dictionary<TalentType, eAttributeType> TalentType2TalentLvAttr = new()
    {
        {TalentType.Battle,eAttributeType.CombatLv},
        {TalentType.Gather,eAttributeType.CollectionLv},
        {TalentType.Farming,eAttributeType.FarmingLv},
    };
}
/// <summary>
/// 属性修改器类型
/// </summary>
public enum eModifierType : int
{
    Add = 1,  //增加
    PctAdd,   //百分比增加
    FinalAdd,  //最终增加
    FinalPctAdd,  //最终百分比增加
}

/// <summary>
/// 属性类型
/// </summary>
public enum eAttributeValueType : int
{
    Int = 1,  //整型
    ThousandthPct = 2,   //千分位百分比百分比
    Thousandth = 3,  //千分位浮点
}