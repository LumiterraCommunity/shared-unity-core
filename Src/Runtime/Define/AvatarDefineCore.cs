using System.Collections.Generic;
using System.Linq;
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

    /// <summary>
    /// 所有部位合集，包括外观和装备，外观部位必须在装备部位之后，外观显示优先级高于装备
    /// </summary>
    /// <returns></returns>
    public static readonly List<AvatarPosition> AllPartList = EquipmentPartList.Concat(AppearancePartList).ToList();
}