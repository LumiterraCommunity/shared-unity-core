using System;
using System.Collections.Generic;
using GameMessageCore;
using Google.Protobuf.Collections;


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
    public static ePetAbility PetAbilityBitArrayToEnum(IEnumerable<PetAbilityType> abilityBitOffsets)
    {
        ePetAbility result = ePetAbility.None;
        foreach (PetAbilityType ability in abilityBitOffsets)
        {
            result |= (ePetAbility)(1 << (int)ability);
        }

        return result;
    }

    /// <summary>
    /// 宠物能力枚举值转换为位移协议Repeated结构 没有直接通用转List是不想多一层遍历到repeated结构
    /// </summary>
    /// <param name="enumValue"></param>
    /// <param name="protoRepeated"></param>
    public static void PetAbilityEnumToBitProtoRepeated(ePetAbility enumValue, RepeatedField<PetAbilityType> protoRepeated)
    {
        if (protoRepeated == null)
        {
            return;
        }

        // 将枚举值转换为整数
        int intValue = (int)enumValue;

        // 遍历整数的每个位
        for (int i = 0; i < sizeof(int) * 8; i++)
        {
            // 如果当前位为1，则记录偏移量
            if ((intValue & (1 << i)) != 0)
            {
                protoRepeated.Add((PetAbilityType)i);
            }
        }
    }
}