/* 
 * @Author XQ
 * @Date 2022-08-05 12:54:15
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Behavior/EntityBehaviorCheckLogic.cs
 */

using GameFramework.Fsm;

/// <summary>
/// 实体行为检查的查询逻辑  用来判定是否能够移动 能够攻击的判定逻辑
/// </summary>
public static class EntityBehaviorCheckLogic
{
    /// <summary>
    /// 获取实体的当前状态 找不到为null
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static FsmState<EntityStatusCtrl> GetCurrentStatus(EntityBase entity)
    {
        if (entity == null)
        {
            return null;
        }

        if (!entity.TryGetComponent(out EntityStatusCtrl entityStatusCtrl))
        {
            return null;
        }

        return entityStatusCtrl.Fsm.CurrentState;
    }

    /// <summary>
    /// 实体是否在idle闲置状态
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static bool IsIdleStatus(EntityBase entity)
    {
        FsmState<EntityStatusCtrl> curStatus = GetCurrentStatus(entity);
        if (curStatus == null)
        {
            return false;
        }

        return curStatus is IdleStatusCore;
    }

    /// <summary>
    /// 实体是否在移动状态
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static bool IsMovingStatus(EntityBase entity)
    {
        FsmState<EntityStatusCtrl> curStatus = GetCurrentStatus(entity);
        if (curStatus == null)
        {
            return false;
        }

        return curStatus is DirectionMoveStatusCore or PathMoveStatusCore;
    }

    /// <summary>
    /// 实体是否在战斗状态
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static bool IsBattleStatus(EntityBase entity)
    {
        FsmState<EntityStatusCtrl> curStatus = GetCurrentStatus(entity);
        if (curStatus == null)
        {
            return false;
        }

        return curStatus is SkillAccumulateStatusCore or SkillForwardStatusCore or SkillCastStatusCore;
    }

    /// <summary>
    /// 实体是否在死亡状态
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static bool IsDeathStatus(EntityBase entity)
    {
        FsmState<EntityStatusCtrl> curStatus = GetCurrentStatus(entity);
        if (curStatus == null)
        {
            return false;
        }

        return curStatus.StatusName == DeathStatusCore.Name;
    }

    /// <summary>
    /// 检查实体是否能够移动
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static bool CheckEntityCanMove(EntityBase entity)
    {
        if (!entity.MoveData.IsGrounded)
        {
            return false;
        }

        if (entity.MoveData.Speed <= 0)
        {
            return false;
        }

        if (!entity.BattleDataCore.CheckCanMove())
        {
            return false;
        }

        FsmState<EntityStatusCtrl> curStatus = GetCurrentStatus(entity);
        if (curStatus == null)
        {
            return false;
        }

        if (curStatus is not IEntityCanMove judge)
        {
            return false;
        }

        return judge.CheckCanMove();
    }

    /// <summary>
    /// 检查实体是否能够放技能
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="skillID">需要检测的具体技能ID  如果想要知道任意技能是否能够释放 就传入0</param>
    /// <returns></returns>
    public static bool CheckEntityCanSkill(EntityBase entity, int skillID)
    {
        //现在下坡稍微有一点就会浮空 导致技能释放失败 这里先屏蔽 这里现在是玩家控制的 先不校验这个条件 重力还需要统一好好处理
        // if (!entity.MoveData.IsGrounded)
        // {
        //     return false;
        // }


        if (!entity.BattleDataCore.CheckCanSkill())
        {
            return false;
        }

        FsmState<EntityStatusCtrl> curStatus = GetCurrentStatus(entity);
        if (curStatus == null)
        {
            return false;
        }

        if (curStatus is not IEntityCanSkill judge)
        {
            return false;
        }

        return judge.CheckCanSkill(skillID);
    }

    /// <summary>
    /// 获取实体死亡结束剩余时间
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static float GetDeathEndTime(EntityBase entity)
    {

        FsmState<EntityStatusCtrl> curStatus = GetCurrentStatus(entity);
        if (curStatus == null)
        {
            return 0;
        }

        if (curStatus.StatusName != DeathStatusCore.Name)
        {
            return 0;
        }

        return (curStatus as DeathStatusCore).GetDeathEndTime();
    }
}