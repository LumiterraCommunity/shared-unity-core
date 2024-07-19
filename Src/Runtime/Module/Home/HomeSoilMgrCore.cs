using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 家园土地管理共享库
/// </summary>
/// <typeparam name="TSoil">具体的家园土地类型</typeparam>
public class HomeSoilMgrCore<TSoil> : MonoBehaviour, IHomeSoilMgr where TSoil : HomeSoilCore
{
    private Transform _soilRoot;//土地根节点

    private readonly Dictionary<ulong, TSoil> _soilMap = new();//土地字典
    private readonly List<TSoil> _soilList = new();//土地列表 用来加速遍历
    /// <summary>
    /// 所有土地列表 用来遍历使用
    /// </summary>
    public IReadOnlyList<TSoil> SoilList => _soilList;

    private void Awake()
    {
        InitRoot();
    }

    private void OnDestroy()
    {
        _soilMap.Clear();
        _soilList.Clear();
        Destroy(_soilRoot.gameObject);
    }

    private void InitRoot()
    {
        _soilRoot = new GameObject("SoilRoot").transform;
        _soilRoot.SetParent(transform);
        _soilRoot.localPosition = Vector3.zero;
    }

    public void AddSoil(ulong id, Vector3 pos)
    {
        if (_soilMap.ContainsKey(id))
        {
            Log.Error($"soil id:{id} already exist");
            return;
        }

        TSoil soil = CreateSoil(id);
        soil.transform.SetParent(_soilRoot);
        soil.transform.position = pos;

        _soilMap.Add(id, soil);
        _soilList.Add(soil);

        soil.GetComponent<SoilStatusCtrl>().StartStatus(HomeDefine.eSoilStatus.Idle);
    }

    private TSoil CreateSoil(ulong id)
    {
        GameObject go = new($"Soil_{id}");
        TSoil soil = go.AddComponent<TSoil>();
        soil.SoilData.SetId(id);
        return soil;
    }

    public void RemoveSoil(ulong id)
    {
        if (!_soilMap.ContainsKey(id))
        {
            Log.Error($"soil id:{id} not exist");
            return;
        }

        TSoil soil = _soilMap[id];
        _ = _soilMap.Remove(id);
        _ = _soilList.Remove(soil);
        Destroy(soil.gameObject);
    }

    /// <summary>
    /// 获取某个土地 不存在返回null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public TSoil GetSoil(ulong id)
    {
        return _soilMap.TryGetValue(id, out TSoil soil) ? soil : null;
    }

    /// <summary>
    /// 根据区域获取空地列表
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<TSoil> GetIdleSoilListByArea(int id)
    {
        List<TSoil> list = new();
        foreach (KeyValuePair<ulong, TSoil> item in _soilMap)
        {
            SoilData data = item.Value.SoilData;
            int areaId = MathUtilCore.SoilToArea(data.SaveData.Id);
            if (areaId == id && data.SaveData.SoilStatus == HomeDefine.eSoilStatus.Idle)
            {
                list.Add(item.Value);
            }
        }
        return list;
    }
    HomeSoilCore IHomeSoilMgr.GetSoil(ulong id)
    {
        return GetSoil(id);
    }

    IEnumerable<HomeSoilCore> IHomeSoilMgr.GetAllSoil()
    {
        return SoilList;
    }

    /// <summary>
    /// 重置所有土地到idle状态 意思清理种子 返回确实重置了的数量
    /// </summary>
    public int ResetAllSoilToIdle()
    {
        int count = 0;
        if (_soilList == null || _soilList.Count == 0)
        {
            return count;
        }

        foreach (HomeSoilCore soil in _soilList)
        {
            try
            {
                if (soil.SoilData.SaveData.SoilStatus == HomeDefine.eSoilStatus.Idle)//已经是idle的不管
                {
                    continue;
                }

                soil.ResetToIdleStatus();
                count++;
            }
            catch (System.Exception e)
            {
                Log.Error($"ResetAllSoilToIdle soilId:{soil.Id} error:{e}");
            }
        }

        return count;
    }
}