using System.Collections.Generic;
using UnityGameFramework.Runtime;


/// <summary>
/// 宠物专属数据
/// </summary>
public class PetDataCore : EntityBaseComponent
{
    /// <summary>
    /// 宠物所有特性集合,通过位存储
    /// </summary>
    public ePetAbility AllAbility { get; protected set; } = ePetAbility.None;
    /// <summary>
    /// 宠物主人ID
    /// </summary>
    public long OwnerId { get; protected set; }
    /// <summary>
    /// 当前跟随的目标
    /// </summary>
    public EntityBase FollowingTarget { get; protected set; }
    /// <summary>
    /// 当前是否正在跟随
    /// </summary>
    public bool IsFollowing => FollowingTarget != null;
    /// <summary>
    /// 宠物配置
    /// </summary>
    public DRPet PetCfg { protected set; get; }
    /// <summary>
    /// 宠物配置
    /// </summary>
    public int PetCfgId => PetCfg == null ? -1 : PetCfg.Id;
    /// <summary>
    /// 当前手持物id
    /// 用于播种，喂食等操作，使用道具丢炸弹等操作
    /// </summary>
    public int InHandItem { get; protected set; } = 0;

    public void SetOwnerId(long ownerId)
    {
        OwnerId = ownerId;
    }

    public void SetPetCfgId(int cfgID)
    {
        PetCfg = GFEntryCore.DataTable.GetDataTable<DRPet>().GetDataRow(cfgID);

        if (PetCfg == null)
        {
            Log.Error($"Can not find pet cfg id:{cfgID}");
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
    /// 获取跟随技能ID列表,一定会返回一个数组
    /// </summary>
    /// <returns></returns>
    public int[] GetFollowingSkills()
    {
        if (!HasPetAbility(ePetAbility.SkillExtend) || !IsFollowing)
        {
            //没有扩展技能特性或者当前没在跟随
            return new int[] { };
        }

        if (PetCfg == null)
        {
            Log.Error("PetCfg is null");
            return new int[] { };
        }

        return PetCfg.ExtendSkill;
    }

    /// <summary>
    /// 设置跟随目标
    /// </summary>
    /// <param name="target"></param>
    public void SetFollowingTarget(EntityBase target)
    {
        if (FollowingTarget == target)
        {
            return;
        }

        FollowingTarget = target;
    }

    public void SetInHandItem(int cid)
    {
        if (cid == InHandItem)
        {
            return;
        }

        InHandItem = cid;
        RefEntity.EntityEvent.InHandItemChange?.Invoke(cid);
    }
}