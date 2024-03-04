using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;


/// <summary>
/// 玩家的宠物数据管理组件 挂在玩家实体上
/// </summary>
public class PlayerPetDataCore : EntityBaseComponent
{
    /// <summary>
    /// 玩家当前跟随的宠物发生变化，null代表没有跟随宠物
    /// </summary>
    public event Action<EntityBase> FollowingPetChanged;
    /// <summary>
    /// 玩家宠物集合，没有任何宠物的时候返回null
    /// 这里只存实体的id，有的宠物可能在家园，不在场景中，所以不存实体引用
    /// 有遍历需求的时候可以额外增加一个List
    /// </summary>
    public HashSet<long> PetSet { get; private set; }
    /// <summary>
    /// 当前跟随的宠物，null表示没有跟随宠物
    /// </summary>
    public EntityBase FollowingPet { get; private set; }
    /// <summary>
    /// 设置当前跟随的宠物，null表示取消跟随
    /// </summary>
    /// <param name="pet"></param>
    public void SetFollowingPet(EntityBase pet)
    {
        if (pet == FollowingPet || !pet.Inited)
        {
            return;
        }

        string petInfo = pet == null ? "null" : pet.BaseData.Id.ToString();
        Log.Info($"Player following pet changed, cur pet is {petInfo}");
        SetPetFlowingTarget(FollowingPet, null);//取消之前的跟随标记
        SetPetFlowingTarget(pet, RefEntity);//设置新的跟随
        FollowingPet = pet;
        FollowingPetChanged?.Invoke(pet);
    }

    /// <summary>
    /// 设置宠物跟随目标
    /// </summary>
    /// <param name="pet"></param>
    /// <param name="target"></param>
    private void SetPetFlowingTarget(EntityBase pet, EntityBase target)
    {
        if (pet == null || !pet.Inited)
        {
            return;
        }

        if (pet.TryGetComponent(out PetFollowDataCore followData))
        {
            followData.SetFollowingTarget(target);
        }
    }

    public void AddPet(long id)
    {
        if (PetSet == null)
        {
            PetSet = new();
        }

        if (!PetSet.Add(id))
        {
            Log.Error("AddPet: pet already exist, id = {0}", id);
        }
    }

    public void RemovePet(long id)
    {
        if (PetSet == null)
        {
            return;
        }

        if (!PetSet.Remove(id))
        {
            Log.Error("RemovePet: pet not exist, id = {0}", id);
        }
    }
}