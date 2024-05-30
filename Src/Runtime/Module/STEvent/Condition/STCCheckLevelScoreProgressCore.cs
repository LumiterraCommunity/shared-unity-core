/*
 * @Author: xiang huan
 * @Date: 2024-05-24 13:56:20
 * @Description: 检测关卡分进度
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/STEvent/Condition/STCCheckLevelScoreProgressCore.cs
 * 
 */

using GameMessageCore;
using UnityGameFramework.Runtime;
/// <summary>
/// 检测关卡分进度
/// DRSceneEventCondition.Parameters_1: [0]检测类型 [1]比较操作 [2]比较值
/// </summary>//  
public class STCCheckLevelScoreProgressCore : STConditionBase
{

    protected float CurValue;
    protected float TargetValue;

    protected eSTCCheckLevelScoreType CheckType = eSTCCheckLevelScoreType.Normal;
    protected eSTConditionComparison Operation = eSTConditionComparison.Equal;
    public override void Init(DRSceneEventCondition cfg)
    {
        base.Init(cfg);
        NetData.ProgressValue = new();
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
        CurValue = 0;
        TargetValue = 0;
        CheckType = eSTCCheckLevelScoreType.Normal;
        Operation = eSTConditionComparison.Equal;
        base.Clear();
    }

    public override SceneEventConditionData GetNetData()
    {
        NetData.ProgressValue.CurValue = (int)(CurValue * MathUtilCore.T2I);
        NetData.ProgressValue.TargetValue = (int)(TargetValue * MathUtilCore.T2I);
        return NetData;
    }

    public override void SyncNetData(SceneEventConditionData conditionData)
    {
        base.SyncNetData(conditionData);
        CurValue = NetData.ProgressValue.CurValue * MathUtilCore.I2T;
        TargetValue = NetData.ProgressValue.TargetValue * MathUtilCore.I2T;
    }

}