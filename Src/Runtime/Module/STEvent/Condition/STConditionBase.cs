/*
 * @Author: xiang huan
 * @Date: 2024-05-24 13:56:20
 * @Description: 场景事件条件基类, 用了引用池记得清理数据！
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/STEvent/Condition/STConditionBase.cs
 * 
 */

using System;
using GameFramework;
using GameMessageCore;
public class STConditionBase : IReference
{
    /// <summary>
    /// 场景事件
    /// </summary>
    public SceneTriggerEvent SceneEvent;
    /// <summary>
    /// 场景事件条件配置
    /// </summary>
    public DRSceneEventCondition DRSceneEventCondition;
    /// <summary>
    /// 场景事件条件类型
    /// </summary>
    public eSTConditionType Type { get; private set; }

    protected SceneEventConditionData NetData = new();

    public virtual void Init(DRSceneEventCondition cfg, SceneTriggerEvent sceneEvent)
    {
        DRSceneEventCondition = cfg;
        SceneEvent = sceneEvent;
        Type = (eSTConditionType)cfg.Type;
        OnAddEvent();
    }
    public virtual bool Check()
    {
        return true;
    }

    protected virtual void OnAddEvent()
    {

    }

    protected virtual void OnRemoveEvent()
    {

    }

    public virtual void Clear()
    {
        Type = eSTConditionType.None;
        DRSceneEventCondition = null;
        SceneEvent = null;
        OnRemoveEvent();
    }

    public void Dispose()
    {
        ReferencePool.Release(this);
    }

    public static STConditionBase Create(Type stClass)
    {
        STConditionBase conditionBase = ReferencePool.Acquire(stClass) as STConditionBase;
        return conditionBase;
    }

    public virtual void SyncNetData(SceneEventConditionData conditionData)
    {
        NetData = conditionData;
    }
    public virtual SceneEventConditionData GetNetData()
    {
        NetData.Cid = DRSceneEventCondition.Id;
        return NetData;
    }
}