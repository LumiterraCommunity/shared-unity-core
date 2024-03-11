using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;


/// <summary>
/// 玩家的宠物数据管理组件 挂在玩家实体上
/// </summary>
public class PlayerPetDataCore : EntityBaseComponent
{
    /// <summary>
    /// 宠物跟随事件
    /// T0:当前正在跟随的实体
    /// </summary>
    public event Action<EntityBase> PetFollow;
    /// <summary>
    /// 宠物取消跟随事件
    /// T0:取消跟随前的实体
    /// </summary>
    public event Action<EntityBase> PetUnFollow;
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
    public virtual void SetFollowingPet(EntityBase pet)
    {
        if (pet == FollowingPet)
        {
            return;
        }

        if (pet == null)
        {
            EntityBase lastPet = FollowingPet;
            FollowingPet = null;
            PetUnFollow?.Invoke(lastPet);
            Log.Info("Player un follow pet");
        }
        else
        {
            FollowingPet = pet;
            PetFollow?.Invoke(pet);
            Log.Info($"Player following pet changed, cur pet is {pet.BaseData.Id}");
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