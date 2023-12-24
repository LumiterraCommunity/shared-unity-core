/*
 * @Author: xiang huan
 * @Date: 2022-09-13 17:26:26
 * @Description: 实体检测位置
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Entity/EntityCheckPosCore.cs
 * 
 */

using UnityEngine;


/// <summary>
/// 实体检测位置
/// </summary>
public class EntityCheckPosCore : EntityBaseComponent
{
    public float DynamicErrorDist;     //动态误差距离
    public long LastSyncTime;      //上次同步位置时间
    public void AddErrorDist(float dist)
    {
        DynamicErrorDist += dist;
    }
    public void ResetErrorDist()
    {
        DynamicErrorDist = 0;
    }

    public void ForceSetPosition(Vector3 pos)
    {
        LastSyncTime = TimeUtil.GetServerTimeStamp();
        RefEntity.SetPosition(pos);
        ResetErrorDist();
    }

    public virtual bool CheckPositionValid(Vector3 cur, Vector3 dest)
    {
        return true;
    }

    /// <summary>
    /// 检测并且设置位置
    /// </summary>
    /// <param name="cur"></param>
    /// <param name="dest"></param>
    /// <returns></returns> <summary>
    public bool CheckSetPosition(Vector3 cur, Vector3 dest)
    {
        if (CheckPositionValid(cur, dest))
        {
            ForceSetPosition(dest);
            return true;
        }
        return false;
    }

}