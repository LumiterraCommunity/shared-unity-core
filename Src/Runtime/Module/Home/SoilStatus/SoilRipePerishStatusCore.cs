using static HomeDefine;

/// <summary>
/// 土地腐败成熟状态
/// </summary>
public class SoilRipePerishStatusCore : SoilStatusCore
{
    public override eSoilStatus StatusFlag => eSoilStatus.RipePerish;

    public override eAction SupportAction => eAction.Harvest;

    protected override float AutoEnterNextStatusTime => 0;

    protected override void OnExecuteHomeAction(eAction action, long playerId, long entityId, object actionData)
    {
        base.OnExecuteHomeAction(action, playerId, entityId, actionData);

        SoilData.ClearSeedData();
        ChangeState(eSoilStatus.Idle);
    }
}