using GameFramework.Fsm;
using GameMessageCore;
using UnityGameFramework.Runtime;
using static HomeDefine;

/// <summary>
/// 土地成熟后一些特殊种子会切换到这个状态 给各特殊逻辑扩展 比如图腾
/// </summary>
public class SoilSpecialFunctionStatusCore : SoilStatusCore
{
    public override eSoilStatus StatusFlag => eSoilStatus.SpecialFunction;

    public override eAction SupportAction => eAction.None;

    protected override float AutoEnterNextStatusTime => 0;

    /// <summary>
    /// 当特殊功能状态下业务完结需要重新进入到空白土地时调用
    /// </summary>
    protected virtual void OnFinishStatus()
    {
        SoilData.ClearSeedData();
        ChangeState(eSoilStatus.Loose);
    }

    protected override void OnEnter(IFsm<SoilStatusCtrl> fsm)
    {
        base.OnEnter(fsm);

        SeedFunctionType functionType = (SeedFunctionType)SoilData.DRSeed.FunctionType;
        if (functionType == SeedFunctionType.None)
        {
            Log.Error("进入特殊功能状态时没有特殊功能类型");
            OnFinishStatus();
            return;
        }

        StatusCtrl.SoilEvent.OnFunctionSeedRipe?.Invoke(functionType);
    }
}