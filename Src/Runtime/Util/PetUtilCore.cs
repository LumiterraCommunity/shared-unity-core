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
    /// 宠物能力位移数组转换为宠物能力枚举
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
    /// 宠物能力位移值转换为宠物能力枚举
    /// </summary>
    /// <param name="ability"></param>
    /// <returns></returns>
    public static ePetAbility PetAbilityBitToEnum(PetAbilityType ability)
    {
        return (ePetAbility)(1 << (int)ability);
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

    /// <summary>
    /// 通过位移获取宠物能力
    /// </summary>
    /// <param name="bitOffset"></param>
    /// <returns></returns>
    public static ePetAbility PetAbilityFromBitOffset(int bitOffset)
    {
        return (ePetAbility)(1 << bitOffset);
    }

    public static int GetPetLv(IEnumerable<AttributeData> attrs)
    {
        int lv = 0;
        foreach (AttributeData attr in attrs)
        {
            eAttributeType attrType = (eAttributeType)attr.Id;
            if (attrType is eAttributeType.CombatLv or eAttributeType.CollectionLv or eAttributeType.FarmingLv)
            {
                lv = Math.Max(lv, attr.Value);
            }
        }

        return lv;
    }

    public static int GetPetLv(EntityAttributeData attrData)
    {
        int battleLv = attrData.GetBaseValue(eAttributeType.CombatLv);
        int collectionLv = attrData.GetBaseValue(eAttributeType.CollectionLv);
        int farmingLv = attrData.GetBaseValue(eAttributeType.FarmingLv);

        return Math.Max(battleLv, Math.Max(collectionLv, farmingLv));
    }

    public static int GetPetProfileByType(IEnumerable<AttributeData> attrs, eAttributeType type)
    {
        foreach (AttributeData attr in attrs)
        {
            if (attr.Id == (int)type)
            {
                return attr.Value;
            }
        }

        return 0;
    }
}