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

        for (int i = 0; i < countX; i++)
        {
            for (int j = 0; j < countZ; j++)
            {
                Vector3 pos = new(minPos.x + i * HomeDefine.SOIL_SIZE.x, minPos.y, minPos.z + j * HomeDefine.SOIL_SIZE.z);
                ulong id = MathUtilCore.AreaToSoil(area.Id, i, j);
                HomeModuleCore.SoilMgr.AddSoil(id, pos);
            }
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