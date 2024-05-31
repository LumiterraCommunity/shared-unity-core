using System;
using GameFramework.Fsm;
using static HomeDefine;

/// <summary>
/// 土地生长湿润状态
/// </summary>
public class SoilGrowingWetStatusCore : SoilStatusCore
{
    public override eSoilStatus StatusFlag => eSoilStatus.GrowingWet;

    public override eAction SupportAction => HomeUtilCore.JudgeSeedCanDestroy(SoilData) ? eAction.Eradicate : eAction.None;

    protected override float AutoEnterNextStatusTime => SoilData.SeedEveryGrowStageTime;

    protected override void OnAutoEnterNextStatus()
    {
        base.OnAutoEnterNextStatus();

        int growStage = SoilData.SaveData.SeedData.GrowingStage;
        if (growStage >= SoilData.SeedGrowStageNum - 1)//成熟了
        {
            ChangeToRipe();
        }
        else
        {
            SoilData.SetGrowStage(growStage + 1);
            ChangeState(eSoilStatus.GrowingThirsty);
        }
    }

    protected override void OnEnter(IFsm<SoilStatusCtrl> fsm)
    {
        base.OnEnter(fsm);

        StatusCtrl.SoilEvent.TryChangeGrowStage += OnTryChangeGrowStage;
        StatusCtrl.SoilEvent.TryChangeWaterStatus += OnTryChangeWaterStatus;
    }

    protected override void OnLeave(IFsm<SoilStatusCtrl> fsm, bool isShutdown)
    {
        StatusCtrl.SoilEvent.TryChangeGrowStage -= OnTryChangeGrowStage;
        StatusCtrl.SoilEvent.TryChangeWaterStatus -= OnTryChangeWaterStatus;

        base.OnLeave(fsm, isShutdown);
    }

    private void OnTryChangeGrowStage(int offsetStage)
    {
        SoilExternalControl ctrl = StatusCtrl.GetComponent<SoilExternalControl>();
        if (!ctrl.ChangeGrowStage(offsetStage, out int newStage))
        {
            return;
        }

        if (newStage == 0)
        {
            ChangeState(eSoilStatus.SeedWet);
        }
        else
        {
            ChangeState(eSoilStatus.GrowingWet);
        }
    }

    protected virtual void OnTryChangeWaterStatus(bool isWatering)
    {
        if (!isWatering)
        {
            SoilExternalControl ctrl = StatusCtrl.GetComponent<SoilExternalControl>();
            ctrl.ChangeWaterData(false);

            ChangeState(eSoilStatus.GrowingThirsty);
        }
    }

    private void ChangeToRipe()
    {
        if (SoilData.SaveData.SeedData.NeedPerish)
        {
            ChangeState(eSoilStatus.RipePerish);
        }
        else
        {
            ChangeState(eSoilStatus.Ripe);
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