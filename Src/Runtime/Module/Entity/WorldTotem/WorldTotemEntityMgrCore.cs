using System;
using UnityEngine;
using UnityEngine.AI;
using UnityGameFramework.Runtime;


/// <summary>
/// 世界图腾实体管理器 当前世界中的图腾实体管理 并非自己的所有图腾管理
/// </summary>
public class WorldTotemEntityMgrCore : SceneModuleBase
{
    /// <summary>
    /// 当前管理的所有图腾实体 客户端为视野内 key:实体id
    /// </summary>
    protected readonly ListMap<long, EntityBase> EntityMap = new();

    internal void AddWorldTotem(EntityBase refEntity)
    {
        if (!EntityMap.Add(refEntity.BaseData.Id, refEntity))
        {
            Log.Error($"WorldTotemEntityMgrCore.AddWorldTotem() is already exist id:{refEntity.BaseData.Id}");
        }
    }

    internal void RemoveWorldTotem(EntityBase refEntity)
    {
        if (!EntityMap.Remove(refEntity.BaseData.Id))
        {
            Log.Error($"WorldTotemEntityMgrCore.RemoveWorldTotem() is not exist id:{refEntity.BaseData.Id}");
        }
    }

    /// <summary>
    /// 检查是否和其他图腾重叠 有一定性能开销 需要注意
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool CheckOverlapOtherTotem(Vector3 pos)
    {
        if (EntityMap.Count == 0)
        {
            return false;
        }

        float intervalRange = WorldTotemDefineCore.IntervalRange;
        for (int i = 0; i < EntityMap.Count; i++)
        {
            EntityBase entity = EntityMap[i];
            if (Vector3.Distance(entity.Position, pos) < intervalRange)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 检查地形是否可以放置
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool CheckTerrainCanPut(Vector3 pos)
    {
        if (!MapUtilCore.SampleTerrainWalkablePos(pos, out Vector3 _, 1f))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 获取指定位置的密度 有性能开销 需要注意
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public int GetDensityByPos(Vector3 pos)
    {
        float densityRange = WorldTotemDefineCore.DensityRange;
        int density = 0;
        for (int i = 0; i < EntityMap.Count; i++)
        {
            EntityBase entity = EntityMap[i];
            if (Vector3.Distance(entity.Position, pos) < densityRange)
            {
                density++;
            }
        }

        return density;
    }
}