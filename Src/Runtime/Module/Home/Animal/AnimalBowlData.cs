using UnityEngine;

/// <summary>
/// 畜牧食盆数据
/// </summary>
public class AnimalBowlData : MonoBehaviour
{
    /// <summary>
    /// 是否有食物
    /// </summary>
    public bool IsHaveFood => _saveData != null && _saveData.FoodCid > 0 && _saveData.RemainFoodCapacity > 0;
    /// <summary>
    /// 当前存放的食物配置 可能为null
    /// </summary>
    /// <value></value>
    public DRAnimalFood CurDRFood { get; private set; }
    [SerializeField]
    private AnimalBowlSaveData _saveData;
    /// <summary>
    /// 当前食盆保存的食物数据 如果没有食物则为null
    /// </summary>
    /// <value></value>
    public AnimalBowlSaveData SaveData => _saveData;

    /// <summary>
    /// 设置食物的存档数据
    /// </summary>
    /// <param name="saveData"></param>
    internal void SetSaveData(AnimalBowlSaveData saveData)
    {
        _saveData = saveData ?? new AnimalBowlSaveData();

        CurDRFood = _saveData.FoodCid > 0 ? GFEntryCore.DataTable.GetDataTable<DRAnimalFood>().GetDataRow(_saveData.FoodCid) : null;
    }

    /// <summary>
    /// 更新食物
    /// </summary>
    /// <param name="foodCid"></param>
    /// <param name="remainCapacity"></param>
    public void UpdateFood(int foodCid, int remainCapacity)
    {
        _saveData.FoodCid = foodCid;
        _saveData.RemainFoodCapacity = remainCapacity;
        CurDRFood = _saveData.FoodCid > 0 ? GFEntryCore.DataTable.GetDataTable<DRAnimalFood>().GetDataRow(_saveData.FoodCid) : null;
    }
}