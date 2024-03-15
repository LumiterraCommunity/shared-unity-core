/*
 * @Author: xiang huan
 * @Date: 2022-12-08 15:29:03
 * @Description: 家园资源区域管理
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/Home/HomeResourcesAreaMgrCore.cs
 * 
 */

using System.Collections.Generic;
using UnityGameFramework.Runtime;

public class HomeResourcesAreaMgrCore : SceneModuleBase
{
    protected Dictionary<int, HomeResourcesArea> AreaMap = new();

    public virtual void AddArea(HomeResourcesArea area)
    {
        if (AreaMap.ContainsKey(area.Id))
        {
            Log.Error($"Resources Area ID Repeat !!! ID = {area.Id}");
            return;
        }
        AreaMap.Add(area.Id, area);
    }
    public virtual void RemoveArea(int id)
    {
        if (!AreaMap.ContainsKey(id))
        {
            return;
        }
        _ = AreaMap.Remove(id);
    }

    public HomeResourcesArea GetArea(int id)
    {
        if (!AreaMap.ContainsKey(id))
        {
            return null;
        }
        return AreaMap[id];
    }
    public List<HomeResourcesArea> GetAreaListByType(HomeDefine.eHomeResourcesAreaType type)
    {
        List<HomeResourcesArea> list = new();
        foreach (KeyValuePair<int, HomeResourcesArea> item in AreaMap)
        {
            if ((HomeDefine.eHomeResourcesAreaType)item.Value.DRHomeResourceArea.Type == type)
            {
                list.Add(item.Value);
            }
        }

        return list;
    }

    public virtual void InitHomeAreaSaveData(List<HomeResourcesAreaSaveData> areaSaveDataList)
    {
        if (areaSaveDataList == null)
        {
            return;
        }
        for (int i = 0; i < areaSaveDataList.Count; i++)
        {
            if (AreaMap.TryGetValue(areaSaveDataList[i].Id, out HomeResourcesArea area))
            {
                area.SetSaveData(areaSaveDataList[i]);
            }
        }
    }

    public List<HomeResourcesAreaSaveData> GetResourceSaveData()
    {
        List<HomeResourcesAreaSaveData> list = new();
        try
        {
            foreach (KeyValuePair<int, HomeResourcesArea> item in AreaMap)
            {
                list.Add(item.Value.GetSaveData());
            }
        }
        catch (System.Exception e)
        {
            Log.Error($"GetResourceSaveData Error : {e}");
        }
        return list;
    }
}
