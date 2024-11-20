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

    public const float EQUIPMENT_COLOR_ALPHA = 0.9f;
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
    public static readonly List<EquipmentEnhanceEffect> EquipmentEnhanceEffects = new()
    {
        new EquipmentEnhanceEffect
        {
            MinLevel = 0,
            MaxLevel = 1,
            Color = new Color(0.5f, 0.5f, 0.5f),
            Intensity = 100f,
        },
        new EquipmentEnhanceEffect
        {
            MinLevel = 2,
            MaxLevel = 10,
            Color = new Color(0.5f, 0.5f, 0.5f),
            Intensity = 100f,
        },
        new EquipmentEnhanceEffect
        {
            MinLevel = 11,
            MaxLevel = 30,
            Color = new Color(0.5f, 0.5f, 0.5f),
            Intensity = 100f,
        },
    };
}