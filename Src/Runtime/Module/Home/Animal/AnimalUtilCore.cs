/// <summary>
/// 畜牧工具
/// </summary>
public class AnimalUtilCore
{
    /// <summary>
    /// 计算动物的好感度对应的心的数量
    /// </summary>
    /// <param name="favorabilityValue"></param>
    /// <returns></returns>
    public static int CalculateAnimalFavorabilityHeartNum(int favorabilityValue)
    {
        return favorabilityValue / TableUtil.GetGameValue(eGameValueID.animalFavorabilityValueEveryHeart).Value;
    }
}