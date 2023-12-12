/* 
 * @Author XQ
 * @Date 2022-08-17 10:37:20
 * @FilePath: /Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Status/IdleStatusCore.cs
 */
using System;
using GameFramework.Fsm;

/// <summary>
/// 闲置状态通用状态基类
/// </summary>
public class IdleStatusCore : ListenEventStatusCore, IEntityCanMove, IEntityCanSkill
{
    public static new string Name = "idle";

    public override string StatusName => Name;

    private EntityInputData _inputData;

    protected override Type[] EventFunctionTypes => new Type[] {
        typeof(BeHitEventFunc),
        typeof(BeHitMoveEventFunc),
        typeof(WaitToBattleStatusEventFunc),
        typeof(BeStunEventFunc),
        typeof(BeCapturedEventFunc),
    };
    protected override void OnEnter(IFsm<EntityStatusCtrl> fsm)
    {
        base.OnEnter(fsm);

        _inputData = StatusCtrl.GetComponent<EntityInputData>();
    }

    protected override void OnLeave(IFsm<EntityStatusCtrl> fsm, bool isShutdown)
    {
        _inputData = null;

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

        if (StatusCtrl.RefEntity.MoveData != null && !StatusCtrl.RefEntity.MoveData.IsGrounded)
        {
            ChangeState(fsm, FloatInAirStatusCore.Name);
            return;
        }

        if (CheckCanMove())
        {
            if (_inputData.InputMoveDirection != null)
            {
                ChangeState(fsm, DirectionMoveStatusCore.Name);
            }
            else if (_inputData.InputMovePath.Count > 0)
            {
                ChangeState(fsm, PathMoveStatusCore.Name);
            }
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