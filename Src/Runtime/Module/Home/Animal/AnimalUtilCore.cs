using UnityGameFramework.Runtime;
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

    /// <summary>
    /// 从monster实体上生成 AnimalBaseData数据 异常返回null
    /// </summary>
    /// <returns></returns>
    public static AnimalBaseData GenerateAnimalBaseDataFromMonster(EntityBase monster)
    {
        if (monster == null)
        {
            Log.Error("GenerateAnimalBaseDataFromMonster monster is null");
            return null;
        }

        DRMonster dRMonster = monster.GetComponent<MonsterDataCore>().DRMonster;
        long createMs = TimeUtil.GetServerTimeStamp();
        return new()
        {
            AnimalId = (ulong)monster.BaseData.Id,
            Name = dRMonster.Name,
            Cid = dRMonster.Id,
            Favorability = 0,
            CreateMs = createMs,
            UpdateMs = createMs,
        };
    }
}