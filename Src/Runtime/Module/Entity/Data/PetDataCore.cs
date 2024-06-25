using System.Collections.Generic;
using GameMessageCore;
using UnityEngine;
using UnityGameFramework.Runtime;


/// <summary>
/// 宠物核心数据
/// 家园宠物和跟随宠物都使用这个组件
/// </summary>
public class PetDataCore : EntityBaseComponent
{
    /// <summary>
    /// 宠物所有特性集合,通过位存储
    /// </summary>
    public ePetAbility AllAbility { get; protected set; } = ePetAbility.None;
    /// <summary>
    /// 宠物特性对应的技能ID
    /// </summary>
    public Dictionary<ePetAbility, int> AbilityToSkillIdDic { get; protected set; } = new();
    /// <summary>
    /// 宠物主人ID
    /// </summary>
    public long OwnerId { get; protected set; }
    /// <summary>
    /// 是否正在跟随
    /// </summary>
    public bool IsFollowing { get; protected set; }
    /// <summary>
    /// 宠物配置
    /// </summary>
    public DRPet PetCfg { protected set; get; }

    /// <summary>
    /// 怪物和宠物同ID配置, 宠物战斗时使用
    /// </summary>
    public DRMonster MonsterCfg { get; protected set; }

    /// <summary>
    /// 宠物配置
    /// </summary>
    public int PetCfgId => PetCfg == null ? -1 : PetCfg.Id;

    /// <summary>
    /// 好感度数值
    /// </summary>
    public int Favorability;
    /// <summary>
    /// 动物创建时间戳
    /// </summary>
    public long CreateMs;
    /// <summary>
    /// 动物最近更新时间戳
    /// </summary>
    public long UpdateMs;
    /// <summary>
    /// 饥饿度值 需要小数结构 tick会每帧减少
    /// </summary>
    public float HungerValue { get; private set; }

    /// <summary>
    /// 是完全饥饿状态
    /// </summary>
    public bool IsHunger => HungerValue <= 0;

    public void InitFromNetData(GrpcPetData petData)
    {
        Favorability = petData.Favorability;
        CreateMs = petData.CreateMs;
        UpdateMs = petData.UpdateMs;
        IsFollowing = petData.Status;
        HungerValue = petData.Hunger;
        SetAbilityByBitOffsets(petData.AbilityList);
        SetPetCfgId(petData.Cid);

        OnInited();
    }

    public void InitFromProxyPetData(ProxyAnimalBaseData proxyData)
    {
        Favorability = proxyData.FavorAbility;
        CreateMs = proxyData.CreateMs;
        UpdateMs = proxyData.UpdateMs;
        IsFollowing = proxyData.Status;
        HungerValue = proxyData.Hunger;
        SetAbilityByBitOffsets(proxyData.AbilityList);
        SetPetCfgId(proxyData.Cid);

        OnInited();
    }

    /// <summary>
    /// 通过宠物特性位移数组设置宠物特性
    /// </summary>
    /// <param name="abilityOffsets"></param>
    private void SetAbilityByBitOffsets(IEnumerable<PetAbilityType> abilityOffsets)
    {
        //没有能力的宠物这个字段可能为空，这个判断一定得加上
        if (abilityOffsets == null)
        {
            AllAbility = ePetAbility.None;
            return;
        }

        AllAbility = PetUtilCore.PetAbilityBitArrayToEnum(abilityOffsets);
    }

    /// <summary>
    /// 初始化完成
    /// </summary>
    protected virtual void OnInited()
    {
    }

    /// <summary>
    /// 将上层准备的proxy数据 到这里加工 填充上这里管的数据
    /// </summary>
    /// <param name="proxyData"></param>
    public void ToProxyPetData(ProxyAnimalBaseData proxyData)
    {
        proxyData.FavorAbility = Favorability;
        proxyData.CreateMs = CreateMs;
        proxyData.UpdateMs = UpdateMs;
        proxyData.Cid = PetCfgId;
        proxyData.Status = IsFollowing;
        proxyData.Hunger = (int)HungerValue;
        PetUtilCore.PetAbilityEnumToBitProtoRepeated(AllAbility, proxyData.AbilityList);
    }

    /// <summary>
    /// 初始化宠物特性对应的技能ID
    /// !!注意，该函数一定要在设置完宠物特性和宠物配置后调用
    /// </summary>
    private void InitAbilityToSkillDic()
    {
        if (PetCfg == null)
        {
            Log.Error("PetCfg is null");
            return;
        }

        if (AllAbility == ePetAbility.None)
        {
            return;//如果没有特性，就不用初始化技能了
        }

        int[][] abilityInfos = PetCfg.Ability;
        for (int i = 0; i < abilityInfos.Length; i++)
        {
            int[] abilityInfo = abilityInfos[i];
            if (abilityInfo.Length != 3)
            {
                Log.Error($"Pet ability config error, id:{PetCfg.Id}");
                continue;
            }
            int skillId = abilityInfo[2];
            if (skillId <= 0)
            {
                continue;
            }
            ePetAbility ability = PetUtilCore.PetAbilityFromBitOffset(abilityInfo[0]);
            if (!HasPetAbility(ability))
            {
                continue;//虽然配置了技能，但是宠物的特性是随机生成的，所以可能没有这个特性
            }

            AbilityToSkillIdDic[ability] = skillId;
        }
    }

    /// <summary>
    /// 获取宠物特性对应的技能ID 如果没有返回-1
    /// </summary>
    /// <param name="ability"></param>
    /// <returns></returns>
    public int GetAbilitySkillId(ePetAbility ability)
    {
        if (AbilityToSkillIdDic.TryGetValue(ability, out int skillId))
        {
            return skillId;
        }
        return -1;
    }

    public void SetPetCfgId(int cfgID)
    {
        PetCfg = GFEntryCore.DataTable.GetDataTable<DRPet>().GetDataRow(cfgID);

        if (PetCfg == null)
        {
            Log.Error($"Can not find pet cfg id:{cfgID}");
            return;
        }

        InitAbilityToSkillDic();
        MonsterCfg = GFEntryCore.DataTable.GetDataTable<DRMonster>().GetDataRow(cfgID);
        if (MonsterCfg == null)
        {
            Log.Error($"Can not find monster cfg id:{cfgID}");
        }
    }

    /// <summary>
    /// 设置宠物所有特性
    /// </summary>
    /// <param name="abilities"></param>
    public void SetPetAbility(ePetAbility abilities)
    {
        AllAbility = abilities;
    }

    /// <summary>
    /// 设置宠物主人ID
    /// </summary>
    /// <param name="ownerId"></param>
    public void SetOwnerId(long ownerId)
    {
        OwnerId = ownerId;
    }

    public void SetIsFollowing(bool isFollowing)
    {
        IsFollowing = isFollowing;
    }

    /// <summary>
    /// 添加宠物特性
    /// </summary>
    /// <param name="ability"></param>
    public void AddPetAbility(ePetAbility ability)
    {
        AllAbility |= ability;
    }

    /// <summary>
    /// 移除宠物特性
    /// </summary>
    /// <param name="ability"></param>
    public void RemovePetAbility(ePetAbility ability)
    {
        AllAbility &= ~ability;
    }

    /// <summary>
    /// 是否有某个宠物特性
    /// </summary>
    /// <param name="ability"></param>
    /// <returns></returns>
    public bool HasPetAbility(ePetAbility ability)
    {
        return (AllAbility & ability) != 0;
    }

    /// <summary>
    /// 设置饥饿度 内部会处理成最小为0
    /// </summary>
    /// <param name="value"></param>
    /// <param name="force">业务侧过来的一般都是false  那种初始化的强行改底层数据的给true</param>
    public void SetHungerValue(float value, bool force)
    {
        HungerValue = Mathf.Max(0, value);

        RefEntity.EntityEvent.OnPetHungerChanged?.Invoke(HungerValue, force);
    }
}