
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameFramework.Fsm;

/// <summary>
/// 采集格子建筑状态通用状态基类
/// </summary>
public class CollectStatusCore : ListenEventStatusCore, IEntityCanMove, IEntityCanSkill
{
    public static new string Name = "Collect";
    protected virtual int CollectTime => 1500;
    public override string StatusName => Name;

    private EntityInputData _inputData;
    protected CancellationTokenSource CancelToken;


    protected override Type[] EventFunctionTypes => new Type[] {
        typeof(BeHitEventFunc ),
        typeof(BeHitMoveEventFunc),
        typeof(WaitToBattleStatusEventFunc),
        typeof(BeStunEventFunc),
    };
    protected override void OnEnter(IFsm<EntityStatusCtrl> fsm)
    {
        base.OnEnter(fsm);

        _inputData = StatusCtrl.GetComponent<EntityInputData>();
        CollectStart();
    }

    protected override void OnLeave(IFsm<EntityStatusCtrl> fsm, bool isShutdown)
    {
        _inputData = null;
        CancelTimeCollect();
        base.OnLeave(fsm, isShutdown);
    }

    // 取消蓄力
    private void CancelTimeCollect()
    {
        if (CancelToken != null)
        {
            CancelToken.Cancel();
            CancelToken = null;
        }
    }

    protected virtual async void CollectStart()
    {
        CancelTimeCollect();
        try
        {
            CancelToken = new();
            await UniTask.Delay(CollectTime, false, PlayerLoopTiming.Update, CancelToken.Token);
            CancelToken = null;
        }
        catch (System.Exception)
        {
            return;
        }
        CollectEnd();
    }
    protected virtual void CollectEnd()
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