using System;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 动物数据
/// </summary>
public class AnimalDataCore : EntityBaseComponent
{
    /// <summary>
    /// 动物Id 家园系统中的ID和数据管理Id一致
    /// </summary>
    public ulong AnimalId => _saveData.AnimalId;
    //TODO: pet 这两个表需要删掉 直接使用PetDataCore中的数据
    /// <summary>
    /// 怪物配置表
    /// </summary>
    /// <value></value>
    public DRMonster DRMonster { get; private set; }
    /// <summary>
    /// 宠物配置表,宠物都是从怪物表中转化过来的，基础属性读取怪物表，宠物特有属性读取宠物表
    /// </summary>
    public DRPet DRPet { get; private set; }
    [SerializeField]
    private AnimalSaveData _saveData;
    /// <summary>
    /// 动物存档数据
    /// </summary>
    /// <value></value>
    public AnimalSaveData SaveData => _saveData;

    /// <summary>
    /// 动物收获最大时间 秒
    /// </summary>
    public float HarvestMaxTime;

    /// <summary>
    /// 是否能收获
    /// </summary>
    public bool IsCanHarvest => _saveData.HarvestProgress >= HomeDefine.ANIMAL_HARVEST_PROCESS_MAX_UNIT;

    /// <summary>
    /// 幸福值是否有效
    /// </summary>
    public bool IsHappyValid => _saveData.Happiness > 0 && _saveData.Happiness >= DRPet.RequiredHappiness;

    /// <summary>
    /// 是否饥饿状态
    /// </summary>
    public bool IsHunger => SaveData.HungerProgress <= 0;

    /// <summary>
    /// 好感度改变事件 T0:更改后好感度
    /// </summary>
    public Action<int> MsgFavorabilityChanged;

    public void SetPetCid(int petCid)
    {
        DRMonster = GFEntryCore.DataTable.GetDataTable<DRMonster>().GetDataRow(petCid);
        DRPet = GFEntryCore.DataTable.GetDataTable<DRPet>().GetDataRow(petCid);
        if (DRMonster == null || DRPet == null)
        {
            throw new Exception($"配置表中没有找到cid为{petCid}的数据");
        }
    }

    /// <summary>
    /// 初始化设置一个存储数据 如果是null代表没有 会自动创建一个
    /// </summary>
    /// <param name="animalSaveData"></param>
    public void SetSaveData(AnimalSaveData animalSaveData)
    {
        ulong entityId = (ulong)RefEntity.BaseData.Id;
        if (animalSaveData != null)
        {
            if (animalSaveData.ProductSaveData != null && animalSaveData.ProductSaveData.ProductId == 0)
            {
                Log.Error($"动物存档数据中的产品数据有误 id==0 Product:{animalSaveData.ProductSaveData}");
                animalSaveData.ProductSaveData = null;
            }

            _saveData = animalSaveData;
            if (entityId != _saveData.AnimalId)
            {
                Log.Error($"动物数据和存档数据不一致 _baseData.AnimalId:{RefEntity.BaseData.Id} _saveData.AnimalId:{_saveData.AnimalId}");
                _saveData.AnimalId = entityId;
            }
        }
        else
        {
            _saveData = new AnimalSaveData(entityId)
            {
                HungerProgress = DRPet.MaxHunger
            };
        }

        SetHappiness(_saveData.Happiness);
    }

    /// <summary>
    /// 设置动物快乐值 如果之前有的话 会退回给系统
    /// </summary>
    /// <param name="happiness"></param>
    internal void SetHappiness(int happiness)
    {
        SaveData.Happiness = happiness;
        MessageCore.HomeOneAnimalUsedHappyChanged.Invoke(SaveData.AnimalId);

        //当前进度比例不变 但是最大值变了 后面的进度根据新的最大值来计算
        UpdateMaxHarvestTime();
    }

    private void UpdateMaxHarvestTime()
    {
        if (IsHappyValid)
        {
            int remainHappy = SaveData.Happiness - DRPet.RequiredHappiness;
            remainHappy = Mathf.Max(remainHappy, 1);
            HarvestMaxTime = (float)DRPet.BreedingDifficulty / remainHappy * TableUtil.GetGameValue(eGameValueID.AnimalHarvestTimeRate).Value;
        }
        else
        {
            HarvestMaxTime = 0;
        }
    }

    /// <summary>
    /// 收获后需要清理的数据
    /// </summary>
    public void ClearDataAfterHarvest()
    {
        SetHappiness(0);
        SaveData.SetHarvestProgress(0);
        SaveData.IsComforted = false;
    }

    /// <summary>
    /// 获取下一次收获的时间 秒 无效时返回-1
    /// </summary>
    /// <returns></returns>
    public float GetNextHarvestTime()
    {
        if (IsHunger)
        {
            return -1f;
        }

        if (!IsHappyValid)
        {
            return -1f;
        }

        float remainProgress = 1 - (SaveData.HarvestProgress / HomeDefine.ANIMAL_HARVEST_PROCESS_MAX_UNIT);
        return Mathf.Max(HarvestMaxTime * remainProgress, 0);
    }
}