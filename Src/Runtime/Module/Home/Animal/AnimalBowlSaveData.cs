/// <summary>
/// 畜牧食盆存档数据
/// </summary>
[System.Serializable]
public class AnimalBowlSaveData
{
    public ulong BowlId;
    /// <summary>
    /// 食物cid 如果没有食物则为0
    /// </summary>
    public int FoodCid = 0;
    /// <summary>
    /// 剩余食物容量
    /// </summary>
    public int RemainFoodCapacity;
}