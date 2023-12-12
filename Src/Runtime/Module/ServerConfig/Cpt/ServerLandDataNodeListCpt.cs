using System;
/*
 * @Author: wym
 * @Date: 2022-11-16 15:10:48
 * @Description: 领地数据节点list组件 
 * @FilePath: /Assets/Plugins/SharedCore/Src/Runtime/Module/ServerConfig/Cpt/ServerLandDataNodeListCpt.cs
 * 
 */
using System.Collections.Generic;
public class ServerLandDataNodeListCpt : ServerDataNodeListCpt
{
    public override object GetServerData()
    {
        ServerDataNodeListData<object> config = (ServerDataNodeListData<object>)base.GetServerData();
        LandGridData[] listData = new LandGridData[config.DataList.Length];
        for (int i = 0; i < config.DataList.Length; i++)
        {
            listData[i] = (LandGridData)config.DataList[i];
        }

        // 从小到达排序id
        Array.Sort<LandGridData>(listData);

        // id 连续性检查
        for (int i = 1; i < listData.Length; i++)
        {
            LandGridData pre = listData[i - 1];
            LandGridData cur = listData[i];
            if (pre.ID != cur.ID - 1)
            {
                throw new System.Exception($"领地ID不连续,不可导出,请检查Id{cur.ID}");
            }
        }
        return config;
    }
}