/*
 * @Author: xiang huan
 * @Date: 2022-07-25 15:56:56
 * @Description: 蓄力状态
 * @FilePath: /Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Status/SkillAccumulateStatusCore.cs
 * 
 */
using System.Threading;
using GameFramework.Fsm;
using UnityGameFramework.Runtime;
using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// 蓄力状态通用状态基类 
/// </summary>

public class SkillAccumulateStatusCore : ListenEventStatusCore, IEntityCanMove, IEntityCanSkill
{
    protected int SkillID;
    protected long[] Targets;
    protected UnityEngine.Vector3 SkillDir;
    protected double SkillTimeScale;
    protected DRSkill CurSkillCfg;
    private EntityInputData _inputData;
    protected CancellationTokenSource CancelToken;

    protected InputSkillReleaseData InputSkillData;

    public static new string Name => "skillAccumulate";
    public override string StatusName => Name;
    /// <summary>
    /// 是否正常继续战斗状态离开的 false以为着是打断离开的
    /// </summary>
    protected bool IsContinueBattleLeave;

    protected override Type[] EventFunctionTypes => new Type[] {
        typeof(OnInputSkillInBattleStatusEventFunc),
        typeof(BeHitMoveEventFunc),
        typeof(BeHitEventFunc),
        typeof(BeStunEventFunc),
        typeof(BeCapturedEventFunc),
        typeof(StopHoldSkillEventFunc),
    };

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
        fsm.SetData<VarVector3>(StatusDataDefine.SKILL_START_POS, StatusCtrl.RefEntity.Position);

        if (CurSkillCfg == null)
        {
            Log.Error($"AccumulateStatusCore DRSkill is null skillID = {SkillID}");
            return;
        }
        if (CurSkillCfg.AccuBreakable)
        {
            _inputData = StatusCtrl.GetComponent<EntityInputData>();
        }

        if (CurSkillCfg.AccuTime > 0)
        {
            StartTimingAccumulate();
        }
        else
        {
            EndAccumulate();
        }
    }

    // 蓄力动画
    protected virtual void StartAccumulateAnim()
    {
        //.. 
    }

    protected override void OnLeave(IFsm<EntityStatusCtrl> fsm, bool isShutdown)
    {
        CancelTimeAccumulate();
        _inputData = null;
        Targets = null;
        SkillDir = UnityEngine.Vector3.zero;
        CurSkillCfg = null;
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

    // 取消蓄力
    private void CancelTimeAccumulate()
    {
        if (CancelToken != null)
        {
            CancelToken.Cancel();
            CancelToken = null;
        }
    }

    // 开始定时蓄力
    protected virtual async void StartTimingAccumulate()
    {
        StartAccumulateAnim();
        CancelTimeAccumulate();
        try
        {
            CancelToken = new();
            await UniTask.Delay((int)(CurSkillCfg.AccuTime / SkillTimeScale), false, PlayerLoopTiming.Update, CancelToken.Token);
            CancelToken = null;
        }
        catch (System.Exception)
        {
            return;
        }
        EndAccumulate();
    }
    // 结束蓄力
    protected virtual void EndAccumulate()
    {
        IsContinueBattleLeave = true;
        ChangeState(OwnerFsm, SkillForwardStatusCore.Name);
    }

    public bool CheckCanMove()
    {
        return _inputData && CurSkillCfg.AccuBreakable;
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