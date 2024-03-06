using GameMessageCore;
using UnityEngine;

/// <summary>
/// 动物存档的细节数据 不包含main服的动物管理数据
/// </summary>
[System.Serializable]
public class AnimalSaveData
{
    /// <summary>
    /// 动物在家园系统中的全局ID和数据服那边的ID一致 也是实体ID
    /// </summary>
    public long AnimalId;
    /// <summary>
    /// 饥饿进度 0代表完全饿了 具体值 不是100进度值 有配置最大饥饿度配套
    /// </summary>
    public float HungerProgress = 0;
    /// <summary>
    /// 上次完全饥饿的时间戳 >0说明正在饥饿中
    /// </summary>
    public long LastCompleteHungerStamp;
    /// <summary>
    /// 收获进度 0~ANIMAL_HARVEST_PROCESS_MAX_UNIT 为什么用比例因为新需求设置幸福值保留收获比例 为什么目前用10000 提升保存精度 防止统一截取浮点小数位
    /// </summary>
    public float HarvestProgress = 0;
    /// <summary>
    /// 本收获阶段是否已经安抚过
    /// </summary>
    public bool IsComforted;
    /// <summary>
    /// 动物已经死亡
    /// </summary>
    public bool IsDead;
    /// <summary>
    /// 当前幸福值
    /// </summary>
    public int Happiness;
    /// <summary>
    /// 如果有自动生产的产品在场景中 这里就有数据
    /// </summary>
    public AnimalProductSaveData ProductSaveData;

    public AnimalSaveData()
    {

    }

    public AnimalSaveData(long animalId)
    {
        AnimalId = animalId;
    }

    public AnimalSaveData(ProxyAnimalData data)
    {
        AnimalId = data.AnimalId;
        HungerProgress = data.HungerProgress;
        HarvestProgress = data.HarvestProgress;
        IsComforted = data.IsComforted;
        IsDead = data.IsDead;
        Happiness = data.Happiness;

        ProductSaveData = data.ProductData == null ? null : new AnimalProductSaveData(data.ProductData);
    }

    public ProxyAnimalData ToProxyAnimalData()
    {
        ProxyAnimalData data = new()
        {
            AnimalId = AnimalId,
            HungerProgress = HungerProgress,
            HarvestProgress = HarvestProgress,
            IsComforted = IsComforted,
            IsDead = IsDead,
            Happiness = Happiness,

            ProductData = ProductSaveData?.ToProxyAnimalData()
        };

        return data;
    }

    /// <summary>
    /// 统一设置饥饿进度 0~ANIMAL_HARVEST_PROCESS_MAX_UNIT
    /// </summary>
    public void SetHarvestProgress(float progress)
    {
        HarvestProgress = Mathf.Clamp(progress, 0, HomeDefine.ANIMAL_HARVEST_PROCESS_MAX_UNIT);
    }
}