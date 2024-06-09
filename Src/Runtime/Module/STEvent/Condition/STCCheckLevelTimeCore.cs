/*
 * @Author: xiang huan
 * @Date: 2024-05-24 13:56:20
 * @Description: 检测关卡时间
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/STEvent/Condition/STCCheckLevelTimeCore.cs
 * 
 */

using UnityGameFramework.Runtime;
/// <summary>
/// 检测关卡分时间
/// DRSceneEventCondition.Parameters_1: [0]比较操作 [1]比较值
/// </summary>//  
public class STCCheckLevelTimeCore : STCCheckProgressCore
{
    protected IInstancingMgr InstancingMgr;
    public override void Init(DRSceneEventCondition cfg, SceneTriggerEvent sceneEvent)
    {
        base.Init(cfg, sceneEvent);
        if (DRSceneEventCondition.Parameters_1.Length < 2)
        {
            Log.Error("STCCheckLevelTimeCore Check Error: Parameters_1.Length < 2");
            return;
        }
        Operation = (eSTConditionComparison)DRSceneEventCondition.Parameters_1[0];
        TargetValue = DRSceneEventCondition.Parameters_1[1];
        InstancingMgr = GFEntryCore.GetModule<IInstancingMgr>();
    }
    public override bool Check()
    {
        CurValue = (int)((TimeUtil.GetServerTimeStamp() - InstancingMgr.CurLevelStartTime) * TimeUtil.MS2S);
        return CheckValue(Operation, CurValue, TargetValue);
    }

    public override void Clear()
    {
        base.Clear();
    }
}