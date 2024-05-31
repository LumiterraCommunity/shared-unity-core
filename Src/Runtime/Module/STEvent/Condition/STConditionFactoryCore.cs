/*
 * @Author: xiang huan
 * @Date: 2024-05-24 13:56:20
 * @Description: 
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/STEvent/Condition/STConditionFactoryCore.cs
 * 
 */

using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;


public class STConditionFactoryCore
{
    protected Dictionary<eSTConditionType, Type> STConditionMap;

    /// <summary>
    /// 初始化触发器事件工厂Map
    /// </summary>
    public virtual void InitSTConditionMap()
    {

    }

    /// <summary>
    /// 创建触发器条件
    /// </summary>
    /// <param name="cid">条件配置ID</param> 
    public STConditionBase CreateSTCondition(int cid)
    {
        if (STConditionMap == null)
        {
            Log.Error($"STConditionMap Error not init map");
            return null;
        }
        DRSceneEventCondition drCondition = GFEntryCore.DataTable.GetDataTable<DRSceneEventCondition>().GetDataRow(cid);
        if (drCondition == null)
        {
            Log.Error($"CreateSTCondition Error not find condition id = {cid}");
            return null;
        }
        eSTConditionType type = (eSTConditionType)drCondition.Type;
        if (!STConditionMap.ContainsKey(type))
        {
            Log.Error($"CreateSTCondition Error type is Unknown  type = {type} ");
            return null;
        }
        Type stClass = STConditionMap[type];
        STConditionBase stConditionBase = STConditionBase.Create(stClass);
        stConditionBase.Init(drCondition);
        return stConditionBase;
    }
}