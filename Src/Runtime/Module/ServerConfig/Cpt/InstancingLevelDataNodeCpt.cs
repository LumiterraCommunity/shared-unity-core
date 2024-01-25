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
    public List<GameObject> BirthPoint;
    public object GetServerData()
    {
        List<ResourcesPointData> resourcesList = new();
        SearchResourcesDataNode(transform, resourcesList);
        List<System.Numerics.Vector3> birthPointList = new();
        if (BirthPoint != null && BirthPoint.Count > 0)
        {
            for (int i = 0; i < BirthPoint.Count; i++)
            {
                birthPointList.Add(new System.Numerics.Vector3(BirthPoint[i].transform.position.x, BirthPoint[i].transform.position.y, BirthPoint[i].transform.position.z));
            }
        }
        else
        {
            birthPointList.Add(new System.Numerics.Vector3(transform.position.x, transform.position.y, transform.position.z));
        }

        InstancingLevelData data = new()
        {
            X = transform.position.x,
            Y = transform.position.y,
            Z = transform.position.z,
            LevelAI = LevelAI,
            ResourcesPointList = resourcesList.ToArray(),
            BirthPointList = birthPointList.ToArray(),
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