
using GameFramework.Fsm;

/// <summary>
/// 怪物被捕获完成的状态通用状态基类
/// </summary>/*
public class BeCapturedStatusCore : ListenEventStatusCore, IEntityCanMove, IEntityCanSkill
{
    public static new string Name = "BeCaptured";

    public override string StatusName => Name;
    protected virtual int CaptureSuccEndDurTime => 3000;
    protected override void OnEnter(IFsm<EntityStatusCtrl> fsm)
    {
        base.OnEnter(fsm);

        OnBeCapturedStart();
    }

    protected override void OnLeave(IFsm<EntityStatusCtrl> fsm, bool isShutdown)
    {
        CancelTimeCapture();
        base.OnLeave(fsm, isShutdown);
    }
    private void CancelTimeCapture()
    {
        _ = TimerMgr.RemoveTimer(GetHashCode());
    }

    /// <summary>
    /// 被捕获
    /// </summary>
    /// <value></value>
    protected virtual void OnBeCapturedStart()
    {

        CancelTimeCapture();
        TimerMgr.AddTimer(GetHashCode(), CaptureSuccEndDurTime, OnBeCapturedEnd);
    }

    protected virtual void OnBeCapturedEnd()
    {

    }

    public bool CheckCanMove()
    {
        return false;
    }

    public bool CheckCanSkill(int skillId)
    {
        return false;
    }
}