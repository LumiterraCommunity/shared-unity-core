/*
 * @Author: xiang huan
 * @Date: 2022-09-13 17:26:26
 * @Description: 实体阵营数据
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Entity/EntityCampDataCore.cs
 * 
 */

using System.Collections.Generic;


/// <summary>
/// 实体上的战斗数据 双端通用的核心数据
/// </summary>
public class EntityCampDataCore : EntityBaseComponent
{
    /// <summary>
    /// 阵营类型
    /// </summary>
    /// <value></value>
    public eEntityCampType CampType;

    public void Init(eEntityCampType campType)
    {
        CampType = campType;
    }

    /// <summary>
    /// 是否是敌人
    /// </summary>
    public virtual bool IsEnemy(EntityBase other)
    {
        if (BattleDefine.EntityCampEnemy.TryGetValue(CampType, out HashSet<eEntityCampType> enemyList))
        {
            return enemyList.Contains(other.EntityCampDataCore.CampType);
        }
        return false;
    }

    /// <summary>
    /// 是否是友军
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public virtual bool IsFriend(EntityBase other)
    {
        if (BattleDefine.EntityCampFriend.TryGetValue(CampType, out HashSet<eEntityCampType> friendList))
        {
            return friendList.Contains(other.EntityCampDataCore.CampType);
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
        if ((skillTargetType & (int)eSkillTargetType.Friend) != 0 && IsFriend(other))
        {
            return true;
        }

        // 目标为敌人
        if ((skillTargetType & (int)eSkillTargetType.Enemy) != 0 && IsEnemy(other))
        {
            return true;
        }

        return false;
    }

    public virtual bool ChangeCamp(eEntityCampType campType)
    {
        CampType = campType;
        RefEntity.EntityEvent.ChangeCamp?.Invoke();
        return true;
    }
}