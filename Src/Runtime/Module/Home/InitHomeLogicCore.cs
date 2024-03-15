using System.Collections.Generic;
using GameMessageCore;
using UnityEngine;
using UnityGameFramework.Runtime;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// 初始化家园逻辑core 相当于启动逻辑共享部分
/// </summary>
public class InitHomeLogicCore : MonoBehaviour
{

    private void Start()
    {
        InitSoil();
    }

    //初始化所有土地
    private void InitSoil()
    {
        GenerateSoil();
    }

    //生成具体土地
    private void GenerateSoil()
    {
        List<HomeResourcesArea> list = GFEntryCore.HomeResourcesAreaMgr.GetAreaListByType(HomeDefine.eHomeResourcesAreaType.farmland);
        list.Sort((a, b) => a.Id.CompareTo(b.Id));//按照id排序生成土地
        for (int i = 0; i < list.Count; i++)
        {
            CreateSoil(list[i]);
        }
    }

    private void CreateSoil(HomeResourcesArea area)
    {
        Vector3 minPos = area.AreaBounds.min;
        Vector3 maxPos = area.AreaBounds.max;
        int countX = (int)System.MathF.Floor((maxPos.x - minPos.x) / HomeDefine.SOIL_SIZE.x);
        int countZ = (int)System.MathF.Floor((maxPos.z - minPos.z) / HomeDefine.SOIL_SIZE.z);

        //不是用行扫描方式生成土地 而是回路方式生成土地类似贪吃蛇顺序 目的为了遍历时的土地在空间上是连续的 给宠物AI更自然
        for (int i = 0; i < countX; i++)
        {
            if ((i & 1) == 0)//偶数行
            {
                for (int j = 0; j < countZ; j++)
                {
                    AddSoil(minPos, area.Id, i, j);
                }
            }
            else //奇数行
            {
                for (int j = countZ - 1; j >= 0; j--)
                {
                    AddSoil(minPos, area.Id, i, j);
                }
            }
        }

        static void AddSoil(Vector3 minPos, int areaId, int i, int j)
        {
            Vector3 pos = new(minPos.x + i * HomeDefine.SOIL_SIZE.x, minPos.y, minPos.z + j * HomeDefine.SOIL_SIZE.z);
            ulong id = MathUtilCore.AreaToSoil(areaId, i, j);
            HomeModuleCore.SoilMgr.AddSoil(id, pos);
        }
    }

    /// <summary>
    /// 从家园保存数据中恢复土地状态
    /// </summary>
    /// <param name="saveData"></param>
    public void RestoreSoilStatus(HomeSaveData saveData)
    {
        if (saveData == null || saveData.SoilSaveDataList == null || saveData.SoilSaveDataList.Count == 0)
        {
            Log.Info($"saveData is null, init soil default status");
            return;
        }

        foreach (SoilSaveData data in saveData.SoilSaveDataList)
        {
            ulong id = data.Id;
            HomeSoilCore soil = HomeModuleCore.SoilMgr.GetSoil(id);
            if (soil == null)
            {
                Log.Error($"not found soil form db, id:{id}");
                continue;
            }

            soil.SoilEvent.MsgInitStatus?.Invoke(data);
        }
    }

    /// <summary>
    /// 从家园保存数据中恢复实体种子状态 可以根据业务层数据结构分多次恢复也可以
    /// </summary>
    public void RestoreSeedEntityStatus(List<ProxySeedEntityData> entityProxyDataList)
    {
        if (entityProxyDataList == null || entityProxyDataList.Count == 0)
        {
            return;
        }

        foreach (ProxySeedEntityData data in entityProxyDataList)
        {
            try
            {
                HomeSoilCore soil = HomeModuleCore.SoilMgr.GetSoil(data.SoilData.Id);
                if (soil == null)
                {
                    Log.Error($"not found soil form db, id:{data.SoilData.Id}");
                    continue;
                }

                //填充实体数据
                SoilSeedEntityProxyDataProcess process = soil.GetComponent<SoilSeedEntityProxyDataProcess>();
                process.SetInitProxyData(data);

                //恢复土地状态 顺序不能错 要等上面先填充实体数据 状态恢复生成实体时会用到
                soil.SoilEvent.MsgInitStatus?.Invoke(new SoilSaveData(data.SoilData));
            }
            catch (System.Exception e)
            {
                Log.Error($"totem restore is error, id:{data.SoilData.Id}error:{e}");
                continue;
            }
        }
    }
}