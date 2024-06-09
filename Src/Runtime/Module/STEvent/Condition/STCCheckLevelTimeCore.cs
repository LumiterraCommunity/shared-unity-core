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
/// DRSceneEventCondition.Parameters_1: [0]检测类型 [1]比较操作 [2]比较值
/// </summary>//  
public class STCCheckLevelTimeCore : STCCheckProgressCore
{
    protected eSTCCheckLevelScoreType CheckType = eSTCCheckLevelScoreType.Normal;
    protected IInstancingMgr InstancingMgr;

    protected SceneAreaMgrCore SceneAreaMgrCore;
    public override void Init(DRSceneEventCondition cfg, SceneTriggerEvent sceneEvent)
    {
        base.Init(cfg, sceneEvent);
        if (DRSceneEventCondition.Parameters_1.Length < 2)
        {
            Log.Error("STCCheckLevelTimeCore Check Error: Parameters_1.Length < 2");
            return;
        }

        CheckType = (eSTCCheckLevelScoreType)DRSceneEventCondition.Parameters_1[0];
        Operation = (eSTConditionComparison)DRSceneEventCondition.Parameters_1[1];
        TargetValue = DRSceneEventCondition.Parameters_1[2] * MathUtilCore.PM;
        InstancingMgr = GFEntryCore.GetModule<IInstancingMgr>();
        SceneAreaMgrCore = GFEntryCore.GetModule<SceneAreaMgrCore>();
    }
    public override bool Check()
    {
        int levelTime = (int)((TimeUtil.GetServerTimeStamp() - InstancingMgr.CurLevelStartTime) * TimeUtil.MS2S);
        float timeLimit = TableUtil.GetInstancingLevelTimeLimit(SceneAreaMgrCore.DefaultDRSceneArea, InstancingMgr.CurLevelIndex);
        if (CheckType == eSTCCheckLevelScoreType.Normal)
        {
            CurValue = (int)((TimeUtil.GetServerTimeStamp() - InstancingMgr.CurLevelStartTime) * TimeUtil.MS2S);
        }
        else if (CheckType == eSTCCheckLevelScoreType.Percent)
        {
            CurValue = levelTime / timeLimit;
        }
        return CheckValue(Operation, CurValue, TargetValue);
    }

    public override void Clear()
    {
        base.Clear();
    }
}