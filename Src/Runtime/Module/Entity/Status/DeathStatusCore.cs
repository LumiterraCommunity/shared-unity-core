/*
 * @Author: xiang huan
 * @Date: 2022-08-07 10:29:02
 * @Description: 死亡状态
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Status/DeathStatusCore.cs
 * 
 */
using System.Threading;
using Cysharp.Threading.Tasks;
using GameFramework.Fsm;
using GameMessageCore;
using UnityEngine;

/// <summary>
/// 死亡状态通用状态基类
/// </summary>
public class DeathStatusCore : ListenEventStatusCore, IEntityCanMove, IEntityCanSkill
{
    public static new string Name = "death";

    public override string StatusName => Name;
    protected CancellationTokenSource CancelToken;
    protected virtual int DeathTime => 3000;  //死亡总时间
    protected float DeathStartTime = 0;   //死亡开始时间
    protected bool IsStopGravityDeath;
    protected override void OnEnter(IFsm<EntityStatusCtrl> fsm)
    {
        base.OnEnter(fsm);


        //非灵魂状态开始死亡
        if (!RefEntityIsSoul())
        {
            OnDeathStart();
        }

        //需要停止重力的死亡
        if (StatusCtrl.RefEntity.BattleDataCore.DeathReason is DamageState.Fall or DamageState.WaterDrown)
        {
            if (StatusCtrl.TryGetComponent(out CharacterMoveCtrl moveCtrl))
            {
                moveCtrl.StopCurSpeed();
                moveCtrl.SetEnableGravity(false);
            }
            IsStopGravityDeath = true;
        }

        ISceneDamageDetection[] detections = StatusCtrl.GetComponents<ISceneDamageDetection>();
        for (int i = 0; i < detections.Length; i++)
        {
            detections[i].StopDetection();
        }
        StatusCtrl.RefEntity.EntityEvent.EnterDeath?.Invoke();
    }

    protected override void OnLeave(IFsm<EntityStatusCtrl> fsm, bool isShutdown)
    {
        CancelTimeDeath();

        if (IsStopGravityDeath)
        {
            if (StatusCtrl.TryGetComponent(out CharacterMoveCtrl moveCtrl))
            {
                moveCtrl.SetEnableGravity(true);
            }
            IsStopGravityDeath = false;
        }

        ISceneDamageDetection[] detections = StatusCtrl.GetComponents<ISceneDamageDetection>();
        for (int i = 0; i < detections.Length; i++)
        {
            detections[i].StartDetection();
        }
        StatusCtrl.RefEntity.EntityEvent.ExitDeath?.Invoke();
        base.OnLeave(fsm, isShutdown);
    }

    protected override void AddEvent(EntityEvent entityEvent)
    {
        entityEvent.EntityBeReborn += OnBeReborn;
    }

    protected override void RemoveEvent(EntityEvent entityEvent)
    {
        entityEvent.EntityBeReborn -= OnBeReborn;
    }

    protected override void OnUpdate(IFsm<EntityStatusCtrl> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        if (RefEntityIsSoul())
        {
            ChangeState(fsm, DeathSoulStatusCore.Name);
            return;
        }
    }
    private void CancelTimeDeath()
    {
        if (CancelToken != null)
        {
            CancelToken.Cancel();
            CancelToken = null;
        }
    }

    /// <summary>
    /// 死亡开始
    /// </summary>
    /// <value></value>
    protected virtual async void OnDeathStart()
    {
        DeathStartTime = Time.realtimeSinceStartup;
        CancelTimeDeath();
        try
        {
            CancelToken = new();
            await UniTask.Delay(DeathTime, false, PlayerLoopTiming.Update, CancelToken.Token);
            CancelToken = null;
        }
        catch (System.Exception)
        {
            return;
        }
        OnDeathEnd();
    }

    /// <summary>
    /// 死亡结束
    /// </summary>
    /// <value></value>
    protected virtual void OnDeathEnd()
    {
        _ = StatusCtrl.RefEntity.BattleDataCore.ChangeIsSoul(true);
    }

    /// <summary>
    /// 复活
    /// </summary>
    /// <value></value>
    protected virtual void OnBeReborn()
    {
        ChangeState(OwnerFsm, IdleStatusCore.Name);
    }

    public bool CheckCanMove()
    {
        return false;
    }

    public bool CheckCanSkill(int skillId)
    {
        return false;
    }

    /// <summary>
    /// 获取死亡结束剩余时间
    /// </summary>
    public float GetDeathEndTime()
    {
        float time = Time.realtimeSinceStartup - DeathStartTime;
        return DeathTime * TimeUtil.MS2S - time;
    }
}