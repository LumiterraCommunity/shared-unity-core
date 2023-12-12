/*
 * @Author: xiang huan
 * @Date: 2022-06-27 14:13:48
 * @Description: 副本关卡数据组件 资源列表 boss索引列表
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/ServerConfig/Cpt/InstancingDataNodeCpt.cs
 * 
 */
using System.Collections.Generic;
using UnityEngine;
public class InstancingDataNodeCpt : MonoBehaviour, IServerDataNodeCpt
{
    public object GetServerData()
    {
        List<object> dataList = new();
        SearchDataNode(transform, dataList);
        ServerDataNodeListData<object> config = new();
        config.DataList = dataList.ToArray();
        return config;
    }

    public void SearchDataNode(Transform parentTransform, List<object> list)
    {
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform childTransform = parentTransform.GetChild(i);
            if (childTransform.gameObject.TryGetComponent(out InstancingLevelDataNodeCpt dataCpt))
            {
                list.Add(dataCpt.GetServerData());
            }
            if (childTransform.childCount > 0)
            {
                SearchDataNode(childTransform, list);
            }
        }
    }
}