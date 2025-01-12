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

        if (ctrl.JudgeExternalCanWatering())//能进入湿润状态
        {
            //设置初始化标记不会重置进入当前状态时间戳 会持续保持当前生长倒计时 类似让土地初始化到在这个状态 而不是正常跳转重新计时
            OwnerFsm.SetData<UnityGameFramework.Runtime.VarBoolean>(SoilStatusDataName.IS_INIT_STATUS, true);
            if (newStage == 0)
            {
                ChangeState(eSoilStatus.SeedWet);
            }
            else
            {
                ChangeState(eSoilStatus.GrowingWet);
            }
        }
        else//不能进入湿润状态
        {
            if (newStage == 0)
            {
                ChangeState(eSoilStatus.SeedThirsty);
            }
            else
            {
                ChangeState(eSoilStatus.GrowingThirsty);
            }
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

    protected override void OnExecuteHomeAction(eAction action, long playerId, long entityId, object actionData)
    {
        base.OnExecuteHomeAction(action, playerId, entityId, actionData);

        if (action == eAction.Eradicate)
        {
            SoilData.ClearSeedData();
            ChangeState(eSoilStatus.Loose);
        }
    }
}