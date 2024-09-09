using System;
using GameFramework.Fsm;
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

    protected override void OnEnter(IFsm<SoilStatusCtrl> fsm)
    {
        base.OnEnter(fsm);

        // 不能这样清除 可能是初始化过来的状态 会先设置好数据再跳转的状态  不过将来可以考虑使用初始化标记位来参考 目前先不乱动
        // SoilData.ClearSeedData();
    }

    protected override void OnAutoEnterNextStatus()
    {
        base.OnAutoEnterNextStatus();

        ChangeState(eSoilStatus.Idle);
    }
    protected override void OnExecuteHomeAction(eAction action, long playerId, long entityId, object actionData)
    {
        base.OnExecuteHomeAction(action, playerId, entityId, actionData);

        try
        {
            if (action == eAction.Sowing)
            {
                GameMessageCore.SowingResult sowingData = actionData as GameMessageCore.SowingResult;
                SoilData.PutSeed(sowingData.SeedCid, sowingData.SeedNftId, sowingData.SeedEntityId, sowingData.SeedLv, sowingData.SeedPotentiality);
                ChangeState(eSoilStatus.SeedThirsty);
            }
            else if (action == eAction.Eradicate)
            {
                ChangeState(eSoilStatus.Idle);
            }
        }
        catch (System.Exception e)
        {
            Log.Error($"松土上动作错误 action:{action} actionData:{JsonConvert.SerializeObject(actionData)} error:{e}");
            throw e;
        }
    }
}