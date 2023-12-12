using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 存放当前家园土地和家园采集资源关系的数据模块
/// </summary>
public class HomeSoilResourceRelation : MonoBehaviour
{
    private readonly Dictionary<ulong, long> _soilMap = new();//土地和上面资源的关系 id是土地
    private readonly Dictionary<long, ulong> _resourceMap = new();//上面资源和土地的关系 id是资源实体

    /// <summary>
    /// 在土地上添加一个资源
    /// </summary>
    /// <param name="resourceEntityId">资源实体id</param>
    /// <param name="soilId">所在土地id</param>
    public void AddResourceOnSoil(long resourceEntityId, ulong soilId)
    {
        if (_soilMap.ContainsKey(soilId))
        {
            Log.Error($"soil id:{soilId} already exist");
            return;
        }

        if (_resourceMap.ContainsKey(resourceEntityId))
        {
            Log.Error($"resource id:{resourceEntityId} already exist");
            return;
        }

        _soilMap.Add(soilId, resourceEntityId);
        _resourceMap.Add(resourceEntityId, soilId);
    }

    /// <summary>
    /// 在土地上移除一个资源
    /// </summary>
    /// <param name="resourceEntityId"></param>
    public void RemoveResourceOnSoil(long resourceEntityId)
    {
        if (!_resourceMap.ContainsKey(resourceEntityId))
        {
            Log.Error($"resource id:{resourceEntityId} not exist");
            return;
        }

        _ = _soilMap.Remove(_resourceMap[resourceEntityId]);
        _ = _resourceMap.Remove(resourceEntityId);
    }

    /// <summary>
    /// 在某个土地上是否有资源
    /// </summary>
    /// <param name="soilId"></param>
    /// <returns></returns>
    public bool HaveResourceOnSoil(ulong soilId)
    {
        return _soilMap.ContainsKey(soilId);
    }
}