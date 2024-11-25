using System.Collections.Generic;
using System.Linq;
using GameMessageCore;
using UnityEngine;
/// <summary>
/// Avatar related define
/// </summary>
public static class AvatarDefineCore
{
    /// <summary>
    /// 外观部位合集
    /// </summary>
    /// <value></value>
    public static readonly List<AvatarPosition> AppearancePartList = new()
    {
        AvatarPosition.AppearanceHead,
        AvatarPosition.AppearanceCoat,
        AvatarPosition.AppearancePant,
        AvatarPosition.AppearanceShoe,
        AvatarPosition.AppearanceHand,
        AvatarPosition.AppearanceWeapon,
    };

    /// <summary>
    /// 装备部位合集
    /// </summary>
    /// <value></value>
    public static readonly List<AvatarPosition> EquipmentPartList = new()
    {
        AvatarPosition.Head,
        AvatarPosition.Coat,
        AvatarPosition.Pant,
        AvatarPosition.Shoe,
        AvatarPosition.Hand,
        AvatarPosition.Weapon,
    };

    /// <summary>
    /// 所有部位合集，包括外观和装备，外观部位必须在装备部位之后，外观显示优先级高于装备
    /// </summary>
    /// <returns></returns>
    public static readonly List<AvatarPosition> AllPartList = EquipmentPartList.Concat(AppearancePartList).ToList();

    public const float EQUIPMENT_COLOR_ALPHA = 0.9019f; //颜色值230
    public class EquipmentEnhanceEffect
    {
        public int MinLevel;
        public int MaxLevel;
        public Color Color;    //颜色
        public float Intensity; //亮度
    }

    /// <summary>
    /// 装备强化效果配置
    /// </summary>
    public static Dictionary<TalentType, Color> EquipmentEnhanceEffects = new()
    {
        {TalentType.Battle, new Color( 0.776f, 0.208f, 0.051f) },
        {TalentType.Farming, new Color(0.314f, 0.6f, 0.8f)},
        {TalentType.Gather, new Color(0.196f, 0.835f, 0.196f)},
    };
    public const float EQUIPMENT_ENHANCE_MAX_INTENSITY = 30; //装备最大强化亮度
}