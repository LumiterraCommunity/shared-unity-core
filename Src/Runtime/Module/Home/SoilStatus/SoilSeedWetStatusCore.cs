using Newtonsoft.Json;
using UnityGameFramework.Runtime;
using static HomeDefine;

/// <summary>
/// 土地播种已湿润状态
/// </summary>
public class SoilSeedWetStatusCore : SoilGrowingWetStatusCore
{
    public override eSoilStatus StatusFlag => eSoilStatus.SeedWet;

    public override eAction SupportAction
    {
        get
        {
            eAction res = base.SupportAction;
            if (SoilData.SaveData.SeedData.ManureCid <= 0)//如果没有施过肥可以施肥
            {
                res |= eAction.Manure;
            }
            return res;
        }
    }

    protected override void OnTryChangeWaterStatus(bool isWatering)
    {
        if (!isWatering)
        {
            SoilExternalControl ctrl = StatusCtrl.GetComponent<SoilExternalControl>();
            ctrl.ChangeWaterData(false);

            ChangeState(eSoilStatus.SeedThirsty);
        }
    }

    protected override void OnExecuteHomeAction(eAction action, object actionData)
    {
        base.OnExecuteHomeAction(action, actionData);

        if (action == eAction.Manure)
        {
            try
            {
                int manureCid = (int)actionData;
                SoilData.SetManure(manureCid);
            }
            catch (System.Exception e)
            {
                Log.Error($"播种湿润时施肥失败 actionData:{JsonConvert.SerializeObject(actionData)} error:{e}");
                throw e;
            }
        }
    }
}