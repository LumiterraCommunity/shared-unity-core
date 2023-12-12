/*
 * @Author: xiang huan
 * @Date: 2022-09-23 10:08:22
 * @Description: 实体技能CD
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/EntityCD/EntitySkillCD.cs
 * 
 */


using System;
using System.Collections.Generic;
using GameFramework;
using GameMessageCore;

public class EntitySkillCD : EntityCDBase
{
    /// <summary>
    /// 根据协议消息初始化CD
    /// </summary>
    /// <param name="entityCD">协议数据</param>
    public override void InitSvrEntityCD(GameMessageCore.EntityCD entityCD)
    {
        CDMap.Clear();
        long curTimeStamp = TimeUtil.GetServerTimeStamp();
        if (entityCD.SkillCdList != null && entityCD.SkillCdList.Count > 0)
        {
            for (int i = 0; i < entityCD.SkillCdList.Count; i++)
            {
                if (entityCD.SkillCdList[i].Time > curTimeStamp)
                {
                    CDMap[entityCD.SkillCdList[i].SkillId] = entityCD.SkillCdList[i].Time;
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
            GameMessageCore.EntitySkillCD skillCD = new()
            {
                SkillId = item.Key,
                Time = item.Value
            };
            entityCD.SkillCdList.Add(skillCD);
        }
        return entityCD;
    }
}