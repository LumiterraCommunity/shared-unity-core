using GameFramework.Fsm;
using GameMessageCore;
using UnityGameFramework.Runtime;
using static HomeDefine;

/// <summary>
/// 土地成熟等待收获状态
/// </summary>
public class SoilRipeStatusCore : SoilStatusCore
{
    public override eSoilStatus StatusFlag => eSoilStatus.Ripe;

    private eAction _supportAction = eAction.Harvest;
    public override eAction SupportAction => _supportAction;

    protected override float AutoEnterNextStatusTime => 0;


    private bool _waitGoSpecialFunctionStatus;

    protected override void OnEnter(IFsm<SoilStatusCtrl> fsm)
    {
        base.OnEnter(fsm);

        _waitGoSpecialFunctionStatus = false;

        if (((SeedFunctionType)SoilData.DRSeed.FunctionType) != SeedFunctionType.None)
        {
            _supportAction = eAction.None;
            _waitGoSpecialFunctionStatus = true;
        }
    }

    protected override void OnLeave(IFsm<SoilStatusCtrl> fsm, bool isShutdown)
    {
        _supportAction = eAction.Harvest;
        _waitGoSpecialFunctionStatus = false;

        base.OnLeave(fsm, isShutdown);
    }

    protected override void OnUpdate(IFsm<SoilStatusCtrl> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

        if (_waitGoSpecialFunctionStatus)
        {
            _waitGoSpecialFunctionStatus = false;
            ChangeState(eSoilStatus.SpecialFunction);
        }
    }

    protected override void OnExecuteHomeAction(eAction action, object actionData)
    {
        if (_waitGoSpecialFunctionStatus)//防止同一帧收到消息的极端情况被收获掉了
        {
            Log.Error("当前状态等待进入特殊功能状态，不应该触发收获动作");
            return;
        }

        base.OnExecuteHomeAction(action, actionData);

        SoilData.ClearSeedData();
        ChangeState(eSoilStatus.Loose);
    }
}