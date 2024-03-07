using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

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
}