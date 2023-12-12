using Newtonsoft.Json;
using UnityGameFramework.Runtime;
using static HomeDefine;

/// <summary>
/// 土地松土状态
/// </summary>
public class SoilLooseStatusCore : SoilStatusCore
{
    public override eSoilStatus StatusFlag => eSoilStatus.Loose;

    public override eAction SupportAction => eAction.Sowing | eAction.Eradicate;

    protected override float AutoEnterNextStatusTime => TableUtil.GetGameValue(eGameValueID.soilFromLooseToIdleTime).Value;

    protected override void OnAutoEnterNextStatus()
    {
        base.OnAutoEnterNextStatus();

        SoilData.SetSoilFertile(0);
        ChangeState(eSoilStatus.Idle);
    }
    protected override void OnExecuteHomeAction(eAction action, object actionData)
    {
        base.OnExecuteHomeAction(action, actionData);

        try
        {
            if (action == eAction.Sowing)
            {
                int seedCid = (int)actionData;
                SoilData.SetSeedCid(seedCid);
                ChangeState(eSoilStatus.SeedThirsty);
            }
            else if (action == eAction.Eradicate)
            {
                SoilData.SetSoilFertile(0);
                ChangeState(eSoilStatus.Idle);
            }
        }
        catch (System.Exception e)
        {
            Log.Error($"松土上动作错误 action:{action} actionData:{JsonConvert.SerializeObject(actionData)} error:{e}");
        }
    }
}