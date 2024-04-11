using static HomeDefine;

/// <summary>
/// 土地腐败成熟状态
/// </summary>
public class SoilRipePerishStatusCore : SoilStatusCore
{
    public override eSoilStatus StatusFlag => eSoilStatus.RipePerish;

    public override eAction SupportAction => eAction.Harvest;

    protected override float AutoEnterNextStatusTime => 0;

    protected override void OnExecuteHomeAction(eAction action, object actionData)
    {
        base.OnExecuteHomeAction(action, actionData);

        SoilData.ClearSeedData();
        ChangeState(eSoilStatus.Loose);
    }
}