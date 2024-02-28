using System.Collections.Generic;
using GameMessageCore;


/// <summary>
/// 宠物相关工具
/// </summary>
public static class PetUtilCore
{
    /// <summary>
    /// 宠物位移数组转换为宠物能力
    /// </summary>
    /// <param name="abilityBitOffsets"></param>
    /// <returns></returns>
    public static ePetAbility AbilityBitOffsets2ePetAbility(IEnumerable<PetAbilityType> abilityBitOffsets)
    {
        ePetAbility result = ePetAbility.None;
        foreach (PetAbilityType ability in abilityBitOffsets)
        {
            result |= (ePetAbility)(1 << (int)ability);
        }

        return result;
    }
}