using System;
using UnityGameFramework.Runtime;
using static HomeDefine;

/// <summary>
/// 土地等待收割状态 只有普通种子才能收割
/// </summary>
public class SoilHarvestStatusCore : SoilStatusCore
{
    public override eSoilStatus StatusFlag => eSoilStatus.Harvest;

    public override eAction SupportAction => eAction.Harvest;

    protected override float AutoEnterNextStatusTime => 0;
    protected override void OnExecuteHomeAction(eAction action, long playerId, long entityId, object actionData)
    {
        base.OnExecuteHomeAction(action, playerId, entityId, actionData);

        try
        {
            StatusCtrl.HomeSoilCore.OnHarvest(playerId, entityId);
        }
        catch (Exception e)
        {
            Log.Error($"土地收割时异常,id:{StatusCtrl.HomeSoilCore.Id} error:{e}");
        }

        SoilData.ClearSeedData();
        ChangeState(eSoilStatus.Idle);
    }
}