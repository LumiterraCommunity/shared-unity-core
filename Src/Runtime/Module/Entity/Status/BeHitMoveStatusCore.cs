/*
 * @Author: xiang huan
 * @Date: 2022-07-25 15:56:56
 * @Description: 受击移动
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Status/BeHitMoveStatusCore.cs
 * 
 */
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameFramework.Fsm;
using UnityGameFramework.Runtime;

/// <summary>
/// 受击移动状态基类 
/// </summary>
public class BeHitMoveStatusCore : ListenEventStatusCore
{

    protected CancellationTokenSource CancelToken;

    public static new string Name => "beHitMove";
    public override string StatusName => Name;
    protected override Type[] EventFunctionTypes => new Type[]
    {
        typeof(BeStunEventFunc),
        typeof(BeCapturedEventFunc),
    };
    private int _moveTime;

    protected override void OnEnter(IFsm<EntityStatusCtrl> fsm)
    {
        base.OnEnter(fsm);
        _moveTime = OwnerFsm.GetData<VarInt32>(StatusDataDefine.BE_HIT_MOVE_TIME).Value;
        MoveStart();
    }

    protected override void OnLeave(IFsm<EntityStatusCtrl> fsm, bool isShutdown)
    {
        CancelTime();
        _moveTime = 0;
        _ = OwnerFsm.RemoveData(StatusDataDefine.BE_HIT_MOVE_TIME);
        base.OnLeave(fsm, isShutdown);
    }

    // 取消蓄力
    private void CancelTime()
    {
        if (CancelToken != null)
        {
            CancelToken.Cancel();
            CancelToken = null;
        }
    }

    protected virtual async void MoveStart()
    {
        CancelTime();
        try
        {
            CancelToken = new();
            await UniTask.Delay(_moveTime, false, PlayerLoopTiming.Update, CancelToken.Token);
            CancelToken = null;
        }
        catch (OperationCanceledException)
        {
            return;
        }
        catch (Exception e)
        {
            Log.Error($"BeHitMove MoveStart Error,{e.Message},{e.StackTrace}{e}");
            return;
        }

        MoveEnd();
    }
    protected virtual void MoveEnd()
    {
        if (RefEntityIsDead())
        {
            ChangeState(OwnerFsm, DeathStatusCore.Name);
            return;
        }
        ChangeState(OwnerFsm, IdleStatusCore.Name);
    }

    protected override void OnUpdate(IFsm<EntityStatusCtrl> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        //霸体状态，退出受击移动状态
        if (StatusCtrl.RefEntity.BattleDataCore.HasBattleState(BattleDefine.eBattleState.Endure))
        {
            ChangeState(fsm, IdleStatusCore.Name);
            return;
        }
    }
}