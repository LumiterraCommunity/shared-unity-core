using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;


/// <summary>
/// 作物行为事件执行
/// DRSceneEventAction.Parameters_2: [[行为类型,行为参数,行为参数]...]
/// </summary>
public class STASoilBehaviorCore : STActionBase
{
    public override void Init(DRSceneEventAction cfg, SceneTriggerEvent sceneEvent)
    {
        base.Init(cfg, sceneEvent);
    }

    public override void Execute()
    {
        base.Execute();

        if (HomeModuleCore.IsInited == false)
        {
            Log.Error($"STASoilBehaviorCore Execute HomeModuleCore.IsInited == false,eventId:{SceneEvent.Id}");
            return;
        }

        IEnumerable<HomeSoilCore> soils = HomeModuleCore.SoilMgr.GetAllSoil();

        foreach (int[] item in DRSceneEventAction.Parameters_2)
        {
            try
            {
                eSoilBehaviorType behaviorType = (eSoilBehaviorType)item[0];
                switch (behaviorType)
                {
                    case eSoilBehaviorType.ModifyGrowStage:// 修改生长阶段
                        {
                            int offset = item[1];
                            BatchForeachSoil(soils, (soil) =>
                            {
                                ExecuteModifyGrowStage(soil, offset);
                            });
                            break;
                        }
                    case eSoilBehaviorType.Watering:// 浇水
                        {
                            bool isWatering = item[1] != 0;
                            BatchForeachSoil(soils, (soil) =>
                            {
                                ExecuteWatering(soil, isWatering);
                            });
                            break;
                        }

                    case eSoilBehaviorType.None:
                    default:
                        break;
                }
            }
            catch (System.Exception e)
            {
                Log.Error($"STASoilBehaviorCore Execute type error = {e}");
                continue;
            }
        }
    }

    private void BatchForeachSoil(IEnumerable<HomeSoilCore> soils, Action<HomeSoilCore> cb)
    {
        foreach (HomeSoilCore soil in soils)
        {
            try
            {
                cb(soil);
            }
            catch (System.Exception e)
            {
                Log.Error($"STASoilBehaviorCore BatchForeachSoil error = {e}");
                continue;
            }
        }
    }

    public override void Clear()
    {
        base.Clear();
    }

    /// <summary>
    /// 行为类型
    /// </summary>
    private enum eSoilBehaviorType
    {
        None = 0,
        ModifyGrowStage = 1, // 种植 参数:生长阶段offset
        Watering = 2, // 浇水 参数：是否浇水
    }

    //执行浇水
    private void ExecuteWatering(HomeSoilCore soil, bool isWatering)
    {
        soil.SoilEvent.TryChangeWaterStatus(isWatering);
    }

    //执行修改生长阶段
    private void ExecuteModifyGrowStage(HomeSoilCore soil, int offset)
    {
        soil.SoilEvent.TryChangeGrowStage(offset);
    }
}