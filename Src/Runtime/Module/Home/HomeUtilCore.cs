using System.Collections.Generic;
/// <summary>
/// 家园工具core
/// </summary>
public static class HomeUtilCore
{
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
}