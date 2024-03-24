/*
 * @Author: xiang huan
 * @Date: 2022-09-13 17:26:26
 * @Description: 实体阵营数据
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Entity/EntityCampDataCore.cs
 * 
 */

using System.Collections.Generic;
using UnityGameFramework.Runtime;


/// <summary>
/// 实体上的战斗数据 双端通用的核心数据
/// </summary>
public class EntityCampDataCore : EntityBaseComponent
{
    private EntityBase _refOwner;
    public EntityBase RefOwner  //没有主人时，主人为自己
    {
        get
        {
            //宠物获取主人
            if (_refOwner == null && RefEntity.BaseData.Type == GameMessageCore.EntityType.Pet)
            {
                if (RefEntity.TryGetComponent(out PetDataCore PetDataCore))
                {
                    if (!GFEntryCore.GetModule<IEntityMgr>().TryGetEntity(PetDataCore.OwnerId, out _refOwner))
                    {
                        Log.Error("EntityCampDataCore Owner is null");
                    };
                }

            }
            return _refOwner ?? RefEntity;
        }
    }

    private eEntityCampType _selfCampType;
    /// <summary>
    /// 阵营类型
    /// </summary>
    /// <value></value>
    public eEntityCampType CampType => RefOwner.EntityCampDataCore.GetCampType();

    public long DelayChangeTimestamp { get; protected set; } = -1;
    public eEntityCampType DelayChangeCampType { get; protected set; }
    public void Init(eEntityCampType campType)
    {
        _selfCampType = campType;
    }
    protected eEntityCampType GetCampType()
    {
        return _selfCampType;
    }
    public virtual bool CheckIsEnemy(EntityBase other)
    {
        //归属相同
        if (RefOwner.BaseData.Id == other.EntityCampDataCore.RefOwner.BaseData.Id)
        {
            return false;
        }
        return IsEnemy(other.EntityCampDataCore.RefOwner);
    }

    public virtual bool CheckIsFriend(EntityBase other)
    {
        //归属相同
        if (RefOwner.BaseData.Id == other.EntityCampDataCore.RefOwner.BaseData.Id)
        {
            return true;
        }
        return IsFriend(other.EntityCampDataCore.RefOwner);
    }
    /// <summary>
    /// 是否是敌人
    /// </summary>
    protected virtual bool IsEnemy(EntityBase otherOwner)
    {
        if (BattleDefine.EntityCampEnemy.TryGetValue(RefOwner.EntityCampDataCore.CampType, out HashSet<eEntityCampType> enemyList))
        {
            return enemyList.Contains(otherOwner.EntityCampDataCore.CampType);
        }
        return false;
    }

    /// <summary>
    /// 是否是友军
    /// </summary>
    /// <param name="otherOwner"></param>
    /// <returns></returns>
    protected virtual bool IsFriend(EntityBase otherOwner)
    {
        if (BattleDefine.EntityCampFriend.TryGetValue(RefOwner.EntityCampDataCore.CampType, out HashSet<eEntityCampType> friendList))
        {
            return friendList.Contains(otherOwner.EntityCampDataCore.CampType);
        }
        return false;
    }

    /// <summary>
    /// 是否为技能目标
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsSkillTarget(EntityBase other, int skillTargetType)
    {
        if (other == null || !other.Inited || other.BaseData.Id == RefEntity.BaseData.Id)
        {
            return false;
        }

        // 目标为友方
        if ((skillTargetType & (int)eSkillTargetType.Friend) != 0 && CheckIsFriend(other))
        {
            return true;
        }

        // 目标为敌人
        if ((skillTargetType & (int)eSkillTargetType.Enemy) != 0 && CheckIsEnemy(other))
        {
            return true;
        }

        return false;
    }

    public virtual bool ChangeCamp(eEntityCampType campType)
    {
        _selfCampType = campType;
        RefEntity.EntityEvent.ChangeCamp?.Invoke();
        return true;
    }

    protected virtual void Update()
    {
        if (IsDelayChangeCamp())
        {
            long curTimeStamp = TimeUtil.GetServerTimeStamp();
            if (curTimeStamp >= DelayChangeTimestamp)
            {
                StopDelayChangeCamp();
                _ = ChangeCamp(DelayChangeCampType);
            }
        }
    }
    /// <summary>
    /// 开始延迟改变阵营
    /// </summary>
    /// <param name="campType"></param>
    /// <param name="delayTime"></param>
    public virtual bool StartDelayChangeCamp(eEntityCampType campType, long delayTime = 0)
    {
        if (IsDelayChangeCamp())
        {
            return false;
        }
        DelayChangeCampType = campType;
        DelayChangeTimestamp = delayTime;
        RefEntity.EntityEvent.DelayChangeCampUpdate?.Invoke();
        return true;
    }
    /// <summary>
    /// 停止延迟改变阵营
    /// </summary>
    /// <param name="campType"></param>
    /// <param name="delayTime"></param>
    public virtual void StopDelayChangeCamp()
    {
        DelayChangeTimestamp = -1;
        RefEntity.EntityEvent.DelayChangeCampUpdate?.Invoke();
    }

    /// <summary>
    /// 是否有延迟改变阵营
    /// </summary>
    public bool IsDelayChangeCamp()
    {
        return DelayChangeTimestamp > 0;
    }
}