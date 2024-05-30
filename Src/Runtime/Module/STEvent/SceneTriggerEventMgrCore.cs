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
