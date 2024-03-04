using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using static HomeDefine;

/// <summary>
/// 宠物采集资源检测雷达 挂载在主人身上 检测主人附近的资源 按需挂载
/// </summary>
public class PetCollectResourceCheckRadar : CheckEntityRadarBase
{
    protected override float CheckRadius => PET_COLLECT_RESOURCE_CHECK_RADIUS;

    protected override LayerMask CheckLayer => 1 << MLayerMask.HOME_RESOURCE;

    /// <summary>
    /// 当前附近的所有采集物 没有分类 为了快速查找
    /// </summary>
    /// <returns></returns>
    private readonly Dictionary<long, HomeResourcesCore> _entityIdMap = new();
    /// <summary>
    /// 附近采集物按照类型分类 如果某个类型没有则不包含在字典中
    /// </summary>
    /// <returns></returns>
    private readonly Dictionary<eAction, List<HomeResourcesCore>> _resTypeMap = new();

    /// <summary>
    /// 是否有某种采集物在附近
    /// </summary>
    /// <param name="resType"></param>
    /// <returns></returns>
    public bool IsHaveResourceOnNear(eAction resType)
    {
        return _resTypeMap.ContainsKey(resType);
    }

    protected override void OnAddEntity(EntityBase entity)
    {
        //已经存在 理论上走不到这里 上层有判断
        if (_entityIdMap.ContainsKey(entity.BaseData.Id))
        {
            Log.Error($"PetCollectResourceCheckRadar OnAddEntity entity id = {entity.BaseData.Id} is already exist");
            return;
        }

        //不是采集物
        if (!entity.TryGetComponent(out HomeResourcesCore resources))
        {
            return;
        }

        eAction resType = resources.SupportAction;
        if (!MathUtilCore.IsPowerOfTwo((int)resType))
        {
            Log.Error($"PetCollectResourceCheckRadar OnAddEntity resType = {resType} is not power of two,cid: {resources.Data.ConfigId}");
            return;
        }

        _entityIdMap.Add(entity.BaseData.Id, resources);

        if (!_resTypeMap.ContainsKey(resType))
        {
            _resTypeMap[resType] = new List<HomeResourcesCore>();
        }

        _resTypeMap[resType].Add(resources);
    }

    protected override void OnRemoveEntity(EntityBase entity)
    {
        //我不管理的实体
        if (!_entityIdMap.ContainsKey(entity.BaseData.Id))
        {
            return;
        }

        HomeResourcesCore resources = _entityIdMap[entity.BaseData.Id];
        _ = _entityIdMap.Remove(entity.BaseData.Id);

        eAction resType = resources.SupportAction;
        if (_resTypeMap.ContainsKey(resType))
        {
            _ = _resTypeMap[resType].Remove(resources);
            if (_resTypeMap[resType].Count == 0)
            {
                _ = _resTypeMap.Remove(resType);
            }
        }
    }

    /// <summary>
    /// 获取最近的某种资源实体id 每次都会遍历 有性能问题 慎用
    /// </summary>
    /// <param name="targetResType"></param>
    /// <returns>返回0代表没有</returns>
    public long GetNearestResourceEntityId(eAction targetResType)
    {
        if (!_resTypeMap.TryGetValue(targetResType, out List<HomeResourcesCore> resources))
        {
            return 0;
        }

        float minDistance = float.MaxValue;
        long nearestEntityId = 0;

        foreach (HomeResourcesCore resource in resources)
        {
            float distance = Vector3.Distance(transform.position, resource.Position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEntityId = resource.RefEntity.BaseData.Id;
            }
        }

        return nearestEntityId;
    }
}