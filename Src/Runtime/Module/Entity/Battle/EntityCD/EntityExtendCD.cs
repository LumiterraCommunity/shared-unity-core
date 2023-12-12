/*
 * @Author: xiang huan
 * @Date: 2022-09-23 10:08:22
 * @Description: 实体扩展CD，子业务用到的CD，例如角色复活等待CD
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/EntityCD/EntityExtendCD.cs
 * 
 */


using System;
using System.Collections.Generic;
using GameFramework;
using GameMessageCore;

public class EntityExtendCD : EntityCDBase
{
    /// <summary>
    /// 根据协议消息初始化CD
    /// </summary>
    /// <param name="entityCD">协议数据</param>
    public override void InitSvrEntityCD(GameMessageCore.EntityCD entityCD)
    {
        CDMap.Clear();
        long curTimeStamp = TimeUtil.GetServerTimeStamp();
        if (entityCD.ExtendCdList != null && entityCD.ExtendCdList.Count > 0)
        {
            for (int i = 0; i < entityCD.ExtendCdList.Count; i++)
            {
                if (entityCD.ExtendCdList[i].Time > curTimeStamp)
                {
                    CDMap[entityCD.ExtendCdList[i].Type] = entityCD.ExtendCdList[i].Time;
                }
            }
        }
    }

    /// <summary>
    /// 返回协议数据
    /// </summary>
    /// <param name="entityCD">协议数据</param>
    public override GameMessageCore.EntityCD ToSvrEntityCD(GameMessageCore.EntityCD entityCD)
    {
        foreach (KeyValuePair<int, long> item in CDMap)
        {
            GameMessageCore.EntityExtendCD extendCD = new()
            {
                Type = item.Key,
                Time = item.Value
            };
            entityCD.ExtendCdList.Add(extendCD);
        }
        return entityCD;
    }
}