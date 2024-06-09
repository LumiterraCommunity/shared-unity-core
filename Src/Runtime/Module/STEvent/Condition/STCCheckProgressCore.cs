/*
 * @Author: xiang huan
 * @Date: 2024-05-24 13:56:20
 * @Description: 检测关卡分进度
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/STEvent/Condition/STCCheckProgressCore.cs
 * 
 */

using GameMessageCore;
using UnityEngine;
using UnityGameFramework.Runtime;
public class STCCheckProgressCore : STConditionBase
{

    public float CurValue;
    public float TargetValue;
    protected eSTConditionComparison Operation = eSTConditionComparison.Equal;
    public override void Init(DRSceneEventCondition cfg, SceneTriggerEvent sceneEvent)
    {
        base.Init(cfg, sceneEvent);
        NetData.ProgressValue = new();
    }
    public override void Clear()
    {
        CurValue = 0;
        TargetValue = 0;
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

    public bool CheckValue(eSTConditionComparison operation, float curValue, float value)
    {
        switch (operation)
        {
            case eSTConditionComparison.Equal:
                return Mathf.Approximately(curValue, value);
            case eSTConditionComparison.NotEqual:
                return !Mathf.Approximately(curValue, value);
            case eSTConditionComparison.Greater:
                return curValue > value;
            case eSTConditionComparison.GreaterEqual:
                return curValue >= value;
            case eSTConditionComparison.Less:
                return curValue < value;
            case eSTConditionComparison.LessEqual:
                return curValue <= value;
            default:
                break;
        }
        return false;
    }

}