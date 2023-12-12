using System.Collections.Generic;


using UnityEngine;
using UnityGameFramework.Runtime;

public abstract class ResourcesPointMgrCore : MonoBehaviour
{
    private readonly Dictionary<eSceneArea, ServerDataNodeListData<ResourcesPointData>> _areaResourcesPointConfigMap = new();

    protected readonly Dictionary<eSceneArea, Dictionary<eResourcesPointType, List<ResourcesPointData>>> AreaTypeResourcesPointMap = new();

    protected void AddAreaConfigData(eSceneArea sceneArea, ServerDataNodeListData<ResourcesPointData> resourcesPointConfig)
    {
        if (resourcesPointConfig == null)
        {
            Log.Warning("ResourcesPointMgrCore resourcesPointConfig null");
            return;
        }
        if (_areaResourcesPointConfigMap.ContainsKey(sceneArea))
        {
            Log.Warning($"ResourcesPointMgrCore AddAreaConfigData sceneArea repeat {sceneArea}");
            return;
        }
        _areaResourcesPointConfigMap.Add(sceneArea, resourcesPointConfig);
        InitResourcesPointConfig(sceneArea);
    }
    /// <summary>
    /// 初始化资源点配置
    /// </summary>
    private void InitResourcesPointConfig(eSceneArea sceneArea)
    {
        if (!_areaResourcesPointConfigMap.ContainsKey(sceneArea))
        {
            return;
        }
        ServerDataNodeListData<ResourcesPointData> serverDataNodeListData = _areaResourcesPointConfigMap[sceneArea];

        ResourcesPointData[] pointList = serverDataNodeListData.DataList;
        if (pointList == null)
        {
            return;
        }
        if (!AreaTypeResourcesPointMap.ContainsKey(sceneArea))
        {
            AreaTypeResourcesPointMap[sceneArea] = new();
        }

        for (int i = 0; i < pointList.Length; i++)
        {
            if (AreaTypeResourcesPointMap[sceneArea].TryGetValue((eResourcesPointType)pointList[i].ResourceType, out List<ResourcesPointData> points))
            {
                points.Add(pointList[i]);
            }
            else
            {
                points = new()
                {
                    pointList[i]
                };
                AreaTypeResourcesPointMap[sceneArea].Add((eResourcesPointType)pointList[i].ResourceType, points);
            }
        }
    }
    /// <summary>
    /// 获取指定area 资源点数据
    /// </summary>
    protected Dictionary<eResourcesPointType, List<ResourcesPointData>> GetResourcesPointData(eSceneArea sceneArea)
    {
        return AreaTypeResourcesPointMap.GetValueOrDefault(sceneArea, null);
    }

    /// <summary>
    /// 获取指定area 指定类型的 资源点列表
    /// </summary>
    public List<ResourcesPointData> GetResourcesPointDataList(eSceneArea sceneArea, eResourcesPointType type)
    {
        if (AreaTypeResourcesPointMap.TryGetValue(sceneArea, out Dictionary<eResourcesPointType, List<ResourcesPointData>> resourcesPointMap))
        {
            return resourcesPointMap.GetValueOrDefault(type);
        }
        return null;
    }

    private void OnDestroy()
    {
        Clear();
    }

    public virtual void Clear()
    {
        _areaResourcesPointConfigMap.Clear();
        AreaTypeResourcesPointMap.Clear();
    }
}
