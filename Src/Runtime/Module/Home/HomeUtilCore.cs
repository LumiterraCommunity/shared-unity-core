using System.Collections.Generic;
using GameMessageCore;
using UnityGameFramework.Runtime;
using static HomeDefine;
/// <summary>
/// 家园工具core
/// </summary>
public static class HomeUtilCore
{
    /// <summary>
    /// 检查某个场景配置是否是家园模块 out家园类型
    /// </summary>
    /// <param name="dRSceneArea"></param>
    /// <param name="homeType"></param>
    /// <returns></returns>
    public static bool CheckIsHomeModuleScene(DRSceneArea dRSceneArea, out eHomeType homeType)
    {
        if (dRSceneArea == null)
        {
            Log.Error("HomeUtilCore CheckIsHomeModuleScene dRSceneArea is null.");
            homeType = eHomeType.Unknown;
            return false;
        }

        if (dRSceneArea.SceneType == (int)eSceneType.Home)
        {
            homeType = eHomeType.Personal;
            return true;
        }
        else if (dRSceneArea.SceneType == (int)eSceneType.Instancing && dRSceneArea.SceneSubtype == (int)DungeonSubType.Home)
        {
            homeType = eHomeType.Instancing;
            return true;
        }

        homeType = eHomeType.Unknown;
        return false;
    }

    /// <summary>
    /// 计算土地的所有已使用肥沃值
    /// </summary>
    /// <param name="soils"></param>
    /// <returns></returns>
    public static int CalculateTotalFertilityUsed(IEnumerable<HomeSoilCore> soils)
    {
        int totalFertilityUsed = 0;
        foreach (HomeSoilCore soil in soils)
        {
            if (soil.SoilData.SaveData.Fertile > 0)
            {
                totalFertilityUsed += soil.SoilData.SaveData.Fertile;
            }
        }
        return totalFertilityUsed;
    }

    /// <summary>
    /// 计算所有动物的已使用幸福值
    /// </summary>
    /// <param name="animals"></param>
    /// <returns></returns>
    public static int CalculateTotalHappyUsed(IEnumerable<HomeAnimalCore> animals)
    {
        int totalHappyUsed = 0;
        foreach (HomeAnimalCore animal in animals)
        {
            if (animal.Data.SaveData.Happiness > 0)
                totalHappyUsed += animal.Data.SaveData.Happiness;
        }
        return totalHappyUsed;
    }

    /// <summary>
    /// 判断种子是否可以被摧毁 不能摧毁的往往体现在如何如果这个种子也要长成熟
    /// </summary>
    /// <param name="soilData"></param>
    /// <returns></returns>
    public static bool JudgeSeedCanDestroy(SoilData soilData)
    {
        if (soilData == null)
        {
            return false;
        }

        if (soilData.DRSeed == null)
        {
            return false;
        }

        if (soilData.DRSeed.FunctionType == (int)SeedFunctionType.Totem)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 判断种子是否是功能性实体类型
    /// </summary>
    /// <param name="soilData"></param>
    /// <returns></returns>
    public static bool JudgeSeedIsFunctionEntityType(SoilData soilData)
    {
        if (soilData == null)
        {
            return false;
        }

        if (soilData.DRSeed == null)
        {
            return false;
        }

        return soilData.DRSeed.FunctionType != (int)SeedFunctionType.None;
    }
}