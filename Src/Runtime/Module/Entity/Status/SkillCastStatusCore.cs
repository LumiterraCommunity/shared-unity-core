/** 
 * @Author XQ
 * @Date 2022-08-10 16:27:01
 * @FilePath /Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Status/SkillCastStatusCore.cs
 */
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 技能施法状态 正式产生伤害阶段
/// </summary>
public class SkillCastStatusCore : ListenEventStatusCore, IEntityCanSkill
{
    public static new string Name => "skillCast";

    public override string StatusName => Name;
    protected int SkillID;
    protected long[] Targets;
    protected Vector3 SkillDir;

    protected InputSkillReleaseData InputSkillData;
    protected double SkillTimeScale;

    protected DRSkill CurSkillCfg;
    private CancellationTokenSource _castTimeToken;

    private bool _continueNextSkill;//是否继续下一个技能
    protected override Type[] EventFunctionTypes => new Type[] {
        typeof(BeHitMoveEventFunc),
        typeof(BeHitEventFunc),
        typeof(BeCapturedEventFunc),
    };

    protected override void OnEnter(IFsm<EntityStatusCtrl> fsm)
    {
        base.OnEnter(fsm);

        InputSkillData = fsm.GetData<VarInputSkill>(StatusDataDefine.SKILL_INPUT).Value;
        SkillID = InputSkillData.SkillID;
        SkillDir = InputSkillData.Dir;
        Targets = InputSkillData.Targets;
        SkillTimeScale = InputSkillData.SkillTimeScale;

        CurSkillCfg = GFEntryCore.DataTable.GetDataTable<DRSkill>().GetDataRow(SkillID);
        if (CurSkillCfg.ReleaseSpd != 0)
        {
            float releaseSpd = StatusCtrl.RefEntity.EntityAttributeData.GetRealValue((eAttributeType)CurSkillCfg.ReleaseSpd);
            SkillTimeScale = Math.Max(1 + releaseSpd, 0.1f) * SkillTimeScale;
        }
        try
        {
#if UNITY_EDITOR
            if (CurSkillCfg.SkillRange != null && CurSkillCfg.SkillRange.Length > 0)//锁定目标技能 不一定配置范围
            {
                if (StatusCtrl.TryGetComponent(out SkillShapeGizmos skillShapeGizmos))
                {
                    skillShapeGizmos.StartDraw(CurSkillCfg.SkillRange, StatusCtrl.gameObject, SkillDir);
                }
            }
#endif
            SkillCastExecute(CurSkillCfg);
        }
        catch (System.Exception e)
        {
            Log.Error($"skill cast execute error ={e}");
        }
        if (CurSkillCfg == null)
        {
            return;
        }
        CancelTimeCastFinish();

        //如果是持续技能 不用定时去下个阶段 而是需要等待取消动作
        if (CurSkillCfg.IsHoldSkill)
        {
            StatusCtrl.RefEntity.EntityEvent.TryStopHoldSkill += StopHoldSkill;
        }
        else
        {
            TimeCastFinish();
        }

        try
        {
            StatusCtrl.RefEntity.EntityEvent.EnterSkillCast?.Invoke(InputSkillData, CurSkillCfg);
        }
        catch (Exception e)
        {
            Log.Error($"Skill cast invoke EnterSkillCast error ={e}");
        }
    }

    protected override void OnLeave(IFsm<EntityStatusCtrl> fsm, bool isShutdown)
    {
        try
        {
            StatusCtrl.RefEntity.EntityEvent.ExitSkillCast?.Invoke();
            StatusCtrl.RefEntity.EntityEvent.OnSkillStatusEnd?.Invoke(CurSkillCfg, false);
        }
        catch (Exception e)
        {
            Log.Error($"Skill cast invoke ExitSkillCast error ={e}");
        }

        if (CurSkillCfg.IsHoldSkill)
        {
            StatusCtrl.RefEntity.EntityEvent.TryStopHoldSkill -= StopHoldSkill;
        }
        else
        {
            CancelTimeCastFinish();
        }

        CurSkillCfg = null;
        Targets = null;
        SkillDir = Vector3.zero;
#if UNITY_EDITOR
        if (StatusCtrl.TryGetComponent(out SkillShapeGizmos skillShapeGizmos))
        {
            skillShapeGizmos.StopDraw();
        }
#endif

        if (!_continueNextSkill)
        {
            _ = fsm.RemoveData(StatusDataDefine.SKILL_INPUT);
            _continueNextSkill = false;
        }
        base.OnLeave(fsm, isShutdown);
    }

    protected override void AddEvent(EntityEvent entityEvent)
    {
        base.AddEvent(entityEvent);

        entityEvent.InputSkillRelease += OnInputSkillRelease;
    }

    protected override void RemoveEvent(EntityEvent entityEvent)
    {
        base.RemoveEvent(entityEvent);

        entityEvent.InputSkillRelease -= OnInputSkillRelease;
    }
    protected override void OnUpdate(IFsm<EntityStatusCtrl> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        if (RefEntityIsDead())
        {
            OnBeforeChangeToDeath();
            ChangeState(fsm, DeathStatusCore.Name);
            return;
        }

        //不走事件切换，事件切换有可能会在释放技能过程中，触发其他技能，打断释放流程
        if (StatusCtrl.RefEntity.BattleDataCore.HasBattleState(BattleDefine.eBattleState.Stun))
        {
            ChangeState(fsm, StunStatusCore.Name);
            return;
        }
    }
    protected virtual void OnInputSkillRelease(InputSkillReleaseData inputData)
    {
        SeContinueNextSkill(true);

        StatusCtrl.RefEntity.SetForward(inputData.Dir);
        OwnerFsm.SetData<VarInputSkill>(StatusDataDefine.SKILL_INPUT, inputData);
        ChangeState(OwnerFsm, SkillAccumulateStatusCore.Name);
    }

    //施法完成定时
    private async void TimeCastFinish()
    {
        CancelTimeCastFinish();

        try
        {
            _castTimeToken = new();
            int delayTime = (int)((CurSkillCfg.ReleaseTime - CurSkillCfg.ForwardReleaseTime) / SkillTimeScale);
            await UniTask.Delay(delayTime, false, PlayerLoopTiming.Update, _castTimeToken.Token);
        }
        catch (System.Exception)
        {
            return;
        }
        _castTimeToken = null;
        OnFinishChangeToNextStatus();
    }

    // 取消施法完成定时
    private void CancelTimeCastFinish()
    {
        if (_castTimeToken != null)
        {
            _castTimeToken.Cancel();
            _castTimeToken = null;
        }
    }

    /// <summary>
    /// 停止持续技能的持续行为
    /// </summary>
    protected virtual void StopHoldSkill()
    {
        OnFinishChangeToNextStatus();
    }

    /// <summary>
    /// 技能施法开始执行 子类覆写
    /// </summary>
    /// <param name="curSkillCfg"></param>
    protected virtual void SkillCastExecute(DRSkill curSkillCfg) { }
    /// <summary>
    /// 后摇完成需要切换到下一个状态的时候覆写
    /// </summary>
    protected virtual void OnFinishChangeToNextStatus()
    {
        ChangeState(OwnerFsm, IdleStatusCore.Name);
    }

    /// <summary>
    /// 在死亡了 需要切换到死亡状态前执行的额外操作
    /// </summary>
    protected virtual void OnBeforeChangeToDeath()
    {
        SeContinueNextSkill(false);
    }

    /// <summary>
    /// 设置是否继续下一个技能 如果有下一个技能 基类就不会离开状态清理技能数据
    /// </summary>
    /// <param name="isContinue"></param>
    protected void SeContinueNextSkill(bool isContinue)
    {
        _continueNextSkill = isContinue;
    }

    public virtual bool CheckCanSkill(int skillID)
    {
        //是翻滚动作
        if (StatusCtrl.TryGetComponent(out PlayerRoleDataCore playerData) && playerData.DRRole.JumpRollSkill == skillID)
        {
            return true;
        }

        return false;
    }
}