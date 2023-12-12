/*
 * @Author: xiang huan
 * @Date: 2022-06-27 14:13:48
 * @Description: 数据节点list组件
 * @FilePath: /Assets/Plugins/SharedCore/Src/Runtime/Module/ServerConfig/Cpt/ServerDataNodeListCpt.cs
 * 
 */
using System.Collections.Generic;
using UnityEngine;
public class ServerDataNodeListCpt : MonoBehaviour, IServerDataNodeCpt
{
    public virtual object GetServerData()
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
            if (childTransform.gameObject.TryGetComponent(out IServerDataNodeCpt dataCpt))
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