/*
 * @Author: xiang huan
 * @Date: 2024-05-24 13:42:59
 * @Description: 场景触发器事件管理器
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/STEvent/SceneTriggerEventMgrCore.cs
 * 
 */

using GameMessageCore;
using UnityEngine;
public class SceneTriggerEventMgrCore : MonoBehaviour
{
    public ListMap<long, SceneTriggerEvent> SceneTriggerEvents = new(); //场景触发器事件列表
    private long _idIndex = 0; //事件ID索引

    /// <summary>
    /// 添加场景触发器事件
    /// </summary>
    /// <param name="cid">触发器ID</param> 
    public SceneTriggerEvent AddSceneTriggerEvent(int cid)
    {
        long idIndex = _idIndex++;
        SceneTriggerEvent sceneEvent = SceneTriggerEvent.Create(typeof(SceneTriggerEvent));
        sceneEvent.Init(idIndex, cid);
        _ = SceneTriggerEvents.Add(idIndex, sceneEvent);
        return sceneEvent;
    }

    /// <summary>
    /// 同步场景触发器事件
    /// </summary>
    /// <param name="cid">触发器ID</param> 
    public SceneTriggerEvent SyncSceneTriggerEvent(SceneEventData sceneEventData)
    {
        if (!SceneTriggerEvents.TryGetValueFromKey(sceneEventData.Id, out SceneTriggerEvent sceneEvent))
        {
            sceneEvent = SceneTriggerEvent.Create(typeof(SceneTriggerEvent));
            sceneEvent.Init(sceneEventData.Id, sceneEventData.EventCid);
            _ = SceneTriggerEvents.Add(sceneEventData.Id, sceneEvent);
        }
        sceneEvent.SyncNetData(sceneEventData);
        return sceneEvent;
    }

    /// <summary>
    /// 移除场景触发器事件
    /// </summary>
    /// <param name="sceneEvent">场景触发器事件</param>
    public void RemoveSceneTriggerEvent(long id)
    {
        if (SceneTriggerEvents == null || SceneTriggerEvents.Count == 0)
        {
            return;
        }
        if (SceneTriggerEvents.TryGetValueFromKey(id, out SceneTriggerEvent value))
        {
            value.Dispose();
            _ = SceneTriggerEvents.Remove(id);
        }
    }

    public void ClearSceneTrigger()
    {
        if (SceneTriggerEvents == null || SceneTriggerEvents.Count == 0)
        {
            return;
        }
        for (int i = 0; i < SceneTriggerEvents.Count; i++)
        {
            SceneTriggerEvents[i].Dispose();
        }
        SceneTriggerEvents.Clear();
    }

    private void OnDestroy()
    {
        ClearSceneTrigger();
    }
    /// <summary>
    /// 广播场景触发器事件数据
    /// </summary>
    public virtual void BroadcastSceneTriggerEventUpdate(SceneEventData data)
    {

    }
    /// <summary>
    /// 广播场景触发器事件数据
    /// </summary>
    public virtual void BroadcastSceneTriggerEventActionExecute(int actionCid)
    {

    }

    public virtual STConditionBase CreateSTCondition(int cid)
    {
        return STConditionFactoryCore.Inst.CreateSTCondition(cid);
    }

    public virtual STActionBase CreateSTAction(int cid)
    {
        return STActionFactoryCore.Inst.CreateSTAction(cid);
    }
}
