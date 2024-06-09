using static HomeDefine;
using GameFramework.Fsm;
using Newtonsoft.Json;
using UnityGameFramework.Runtime;
using System;

/// <summary>
/// 土地种子发苗后的生长干涸状态
/// </summary>
public class SoilGrowingThirstyStatusCore : SoilStatusCore
{
    public override eSoilStatus StatusFlag => eSoilStatus.GrowingThirsty;

    public override eAction SupportAction => eAction.Watering | (HomeUtilCore.JudgeSeedCanDestroy(SoilData) ? eAction.Eradicate : eAction.None);

    protected override float AutoEnterNextStatusTime => SoilData.GetAttribute(eAttributeType.WitherTime);

    protected override void OnEnter(IFsm<SoilStatusCtrl> fsm)
    {
        base.OnEnter(fsm);

        StatusCtrl.SoilEvent.TryChangeGrowStage += OnTryChangeGrowStage;
        StatusCtrl.SoilEvent.TryChangeWaterStatus += OnTryChangeWaterStatus;
        StatusCtrl.SoilEvent.OnAttributeUpdated += OnAttributeUpdated;
        UpdateNeedWaterProgress();
    }

    protected override void OnLeave(IFsm<SoilStatusCtrl> fsm, bool isShutdown)
    {
        StatusCtrl.SoilEvent.TryChangeGrowStage -= OnTryChangeGrowStage;
        StatusCtrl.SoilEvent.TryChangeWaterStatus -= OnTryChangeWaterStatus;
        StatusCtrl.SoilEvent.OnAttributeUpdated -= OnAttributeUpdated;

        StatusCtrl.GetComponent<HomeActionProgressData>().EndProgressAction();

        base.OnLeave(fsm, isShutdown);
    }


    private void UpdateNeedWaterProgress()
    {
        StatusCtrl.GetComponent<HomeActionProgressData>().StartProgressAction(eAction.Watering, SoilData.DRSeed != null ? SoilData.GetAttribute(eAttributeType.NeedWaterValue) : ACTION_MAX_PROGRESS_PROTECT);
    }

    private void OnAttributeUpdated(eAttributeType type, int value)
    {
        if (type == eAttributeType.NeedWaterValue)
        {
            UpdateNeedWaterProgress();
        }
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
            ChangeState(eSoilStatus.SeedThirsty);
        }
        else
        {
            ChangeState(eSoilStatus.GrowingThirsty);
        }
    }

    private void OnTryChangeWaterStatus(bool isWatering)
    {
        if (isWatering)
        {
            SoilExternalControl ctrl = StatusCtrl.GetComponent<SoilExternalControl>();
            if (ctrl.JudgeExternalCanWatering())
            {
                ctrl.ChangeWaterData(true);

                ChangeState(eSoilStatus.GrowingWet);
            }
        }
    }

    protected override void OnEnterInitStatus(IFsm<SoilStatusCtrl> fsm)
    {
        //如果有额外次数就不用进初始状态去找下一个状态 直接等update时进入下一个生长状态 并且保留初始时进入的初始标记和时间
        if (SoilData.SaveData.SeedData.ExtraWateringNum <= 0)
        {
            base.OnEnterInitStatus(fsm);
        }
    }

    protected override void OnUpdate(IFsm<SoilStatusCtrl> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

        if (SoilData == null)
        {
            return;
        }

        //自动进入湿润状态
        SoilExternalControl ctrl = StatusCtrl.GetComponent<SoilExternalControl>();
        if (SoilData.SaveData.SeedData.ExtraWateringNum > 0 && ctrl.JudgeExternalCanWatering())
        {
            SoilData.SaveData.SeedData.ExtraWateringNum--;

            ctrl.ChangeWaterData(true);

            ChangeState(eSoilStatus.GrowingWet);
        }
    }

    protected override void OnAutoEnterNextStatus()
    {
        base.OnAutoEnterNextStatus();

        ChangeState(eSoilStatus.Withered);
    }

    protected override void OnExecuteHomeAction(eAction action, object actionData)
    {
        base.OnExecuteHomeAction(action, actionData);

        if (action == eAction.Eradicate)
        {
            SoilData.ClearSeedData();
            ChangeState(eSoilStatus.Loose);
        }
        else if (action == eAction.Watering)
        {
            try
            {
                GameMessageCore.WateringResult wateringResult = (GameMessageCore.WateringResult)actionData;
                int extraWateringNum = wateringResult.ExtraWateringNum;
                if (extraWateringNum > 0)
                {
                    SoilData.SaveData.SeedData.ExtraWateringNum = extraWateringNum;
                }
                SoilData.SetCurProficiency(wateringResult.CurProficiency);
                SoilData.SetNeedPerish(wateringResult.NeedPerish);
                if (wateringResult.NeedPerish)
                {
                    Log.Info($"在生长干涸浇水时种子被标记成腐败收获 id={SoilData.SaveData.Id} cid={SoilData.SaveData.SeedData.SeedCid} curStage={SoilData.SaveData.SeedData.GrowingStage} maxStage={SoilData.SeedGrowStageNum - 1}");
                }
            }
            catch (System.Exception e)
            {
                Log.Error($"生长干涸时浇水有错误 actionData:{JsonConvert.SerializeObject(actionData)} error:{e}");
            }

            ChangeState(eSoilStatus.GrowingWet);
        }
    }
}