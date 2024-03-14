using GameFramework.Fsm;
using static HomeDefine;

/// <summary>
/// 土地成熟状态 会自动跳转下一个状态 可能是收获也可能是特殊功能状态
/// </summary>
public class SoilRipeStatusCore : SoilStatusCore
{
    public override eSoilStatus StatusFlag => eSoilStatus.Ripe;

    public override eAction SupportAction => eAction.None;

    protected override float AutoEnterNextStatusTime => 0;

    protected override void OnUpdate(IFsm<SoilStatusCtrl> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

        //特殊功能实体
        if (HomeUtilCore.JudgeSeedIsFunctionEntityType(SoilData))
        {
            ChangeState(eSoilStatus.SpecialFunction);
        }
        else//不同种子可以收获
        {
            ChangeState(eSoilStatus.Harvest);
        }
    }
}