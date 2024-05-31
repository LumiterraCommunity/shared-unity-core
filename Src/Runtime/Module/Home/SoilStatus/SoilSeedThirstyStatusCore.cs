using Newtonsoft.Json;
using UnityGameFramework.Runtime;
using static HomeDefine;
using GameFramework.Fsm;

/// <summary>
/// 土地已播种干涸状态
/// </summary>
public class SoilSeedThirstyStatusCore : SoilStatusCore
{
    public override eSoilStatus StatusFlag => eSoilStatus.SeedThirsty;

    public override eAction SupportAction
    {
        get
        {
            eAction res = eAction.Watering | (HomeUtilCore.JudgeSeedCanDestroy(SoilData) ? eAction.Eradicate : eAction.None);
            if (SoilData.SaveData.SeedData.ManureCid <= 0)//如果没有施过肥可以施肥
            {
                res |= eAction.Manure;
            }
            return res;
        }
    }

    protected override float AutoEnterNextStatusTime => 0;

    protected override void OnEnter(IFsm<SoilStatusCtrl> fsm)
    {
        base.OnEnter(fsm);

        StatusCtrl.GetComponent<HomeActionProgressData>().StartProgressAction(eAction.Watering, SoilData.DRSeed != null ? SoilData.GetAttribute(eAttributeType.NeedWaterValue) : ACTION_MAX_PROGRESS_PROTECT);
    }

    protected override void OnLeave(IFsm<SoilStatusCtrl> fsm, bool isShutdown)
    {
        StatusCtrl.GetComponent<HomeActionProgressData>().EndProgressAction();

        base.OnLeave(fsm, isShutdown);
    }

    protected override void OnExecuteHomeAction(eAction action, object actionData)
    {
        base.OnExecuteHomeAction(action, actionData);

        if (action == eAction.Watering)
        {
            try
            {
                GameMessageCore.WateringResult wateringResult = (GameMessageCore.WateringResult)actionData;
                int extraWateringNum = wateringResult.ExtraWateringNum;
                if (extraWateringNum > 0)
                {
                    SoilData.SaveData.SeedData.ExtraWateringNum = extraWateringNum;
                }
                SoilData.SaveData.SeedData.CurProficiency = wateringResult.CurProficiency;
                SoilData.SaveData.SeedData.NeedPerish = wateringResult.NeedPerish;
                if (wateringResult.NeedPerish)
                {
                    Log.Error($"在播种干涸浇水时种子被标记成腐败收获 id={SoilData.SaveData.Id} cid={SoilData.SaveData.SeedData.SeedCid} curStage={SoilData.SaveData.SeedData.GrowingStage} maxStage={SoilData.SeedGrowStageNum - 1}");//打错误是因为理论上不可能出现 除非配置只有一个生长阶段
                }
            }
            catch (System.Exception e)
            {
                Log.Error($"种子干涸时浇水有错误 actionData:{JsonConvert.SerializeObject(actionData)} error:{e}");
            }

            ChangeState(eSoilStatus.SeedWet);
        }
        else if (action == eAction.Eradicate)
        {
            SoilData.ClearSeedData();
            ChangeState(eSoilStatus.Loose);
        }
        else if (action == eAction.Manure)
        {
            try
            {
                int manureCid = (int)actionData;
                SoilData.SetManure(manureCid);
            }
            catch (System.Exception e)
            {
                Log.Error($"播种干涸时施肥失败 actionData:{JsonConvert.SerializeObject(actionData)} error:{e}");
            }
        }
    }
}