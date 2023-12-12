/*
 * @Author: xiang huan
 * @Date: 2022-07-25 15:56:56
 * @Description: 受击状态 理论上受击状态只有表现,服务器用不到
 * @FilePath: /Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Status/BeHitStatusCore.cs
 * 
 */
using System;
using GameFramework.Fsm;

/// <summary>
/// 受击状态通用状态基类
/// </summary>
public abstract class BeHitStatusCore : ListenEventStatusCore, IEntityCanMove, IEntityCanSkill
{
    public static new string Name => "beHit";
    protected override Type[] EventFunctionTypes => new Type[] {
        typeof(BeHitMoveEventFunc),
        typeof(WaitToBattleStatusEventFunc),
        typeof(BeStunEventFunc),
        typeof(BeCapturedEventFunc),
     };

    public override string StatusName => Name;
    protected override void OnEnter(IFsm<EntityStatusCtrl> fsm)
    {
        base.OnEnter(fsm);
    }

    protected override void OnLeave(IFsm<EntityStatusCtrl> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
    }

    protected virtual void OnBeHitComplete()
    {
        ChangeState(OwnerFsm, IdleStatusCore.Name);
    }
    protected override void OnUpdate(IFsm<EntityStatusCtrl> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        if (RefEntityIsDead())
        {
            ChangeState(fsm, DeathStatusCore.Name);
            return;
        }
    }
    public bool CheckCanMove()
    {
        return true;
    }

    public bool CheckCanSkill(int skillId)
    {
        return true;
    }
}