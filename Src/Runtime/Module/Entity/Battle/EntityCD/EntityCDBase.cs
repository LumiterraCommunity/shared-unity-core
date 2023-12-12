/*
 * @Author: xiang huan
 * @Date: 2022-09-23 09:55:08
 * @Description: 实体CD基础
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/EntityCD/EntityCDBase.cs
 * 
 */

using System;
using System.Collections.Generic;
using GameFramework;
using GameMessageCore;

public class EntityCDBase : IReference
{
    /// <summary>
    ///  int key  long 到期时间戳ms
    /// </summary>
    public Dictionary<int, long> CDMap { get; private set; }

    public EntityCDBase()
    {
        CDMap = new();
    }

    /// <summary>
    /// 根据协议消息初始化CD
    /// </summary>
    /// <param name="entityCD">协议数据</param>
    public virtual void InitSvrEntityCD(GameMessageCore.EntityCD entityCD)
    {

    }

    /// <summary>
    /// 返回协议数据
    /// </summary>
    /// <param name="entityCD">协议数据</param>
    public virtual GameMessageCore.EntityCD ToSvrEntityCD(GameMessageCore.EntityCD entityCD)
    {
        return entityCD;
    }

    /// <summary>
    /// 是否CD
    /// </summary>
    /// <param name="key">cdKey</param>
    public virtual bool IsCD(int key)
    {
        if (CDMap.TryGetValue(key, out long outTime))
        {
            long curTimeStamp = TimeUtil.GetServerTimeStamp();
            return outTime > curTimeStamp;
        }
        return false;
    }

    /// <summary>
    /// 返回CD 
    /// </summary>
    /// <param name="key">cdKey</param>
    /// <returns>返回时间戳</returns>
    public virtual long GetCD(int key)
    {
        if (CDMap.TryGetValue(key, out long value))
        {
            return value;
        }
        return 0;
    }
    /// <summary>
    /// 设置CD ms
    /// </summary>
    /// <param name="key">cdKey</param>
    /// <param name="time">到期时间戳</param>
    public virtual void SetCD(int key, long time)
    {
        CDMap[key] = time;
    }

    /// <summary>
    /// 重置所有CD 
    /// </summary>
    public virtual void ResetAllCD()
    {
        CDMap.Clear();
    }
    public virtual void Clear()
    {
        ResetAllCD();
    }

    /// <summary>
    /// 创建实体CD
    /// </summary>
    public static EntityCDBase Create(Type entityCDClass)
    {
        EntityCDBase entityCD = ReferencePool.Acquire(entityCDClass) as EntityCDBase;
        return entityCD;
    }

    /// <summary>
    /// 销毁实体CD
    /// </summary>
    public void Dispose()
    {
        ReferencePool.Release(this);
    }
}