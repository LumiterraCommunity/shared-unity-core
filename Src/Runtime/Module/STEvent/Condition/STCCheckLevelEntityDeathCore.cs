/*
 * @Author: xiang huan
 * @Date: 2024-05-24 13:56:20
 * @Description: 检测关卡实体死亡
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/STEvent/Condition/STCCheckLevelEntityDeathCore.cs
 * 
 */

using UnityGameFramework.Runtime;
/// <summary>
/// 检测关卡实体死亡
/// DRSceneEventCondition.Parameters_1:[0]实体类型 [1]配置ID [2]比较操作 [3]比较值
/// </summary>//  
public class STCCheckLevelEntityDeathCore : STCCheckProgressCore
{
    public GameMessageCore.EntityType EntityType { get; private set; }
    public int ConfigId { get; private set; }
    public override void Init(DRSceneEventCondition cfg, SceneTriggerEvent sceneEvent)
    {
        base.Init(cfg, sceneEvent);
        if (DRSceneEventCondition.Parameters_1.Length < 4)
        {
            Log.Error("STCCheckLevelEntityDeathCore Check Error: Parameters_1.Length < 2");
            return;
        }
        EntityType = (GameMessageCore.EntityType)DRSceneEventCondition.Parameters_1[0];
        ConfigId = DRSceneEventCondition.Parameters_1[1];
        Operation = (eSTConditionComparison)DRSceneEventCondition.Parameters_1[2];
        TargetValue = DRSceneEventCondition.Parameters_1[3];
    }

    public override bool Check()
    {
        return CheckValue(Operation, CurValue, TargetValue);
    }

    public override void Clear()
    {
        EntityType = GameMessageCore.EntityType.All;
        ConfigId = 0;
        base.Clear();
    }
}