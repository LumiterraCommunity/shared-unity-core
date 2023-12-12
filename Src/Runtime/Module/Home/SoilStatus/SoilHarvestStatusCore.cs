using static HomeDefine;

/// <summary>
/// 土地成熟等待收获状态
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