/*
 * @Author: xiang huan
 * @Date: 2024-05-24 13:56:20
 * @Description: 检测关卡分进度
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/STEvent/Condition/STCCheckLevelScoreProgressCore.cs
 * 
 */

using UnityGameFramework.Runtime;
/// <summary>
/// 检测关卡分进度
/// DRSceneEventCondition.Parameters_1: [0]检测类型 [1]比较操作 [2]比较值
/// </summary>//  
public class STCCheckLevelScoreProgressCore : STCCheckProgressCore
{
    protected eSTCCheckLevelScoreType CheckType = eSTCCheckLevelScoreType.Normal;
    public override void Init(DRSceneEventCondition cfg, SceneTriggerEvent sceneEvent)
    {
        base.Init(cfg, sceneEvent);
        if (DRSceneEventCondition.Parameters_1.Length < 3)
        {
            Log.Error("STCCheckLevelScore Check Error: Parameters_1.Length < 3");
            return;
        }
        CheckType = (eSTCCheckLevelScoreType)DRSceneEventCondition.Parameters_1[0];
        Operation = (eSTConditionComparison)DRSceneEventCondition.Parameters_1[1];
        TargetValue = DRSceneEventCondition.Parameters_1[2] * MathUtilCore.PM;
    }
    public override void Clear()
    {
        CheckType = eSTCCheckLevelScoreType.Normal;
        base.Clear();
    }

}