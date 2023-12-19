using System;
using static HomeDefine;

/// <summary>
/// 土地生长湿润状态
/// </summary>
public class SoilGrowingWetStatusCore : SoilStatusCore
{
    public override eSoilStatus StatusFlag => eSoilStatus.GrowingWet;

    public override eAction SupportAction => eAction.Eradicate;

    protected override float AutoEnterNextStatusTime => SoilData.SeedEveryGrowStageTime;

    protected override void OnAutoEnterNextStatus()
    {
        base.OnAutoEnterNextStatus();

        int growStage = SoilData.SaveData.SeedData.GrowingStage;
        if (growStage >= SoilData.SeedGrowStageNum - 1)//成熟了
        {
            ChangeToHarvest();
        }
        else
        {
            SoilData.SetGrowStage(growStage + 1);
            ChangeState(eSoilStatus.GrowingThirsty);
        }
    }

    private void ChangeToHarvest()
    {
        if (SoilData.SaveData.SeedData.NeedPerish)
        {
            ChangeState(eSoilStatus.HarvestPerish);
        }
        else
        {
            ChangeState(eSoilStatus.Harvest);
        }
    }

    protected override void OnExecuteHomeAction(eAction action, object actionData)
    {
        base.OnExecuteHomeAction(action, actionData);

        if (action == eAction.Eradicate)
        {
            SoilData.ClearSeedData();
            ChangeState(eSoilStatus.Loose);
        }
    }
}