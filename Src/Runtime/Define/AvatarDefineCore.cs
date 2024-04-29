using System.Collections.Generic;
using GameMessageCore;
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
}