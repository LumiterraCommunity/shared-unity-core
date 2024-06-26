using System;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 动物数据 只有家园畜牧的相关数据
/// </summary>
public class AnimalDataCore : EntityBaseComponent
{
    [SerializeField]
    private AnimalSaveData _saveData;
    /// <summary>
    /// 动物存档数据
    /// </summary>
    /// <value></value>
    public AnimalSaveData SaveData => _saveData;

    /// <summary>
    /// 宠物配置表 不要放开给外面用 外面用的话统一请用PetDataCore里的 这里只是为了效率自己引用一份
    /// </summary>
    private DRPet _petCfg;

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
    public bool IsHappyValid => _saveData.Happiness > 0 && _saveData.Happiness >= _petCfg.RequiredHappiness;

    /// <summary>
    /// 好感度改变事件 T0:更改后好感度
    /// </summary>
    public Action<int> MsgFavorabilityChanged;

    protected virtual void Start()
    {
        RefEntity.EntityEvent.OnPetHungerChanged += OnPetHungerChanged;
    }

    protected virtual void OnDestroy()
    {
        RefEntity.EntityEvent.OnPetHungerChanged -= OnPetHungerChanged;
    }

    private void OnPetHungerChanged(float hungerValue, bool force)
    {
        if (force)
        {
            return;
        }

        if (hungerValue > 0)//不饿
        {
            if (SaveData.LastCompleteHungerStamp > 0)
            {
                SaveData.LastCompleteHungerStamp = 0;
            }
        }
        else//饥饿了
        {
            if (SaveData.LastCompleteHungerStamp <= 0)
            {
                SaveData.LastCompleteHungerStamp = TimeUtil.GetServerTimeStamp();
            }
        }
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="animalSaveData">如果是null代表没有 会自动创建一个</param>
    public void InitData(int petCfgId, AnimalSaveData animalSaveData)
    {
        _petCfg = GFEntryCore.DataTable.GetDataTable<DRPet>().GetDataRow(petCfgId);
        if (_petCfg == null)
        {
            Log.Error($"AnimalDataCore InitData Can not find pet cfg id:{petCfgId}");
            return;
        }

        long entityId = RefEntity.BaseData.Id;
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
            _saveData = new AnimalSaveData(entityId);
        }

        SetHappiness(_saveData.Happiness);
    }

    /// <summary>
    /// 设置抚摸成熟度 一次抚摸只能设置一次 不能重复设置
    /// </summary>
    /// <param name="proficiency"></param>
    internal void SetProficiency(int proficiency)
    {
        if (proficiency != 0 && SaveData.Proficiency > 0)
        {
            Log.Error($"SetProficiency 重复设置熟练度 id:{RefEntity.BaseData.Id} Proficiency:{SaveData.Proficiency} new:{proficiency}");
        }

        SaveData.Proficiency = Mathf.Max(proficiency, 0);
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
            int remainHappy = SaveData.Happiness - _petCfg.RequiredHappiness;
            remainHappy = Mathf.Max(remainHappy, 1);
            HarvestMaxTime = (float)_petCfg.BreedingDifficulty / remainHappy * TableUtil.GetGameValue(eGameValueID.AnimalHarvestTimeRate).Value;
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
        SetProficiency(0);
        SaveData.SetHarvestProgress(0);
        SaveData.IsComforted = false;
    }
}