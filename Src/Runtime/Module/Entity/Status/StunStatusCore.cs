using System;
using GameFramework.Fsm;
/// <summary>
/// 实体眩晕状态
/// </summary>
public class StunStatusCore : ListenEventStatusCore, IEntityCanMove, IEntityCanSkill
{
    public static new string Name = "stun";

    public override string StatusName => Name;
    private int _stunStatusCounter;

    protected override Type[] EventFunctionTypes => new Type[] {
        typeof(BeCapturedEventFunc),
    };
    public bool CheckCanMove()
    {
        return false;
    }

    public bool CheckCanSkill(int skillID)
    {
        return false;
    }

    protected override void OnEnter(IFsm<EntityStatusCtrl> fsm)
    {
        base.OnEnter(fsm);
        _stunStatusCounter = 1;//默认为1，因为进入状态时，已经受到了眩晕效果
        StatusCtrl.RefEntity.EntityEvent.EntityReceiveStunEffect += OnEntityReceiveStunEffect;
        StatusCtrl.RefEntity.EntityEvent.EntityRemoveStunEffect += OnEntityRemoveStunEffect;
    }

    protected override void OnLeave(IFsm<EntityStatusCtrl> fsm, bool isShutdown)
    {
        StatusCtrl.RefEntity.EntityEvent.EntityReceiveStunEffect -= OnEntityReceiveStunEffect;
        StatusCtrl.RefEntity.EntityEvent.EntityRemoveStunEffect -= OnEntityRemoveStunEffect;
        base.OnLeave(fsm, isShutdown);
    }

    protected override void OnUpdate(IFsm<EntityStatusCtrl> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        if (RefEntityIsDead())
        {
            ChangeState(fsm, DeathStatusCore.Name);
            return;
        }

        //霸体状态，退出眩晕状态
        if (StatusCtrl.RefEntity.BattleDataCore.HasBattleState(BattleDefine.eBattleState.Endure))
        {
            ChangeState(fsm, IdleStatusCore.Name);
            return;
        }

        if (_stunStatusCounter <= 0)
        {
            ChangeState(fsm, IdleStatusCore.Name);
        }
    }

    private void OnEntityReceiveStunEffect()
    {
        _stunStatusCounter++;
    }

    private void OnEntityRemoveStunEffect()
    {
        _stunStatusCounter--;
    }
}