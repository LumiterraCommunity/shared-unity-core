using static HomeDefine;

/// <summary>
/// 土地成熟等待腐败收获状态
/// </summary>
public class SoilHarvestPerishStatusCore : SoilStatusCore
{
    public override eSoilStatus StatusFlag => eSoilStatus.HarvestPerish;

    public override eAction SupportAction => eAction.Harvest;

    protected override float AutoEnterNextStatusTime => 0;

    protected override void OnExecuteHomeAction(eAction action, object actionData)
    {
        base.OnExecuteHomeAction(action, actionData);

        SoilData.ClearSeedData();
        ChangeState(eSoilStatus.Loose);
    }
}