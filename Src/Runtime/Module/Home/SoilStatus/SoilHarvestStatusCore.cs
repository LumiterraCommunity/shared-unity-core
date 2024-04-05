using static HomeDefine;

/// <summary>
/// 土地等待收割状态 只有普通种子才能收割
/// </summary>
public class SoilHarvestStatusCore : SoilStatusCore
{
    public override eSoilStatus StatusFlag => eSoilStatus.Harvest;

    public override eAction SupportAction => eAction.Harvest;

    protected override float AutoEnterNextStatusTime => 0;
    protected override void OnExecuteHomeAction(eAction action, object actionData)
    {
        base.OnExecuteHomeAction(action, actionData);

        SoilData.ClearSeedData();
        ChangeState(eSoilStatus.Loose);
    }
}