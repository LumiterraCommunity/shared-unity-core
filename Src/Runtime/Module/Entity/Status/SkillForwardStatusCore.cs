using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameFramework.Fsm;
using UnityGameFramework.Runtime;

/// <summary>
/// 技能前摇状态
/// </summary>
public abstract class SkillForwardStatusCore : ListenEventStatusCore, IEntityCanMove, IEntityCanSkill
{
    public static new string Name => "skillForward";

    public override string StatusName => Name;
    protected override Type[] EventFunctionTypes => new Type[]
    {
        typeof(OnInputSkillInBattleStatusEventFunc),
        typeof(BeHitMoveEventFunc),
        typeof(BeHitEventFunc),
        typeof(BeStunEventFunc),
        typeof(BeCapturedEventFunc),
        typeof(StopHoldSkillEventFunc),
    };

    private CancellationTokenSource _forwardTimeToken;

    protected DRSkill CurSkillCfg;
    private EntityInputData _inputData;
    protected int SkillID;
    protected long[] Targets;
    protected UnityEngine.Vector3 SkillDir;
    protected double SkillTimeScale;
    protected InputSkillReleaseData InputSkillData;

    /// <summary>
    /// 是否正常继续战斗状态离开的 false以为着是打断离开的
    /// </summary>
    protected bool IsContinueBattleLeave;

    protected override void OnEnter(IFsm<EntityStatusCtrl> fsm)
    {
        base.OnEnter(fsm);

        IsContinueBattleLeave = false;

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
            SkillForwardExecute(CurSkillCfg);
        }
        catch (Exception e)
        {
            Log.Error($"skill forward execute error ={e}");
        }

        if (CurSkillCfg == null)
        {
            return;
        }

        if (CurSkillCfg.AccuBreakable)
        {
            _inputData = StatusCtrl.GetComponent<EntityInputData>();
        }

        TimeForwardFinish();

        try
        {
            StatusCtrl.RefEntity.EntityEvent.EnterSkillForward?.Invoke(CurSkillCfg);
        }
        catch (Exception e)
        {
            Log.Error($"skill forward invoke EnterSkillForward error ={e}");
        }
    }

    protected override void OnLeave(IFsm<EntityStatusCtrl> fsm, bool isShutdown)
    {
        try
        {
            StatusCtrl.RefEntity.EntityEvent.ExitSkillForward?.Invoke(CurSkillCfg, !IsContinueBattleLeave);
        }
        catch (Exception e)
        {
            Log.Error($"skill forward invoke ExitSkillForward error ={e}");
        }

        if (!IsContinueBattleLeave)//打断技能
        {
            try
            {
                StatusCtrl.RefEntity.EntityEvent.OnSkillStatusEnd?.Invoke(CurSkillCfg, true);
                AbolishSkillForwardEffect(CurSkillCfg, StatusCtrl.RefEntity);
            }
            catch (System.Exception e)
            {
                Log.Error($"on skill forward event invoke error = {e}");
            }
        }

        CancelTimeForwardFinish();

        _inputData = null;
        CurSkillCfg = null;
        Targets = null;
        SkillDir = UnityEngine.Vector3.zero;
        IsContinueBattleLeave = false;

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

    //前摇完成定时
    private async void TimeForwardFinish()
    {
        CancelTimeForwardFinish();

        try
        {
            _forwardTimeToken = new();
            await UniTask.Delay((int)(CurSkillCfg.ForwardReleaseTime / SkillTimeScale), false, PlayerLoopTiming.Update, _forwardTimeToken.Token);
        }
        catch (Exception)
        {
            return;
        }
        _forwardTimeToken = null;

        IsContinueBattleLeave = true;
        ChangeState(OwnerFsm, SkillCastStatusCore.Name);
    }

    // 取消前摇完成定时
    private void CancelTimeForwardFinish()
    {
        if (_forwardTimeToken != null)
        {
            _forwardTimeToken.Cancel();
            _forwardTimeToken = null;
        }
    }

    /// <summary>
    /// 技能前摇开始执行 子类覆写
    /// </summary>
    /// <param name="drSkill"></param>
    protected abstract void SkillForwardExecute(DRSkill drSkill);

    /// <summary>
    /// 执行前摇效果 子类调用
    /// </summary>
    /// <param name="drSkill">技能表数据</param>
    /// <param name="entity">实体</param>
    protected void SkillForwardEffectExecute(DRSkill drSkill, EntityBase entity)
    {
        _ = SkillUtil.EntitySkillEffectExecute(InputSkillData, SkillUtil.GetSkillEffect(entity, drSkill, eSkillEffectApplyType.Forward), entity, entity);
    }


    /// <summary>
    /// 取消技能前摇效果
    /// </summary>
    /// <param name="drSkill"></param>
    /// <param name="entity"></param>
    protected void AbolishSkillForwardEffect(DRSkill drSkill, EntityBase entity)
    {
        SkillUtil.EntityAbolishSkillEffect(SkillID, SkillUtil.GetSkillEffect(entity, drSkill, eSkillEffectApplyType.Forward), entity, entity);
    }
    public bool CheckCanMove()
    {
        return _inputData && CurSkillCfg.AccuBreakable;//能打断 时有移动输入时切换移动
    }

    public bool CheckCanSkill(int skillID)
    {
        //是翻滚动作
        if (StatusCtrl.TryGetComponent(out PlayerRoleDataCore playerData) && playerData.DRRole.JumpRollSkill == skillID)
        {
            return true;
        }

        return false;
    }
}