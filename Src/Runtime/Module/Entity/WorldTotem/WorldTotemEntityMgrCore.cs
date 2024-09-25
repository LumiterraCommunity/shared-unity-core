using UnityEngine;
using UnityGameFramework.Runtime;


/// <summary>
/// 世界图腾实体管理器 当前世界中的图腾实体管理 并非自己的所有图腾管理
/// </summary>
public class WorldTotemEntityMgrCore : SceneModuleBase
{
    /// <summary>
    /// 当前管理的所有图腾实体 客户端为视野内 key:实体id
    /// </summary>
    private readonly ListMap<long, WorldTotemDataCore> _entityMap = new();

    /// <summary>
    /// 当前管理的所有图腾实体 客户端为视野内 key:实体id 无GC不要修改内部元素
    /// </summary>
    public ListMap<long, WorldTotemDataCore> EntityMap => _entityMap;

    internal void AddWorldTotem(WorldTotemDataCore totemData)
    {
        if (!_entityMap.Add(totemData.BaseData.Id, totemData))
        {
            Log.Error($"WorldTotemEntityMgrCore.AddWorldTotem() is already exist id:{totemData.BaseData.Id}");
        }
    }

    internal void RemoveWorldTotem(WorldTotemDataCore totemData)
    {
        if (!_entityMap.Remove(totemData.BaseData.Id))
        {
            Log.Error($"WorldTotemEntityMgrCore.RemoveWorldTotem() is not exist id:{totemData.BaseData.Id}");
        }
    }

    /// <summary>
    /// 检查是否和其他图腾重叠 有一定性能开销 需要注意
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool CheckOverlapOtherTotem(Vector3 pos)
    {
        if (_entityMap.Count == 0)
        {
            return false;
        }

        float intervalRange = WorldTotemDefineCore.IntervalRange;
        for (int i = 0; i < _entityMap.Count; i++)
        {
            WorldTotemDataCore totemDataCore = _entityMap[i];
            if (Vector3.Distance(totemDataCore.RefEntity.Position, pos) < intervalRange)
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
        for (int i = 0; i < _entityMap.Count; i++)
        {
            WorldTotemDataCore totemDataCore = _entityMap[i];
            if (Vector3.Distance(totemDataCore.RefEntity.Position, pos) < densityRange)
            {
                density++;
            }
        }

        return density;
    }
}