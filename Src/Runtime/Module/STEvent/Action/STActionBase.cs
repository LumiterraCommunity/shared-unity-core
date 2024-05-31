/*
 * @Author: xiang huan
 * @Date: 2024-05-24 13:56:20
 * @Description: 场景事件行为基类, 用了引用池记得清理数据！
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/STEvent/Action/STActionBase.cs
 * 
 */

using System;
using GameFramework;
public class STActionBase : IReference
{
    /// <summary>
    /// 场景事件
    /// </summary>
    public SceneTriggerEvent SceneEvent { get; private set; }
    /// <summary>
    /// 场景事件行为配置
    /// </summary> 
    public DRSceneEventAction DRSceneEventAction;
    /// <summary>
    /// 场景事件行为类型
    /// </summary>
    public eSTActionType Type { get; private set; }

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Init(DRSceneEventAction cfg, SceneTriggerEvent sceneEvent)
    {
        DRSceneEventAction = cfg;
        SceneEvent = sceneEvent;
        Type = (eSTActionType)cfg.Type;
        OnAddEvent();
    }

    /// <summary>
    /// 执行行为
    /// </summary>
    public virtual void Execute()
    {

    }

    protected virtual void OnAddEvent()
    {

    }
    protected virtual void OnRemoveEvent()
    {

    }

    public virtual void Clear()
    {
        Type = eSTActionType.None;
        DRSceneEventAction = null;
        SceneEvent = null;
        OnRemoveEvent();
    }

    public void Dispose()
    {
        ReferencePool.Release(this);
    }

    public static STActionBase Create(Type stClass)
    {
        STActionBase actionBase = ReferencePool.Acquire(stClass) as STActionBase;
        return actionBase;
    }
}