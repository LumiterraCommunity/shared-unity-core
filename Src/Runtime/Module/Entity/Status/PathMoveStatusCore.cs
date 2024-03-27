using UnityEngine;
using GameFramework.Fsm;
using UnityGameFramework.Runtime;
using System;

/// <summary>
/// 路径移动状态
/// </summary>
public class PathMoveStatusCore : ListenEventStatusCore, IEntityCanMove, IEntityCanSkill
{
    public static new string Name = "pathMove";

    public override string StatusName => Name;

    protected EntityInputData InputData { get; private set; }
    private DistanceMove _distanceMove;
    protected override Type[] EventFunctionTypes => new Type[] {
        typeof(BeHitMoveEventFunc),
        typeof(WaitToBattleStatusEventFunc),
        typeof(BeStunEventFunc),
        typeof(BeCapturedEventFunc),
    };
    protected override void OnEnter(IFsm<EntityStatusCtrl> fsm)
    {
        base.OnEnter(fsm);

        StatusCtrl.EntityEvent.MoveStart?.Invoke();

        InputData = StatusCtrl.GetComponent<EntityInputData>();

        if (InputData.InputMovePath.Count == 0)
        {
            Log.Error($"path move status enter,not find path ,data empth");
            ChangeState(fsm, IdleStatusCore.Name);
            return;
        }

        if (!StatusCtrl.TryGetComponent(out _distanceMove))
        {
            Log.Error($"path move status enter,not find DistanceMove,name={StatusCtrl.gameObject.name}");
            ChangeState(fsm, IdleStatusCore.Name);
            return;
        }

        MoveToNextPoint();
    }

    protected override void OnLeave(IFsm<EntityStatusCtrl> fsm, bool isShutdown)
    {
        if (InputData != null)
        {
            InputData.ClearInputMovePath(false);
            InputData = null;
        }

        if (_distanceMove != null)
        {
            _distanceMove.StopMove();
            _distanceMove = null;
        }

        StatusCtrl.EntityEvent.MoveStop?.Invoke();

        base.OnLeave(fsm, isShutdown);
    }

    protected virtual void MoveToNextPoint()
    {
        Vector3 nextPos = InputData.InputMovePath.Peek();
        Vector3 offset = nextPos - StatusCtrl.RefEntity.Position;

        //改变朝向
        StatusCtrl.RefEntity.SetForward(new Vector3(offset.x, 0, offset.z));

        //移动
        float speed = StatusCtrl.RefEntity.MoveData.Speed;
        _distanceMove.MoveTo(offset, offset.magnitude, speed, OnNextPointArrived);
    }

    /// <summary>
    /// 下一个拐点走到了
    /// </summary>
    private void OnNextPointArrived()
    {
        //在极端上层清理了InputMovePath后 本状态的update还没执行到 这里同一帧先走到了会出现 就等着update退出状态即可 不用走到下面finish逻辑
        if (InputData.InputMovePath.Count == 0)
        {
            return;
        }

        _ = InputData.InputMovePath.Dequeue();

        if (InputData.InputMovePath.Count == 0)
        {
            OnMoveFinish();
        }
        else
        {
            MoveToNextPoint();
        }
    }

    protected override void AddEvent(EntityEvent entityEvent)
    {
        entityEvent.InputMovePathChanged += OnPathChanged;
        entityEvent.SpecialMoveStartNotMoveStatus += OnSpecialMoveStart;
        entityEvent.InputMovePathMoveStop += OnPathStop;
        entityEvent.EntityMoveDataSpeedUpdate += OnMoveSpeedUpdate;
    }

    protected override void RemoveEvent(EntityEvent entityEvent)
    {
        entityEvent.InputMovePathChanged -= OnPathChanged;
        entityEvent.SpecialMoveStartNotMoveStatus -= OnSpecialMoveStart;
        entityEvent.InputMovePathMoveStop -= OnPathStop;
        entityEvent.EntityMoveDataSpeedUpdate -= OnMoveSpeedUpdate;
    }


    private void OnSpecialMoveStart()
    {
        ChangeState(OwnerFsm, IdleStatusCore.Name);
    }

    private void OnPathStop(Vector3 stopPos)
    {
        InputData.ClearInputMovePath(false);
        _distanceMove.StopMove();
        //移动到停止点, 服务器已经在停止点，而客户端可能还没跑到，这里就移动到停止点
        Vector3 offset = stopPos - StatusCtrl.RefEntity.Position;
        if (offset != Vector3.zero)
        {
            float speed = StatusCtrl.RefEntity.MoveData.Speed;
            _distanceMove.MoveTo(offset, offset.magnitude, speed, OnMoveFinish);
        }
        else
        {
            OnMoveFinish();
        }

    }
    private void OnMoveFinish()
    {
        ChangeState(OwnerFsm, IdleStatusCore.Name);
        StatusCtrl.EntityEvent.OnEntityPathMoveArrived?.Invoke();
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

        if (InputData.InputMovePath.Count == 0)
        {
            ChangeState(OwnerFsm, IdleStatusCore.Name);
            return;
        }
    }

    private void OnMoveSpeedUpdate()
    {
        OnPathChanged();
    }

    //路径改了
    private void OnPathChanged()
    {
        if (InputData.InputMovePath.Count > 0)
        {
            MoveToNextPoint();
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