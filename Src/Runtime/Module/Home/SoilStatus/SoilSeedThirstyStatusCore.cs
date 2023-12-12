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
            eAction res = eAction.Watering | eAction.Eradicate;
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

        StatusCtrl.GetComponent<HomeActionProgressData>().StartProgressAction(eAction.Watering, SoilData.DRSeed != null ? SoilData.DRSeed.NeedWaterValue : ACTION_MAX_PROGRESS_PROTECT);
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
                int extraWateringNum = (int)actionData;
                if (extraWateringNum > 0)
                {
                    SoilData.SaveData.SeedData.ExtraWateringNum = extraWateringNum;
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