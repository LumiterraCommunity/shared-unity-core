using System;
using System.Collections.Generic;
using GameMessageCore;
using Google.Protobuf.Collections;
using UnityGameFramework.Runtime;
using static HomeDefine;

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
        if (abilityBitOffsets == null)
        {
            Log.Error("PetAbilityBitArrayToEnum abilityBitOffsets is null");
            return result;
        }

        foreach (PetAbilityType ability in abilityBitOffsets)
        {
            result |= PetAbilityBitToEnum(ability);
        }

        return result;
    }

    /// <summary>
    /// 宠物能力位移值枚举转换为宠物能力枚举
    /// </summary>
    /// <param name="ability"></param>
    /// <returns></returns>
    public static ePetAbility PetAbilityBitToEnum(PetAbilityType ability)
    {
        return PetAbilityFromBitOffset((int)ability);
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

        return UnityEngine.Mathf.Max(battleLv, collectionLv, farmingLv);
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

    /// <summary>
    /// 判断宠物是否可以采集
    /// </summary>
    /// <returns></returns>
    public static bool JudgePetCanCollect(EntityBase pet)
    {
        //没召唤出了宠物
        if (pet == null)
        {
            return false;
        }

        //不是宠物
        if (!pet.TryGetComponent(out PetDataCore petData))
        {
            Log.Error($"JudgePetCanCollect: petData is null");
            return false;
        }

        // 没有采集能力
        if ((petData.AllAbility & ePetAbility.Gather) == 0)
        {
            return false;
        }

        if (!pet.TryGetComponent(out EntityAvatarDataCore petAvatarDataCore))
        {
            Log.Error($"JudgePetCanCollect: petAvatarDataCore is null");
            return false;
        }

        //没有装备武器
        int itemId = petAvatarDataCore.GetWeaponAvatar();
        if (itemId <= 0)
        {
            return false;
        }

        DREquipment equipment = EquipmentTable.Inst.GetRowByItemID(itemId);
        if (equipment == null)
        {
            Log.Error($"JudgePetCanCollect: equipment cfg error itemId:{itemId}");
            return false;
        }

        //宠物穿的武器类型不对
        if (!PetWearWeaponIsCollect(equipment))
        {
            return false;
        }

        //宠物穿的武器没有采集动作
        if (!PetWearWeaponCanCollectAction(equipment))
        {
            Log.Error($"JudgePetCanCollect: pet wear weapon can not collect action,equipment id: {equipment.Id}");
            return false;
        }

        return true;
    }

    /// <summary>
    /// 宠物穿的武器有采集动作
    /// </summary>
    /// <param name="equipment"></param>
    /// <returns></returns>
    public static bool PetWearWeaponCanCollectAction(DREquipment equipment)
    {
        if (equipment.GivenSkillId.Length <= 0)
        {
            return false;
        }

        int skillId = equipment.GivenSkillId[0];
        if (skillId <= 0)
        {
            return false;
        }

        DRSkill drSkill = TableUtil.GetConfig<DRSkill>(skillId);
        if (drSkill == null)
        {
            return false;
        }

        eAction actions = drSkill.GetHomeAction();
        return (actions & (eAction.Mowing | eAction.Cut | eAction.Mining)) != 0;
    }

    /// <summary>
    /// 宠物穿的武器是采集类型
    /// </summary>
    /// <param name="equipment"></param>
    /// <returns></returns>
    public static bool PetWearWeaponIsCollect(DREquipment equipment)
    {
        return (WeaponSubType)equipment.WeaponSubtype is WeaponSubType.Sickle or WeaponSubType.Axe or WeaponSubType.Pickaxe;
    }
}