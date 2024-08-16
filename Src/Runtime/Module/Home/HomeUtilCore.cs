using System.Collections.Generic;
using GameMessageCore;
using UnityEngine;
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

        eSceneFunctionModuleType modules = TableUtil.ConvertToBitEnum<eSceneFunctionModuleType>(dRSceneArea.FunctionModule);
        if ((modules & eSceneFunctionModuleType.Home) != 0)
        {
            if (dRSceneArea.SceneType == (int)eSceneType.Instancing)
            {
                homeType = eHomeType.Instancing;
            }
            else
            {
                homeType = eHomeType.Unknown;
            }
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
    /// 判断种子是否可以播种
    /// </summary>
    /// <param name="seedCid"></param>
    /// <param name="homeType"></param>
    /// <returns></returns>
    public static bool JudgeSeedCanSowing(int seedCid, eHomeType homeType, bool isOwner)
    {
        DRSeed drSeed = TableUtil.GetConfig<DRSeed>(seedCid);
        if (drSeed == null)
        {
            Log.Error("HomeUtilCore JudgeSeedCanSowing drSeed is null.");
            return false;
        }

        //图腾不能乱播种 只有自己家园才能播种
        if (drSeed.FunctionType == (int)SeedFunctionType.Totem)
        {
            return homeType == eHomeType.Personal && isOwner;
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

    /// <summary>
    /// 获取资源熟练度掉落概率 返回0-1 异常也返回0
    /// </summary>
    /// <param name="targetCurAction">拾取的不在这里</param>
    /// <returns></returns>
    public static float GetResourceProficiencyProbability(EntityAttributeData entityAttribute, HomeResourcesCore homeResource, eAction targetCurAction)
    {
        float resourceProficiency = homeResource.RefEntity.EntityAttributeData.GetRealValue(eAttributeType.RequiredProficiency);
        if (resourceProficiency <= 0)//防止分母0
        {
            Log.Error($"CheckCanGetResourceDrop error: resourceProficiency invalid {resourceProficiency}");
            return 0;
        }

        float playerProficiency;
        switch (targetCurAction)
        {
            case eAction.Mowing:
                playerProficiency = entityAttribute.GetRealValue(eAttributeType.GrassProficiency);
                break;
            case eAction.Cut:
                playerProficiency = entityAttribute.GetRealValue(eAttributeType.TreeProficiency);
                break;
            case eAction.Mining:
                playerProficiency = entityAttribute.GetRealValue(eAttributeType.OreProficiency);
                break;
            default:
                playerProficiency = resourceProficiency;
                Log.Error($"CheckCanGetResourceDrop error: targetCurAction invalid {targetCurAction}");
                break;
        }

        float fromLevel = entityAttribute.RefEntity.GetComponent<EntityAvatarDataCore>().AbilityLevel;
        float toLevel = homeResource.GetActionLevel(targetCurAction);
        float probability = playerProficiency / resourceProficiency * Mathf.Pow(2, fromLevel - toLevel + 1);//概率 = 采集熟练度/需求熟练度 * 2^(采集装等 - 采集物等级 + 1)
        return probability;
    }
}