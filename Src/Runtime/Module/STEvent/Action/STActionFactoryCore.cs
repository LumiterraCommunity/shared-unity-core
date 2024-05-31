/*
 * @Author: xiang huan
 * @Date: 2024-05-24 14:09:12
 * @Description: 
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/STEvent/Action/STActionFactoryCore.cs
 * 
 */

using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;


public class STActionFactoryCore
{
    protected Dictionary<eSTActionType, Type> STActionMap;
    /// <summary>
    /// 初始化场景触发器行为工厂Map
    /// </summary>
    public virtual void InitSTActionMap()
    {

    }

    /// <summary>
    /// 创建触发器行为
    /// </summary>
    /// <param name="cid">行为配置ID</param>
    /// <returns></returns>
    public STActionBase CreateSTAction(int cid, SceneTriggerEvent sceneEvent)
    {
        if (STActionMap == null)
        {
            Log.Error($"STActionMap Error not init map");
            return null;
        }
        DRSceneEventAction drAction = GFEntryCore.DataTable.GetDataTable<DRSceneEventAction>().GetDataRow(cid);
        if (drAction == null)
        {
            Log.Error($"CreateSTAction Error not find action id = {cid}");
            return null;
        }
        eSTActionType type = (eSTActionType)drAction.Type;
        if (!STActionMap.ContainsKey(type))
        {
            Log.Error($"CreateSTAction Error type is Unknown  type = {type} ");
            return null;
        }
        Type actionType = STActionMap[type];
        STActionBase action = STActionBase.Create(actionType);
        action.Init(drAction, sceneEvent);
        return action;
    }
}