/*
 * @Author: xiang huan
 * @Date: 2022-06-27 14:13:48
 * @Description: 副本关卡数据组件 资源列表 boss索引列表
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/ServerConfig/Cpt/InstancingLevelDataNodeCpt.cs
 * 
 */
using System.Collections.Generic;
using UnityEngine;
public class InstancingLevelDataNodeCpt : MonoBehaviour, IServerDataNodeCpt
{
    [Header("关卡AI")]
    public string LevelAI = "";
    [Header("出生点")]
    public GameObject BirthPoint;
    public object GetServerData()
    {
        List<ResourcesPointData> resourcesList = new();
        SearchResourcesDataNode(transform, resourcesList);
        Transform tansformPoint = BirthPoint != null ? BirthPoint.transform : transform;
        InstancingLevelData data = new()
        {
            X = tansformPoint.position.x,
            Y = tansformPoint.position.y,
            Z = tansformPoint.position.z,
            LevelAI = LevelAI,
            ResourcesPointList = resourcesList.ToArray(),
        };
        return data;
    }

    public void SearchResourcesDataNode(Transform parentTransform, List<ResourcesPointData> list)
    {
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform childTransform = parentTransform.GetChild(i);
            if (childTransform.gameObject.TryGetComponent(out ResourcesPointDataNodeCpt dataCpt))
            {
                list.Add(dataCpt.GetServerData() as ResourcesPointData);
            }
            if (childTransform.childCount > 0)
            {
                SearchResourcesDataNode(childTransform, list);
            }
        }
    }
}