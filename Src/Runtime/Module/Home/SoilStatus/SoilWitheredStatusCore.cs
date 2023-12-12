using static HomeDefine;

/// <summary>
/// 土地植物干枯状态
/// </summary>
public class SoilWitheredStatusCore : SoilStatusCore
{
    public override eSoilStatus StatusFlag => eSoilStatus.Withered;

    public override eAction SupportAction => eAction.Eradicate;

    protected override float AutoEnterNextStatusTime => 0;

    protected override void OnExecuteHomeAction(eAction action, object actionData)
    {
        base.OnExecuteHomeAction(action, actionData);

        SoilData.ClearSeedData();
        SoilData.SetSoilFertile(0);
        ChangeState(eSoilStatus.Idle);
    }
}