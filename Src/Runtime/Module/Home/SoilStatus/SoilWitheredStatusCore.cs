using static HomeDefine;

/// <summary>
/// 土地植物干枯状态
/// </summary>
public class SoilWitheredStatusCore : SoilStatusCore
{
    public override eSoilStatus StatusFlag => eSoilStatus.Withered;

    public override eAction SupportAction => eAction.Eradicate;

    protected override float AutoEnterNextStatusTime => 0;

    protected override void OnExecuteHomeAction(eAction action, long playerId, long entityId, object actionData)
    {
        base.OnExecuteHomeAction(action, playerId, entityId, actionData);

        SoilData.ClearSeedData();
        ChangeState(eSoilStatus.Idle);
    }
}