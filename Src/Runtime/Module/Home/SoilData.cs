using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 单块土地上的数据
/// </summary>
public class SoilData : MonoBehaviour
{
    /// <summary>
    /// 属性数据 目前只有有种子时才不为空 为了节省数据和通信 将来如果需要播种前就有属性加成时可以直接初始化土地添加
    /// </summary>
    /// <value></value>
    public SoilAttributeData AttributeData { get; private set; }

    [SerializeField]
    private SoilSaveData _saveData;
    /// <summary>
    /// 能存档的数据
    /// </summary>
    /// <value></value>
    public SoilSaveData SaveData => _saveData;
    /// <summary>
    /// 当前土地的种子配置 可能为空 只有真正有效才有值
    /// </summary>
    /// <value></value>
    public DRSeed DRSeed { get; private set; }

    /// <summary>
    /// 种子生长阶段数量 无效时为0 可以像数组的长度一样使用 种子当前从0开始 到了SeedGrowStageNum就需要收获了
    /// </summary>
    public int SeedGrowStageNum => DRSeed == null ? 0 : DRSeed.GrowRes.Length;
    /// <summary>
    /// 种子每个生长阶段的时间 无效时为0 秒
    /// </summary>
    public float SeedEveryGrowStageTime
    {
        get
        {
            if (SeedGrowStageNum == 0)
            {
                return 0;
            }

            float remainFertile = SaveData.Fertile - GetAttribute(eAttributeType.RequiredFertilizer);
            remainFertile = Mathf.Max(remainFertile, 1);
            float totalGrowTime = GetAttribute(eAttributeType.PlantingDifficulty) / remainFertile * TableUtil.GetGameValue(eGameValueID.SoilGrowTimeRate).Value;
            return totalGrowTime / SeedGrowStageNum;
        }
    }

    private void Awake()
    {
        _saveData = new SoilSaveData();
    }

    /// <summary>
    /// 获取某个属性的值 只有有种子时才有效 否则返回-1
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public float GetAttribute(eAttributeType type)
    {
        if (AttributeData == null)
        {
            Log.Error($"土地上没有种子属性,seed cid={SaveData.SeedData.SeedCid}");
            return -1;
        }

        return AttributeData.GetRealValue(type);
    }

    /// <summary>
    /// 初始种子属性
    /// </summary>
    private void InitSeedAttribute()
    {
        if (AttributeData != null)
        {
            Log.Error("土地上已经有属性了");
            ClearSeedAttribute();
        }

        if (DRSeed == null)
        {
            Log.Error("没有种子配置");
            return;
        }

        AttributeData = gameObject.AddComponent<SoilAttributeData>();
        AttributeData.SetBaseValue(eAttributeType.FarmingLv, SaveData.SeedData.Lv);
        AttributeData.SetBaseValue(eAttributeType.FarmPotentiality, SaveData.SeedData.Potentiality);

        float potentiality = AttributeData.GetRealValue(eAttributeType.FarmPotentiality);
        TableUtil.SetTableInitAttribute(AttributeData, DRSeed.InitialAttribute, SaveData.SeedData.Lv, potentiality);
    }

    /// <summary>
    /// 清理种子属性
    /// </summary>
    private void ClearSeedAttribute()
    {
        if (AttributeData == null)
        {
            return;
        }

        Destroy(AttributeData);
        AttributeData = null;
    }

    internal void SetId(ulong id)
    {
        SaveData.Id = id;
    }

    /// <summary>
    /// 整个保存数据直接设置 往往是初始化的时候
    /// </summary>
    /// <param name="saveData"></param>
    internal void SetSaveData(SoilSaveData saveData)
    {
        ClearSeedData();

        _saveData = saveData;

        if (saveData.SeedData.SeedCid > 0)
        {
            DRSeed = GFEntryCore.DataTable.GetDataTable<DRSeed>().GetDataRow(saveData.SeedData.SeedCid);
            if (DRSeed == null)
            {
                Log.Error($"初始化土地数据时种子配置表里没有找到cid为 {saveData.SeedData.SeedCid} 的种子");
            }
            else
            {
                InitSeedAttribute();
            }
        }
    }

    /// <summary>
    /// 放置一个种子 往往是播种后调用这里
    /// </summary>
    /// <param name="seedCid">0代表清除</param>
    /// <param name="seedNftId">种子nftId</param>
    /// <param name="seedEntityId">种子实体id 一般是0 只有特殊种子 还是播种就由数据服分配了id的才有值</param>
    /// <param name="seedLv">种子等级</param>
    /// <param name="seedPotentiality">种子潜力</param>
    internal void PutSeed(int seedCid, string seedNftId, long seedEntityId, int seedLv, int seedPotentiality)
    {
        if (SaveData.SeedData.SeedCid > 0)
        {
            Log.Error($"土地上已经有种子了 soilId:{SaveData.Id} oldSeedCid:{SaveData.SeedData.SeedCid} newSeedCid:{seedCid}");
            throw new System.Exception("土地上已经有种子了");//抛出异常 玩家请求那边会处理异常时不扣除种子
        }

        SaveData.SeedData.SeedCid = seedCid;
        SaveData.SeedData.SeedNftId = seedNftId;
        SaveData.SeedData.SeedEntityId = seedEntityId;
        SaveData.SeedData.Lv = seedLv;
        SaveData.SeedData.Potentiality = seedPotentiality;

        DRSeed = GFEntryCore.DataTable.GetDataTable<DRSeed>().GetDataRow(seedCid);
        if (DRSeed == null)
        {
            Log.Error($"种子配置表里没有找到cid为 {seedCid} 的种子");
        }
        else
        {
            InitSeedAttribute();
        }
        SetGrowStage(0);
    }

    /// <summary>
    /// 清理所有数据到默认值
    /// </summary>
    internal void ClearAllData()
    {
        DRSeed = null;
        _saveData = new SoilSaveData() { Id = _saveData.Id };//防止数据没清干净
    }

    /// <summary>
    /// 清理种子数据到默认值
    /// </summary>
    internal void ClearSeedData()
    {
        if (!_saveData.SeedData.HaveSeed())
        {
            return;
        }

        DRSeed = null;
        _saveData.ClearSeedData();

        ClearSeedAttribute();
    }

    /// <summary>
    /// 设置当前种子的成长阶段 从0开始 最大不能超过配置的数量索引
    /// </summary>
    /// <param name="growStage"></param>
    internal void SetGrowStage(int growStage)
    {
        if (SaveData.SeedData.GrowingStage == growStage)
        {
            return;
        }

        if (growStage >= SeedGrowStageNum)
        {
            Log.Error($"土地的成长阶段设置错误 :{growStage} cfgNum:{SeedGrowStageNum}");
            return;
        }

        SaveData.SeedData.GrowingStage = growStage;
    }

    /// <summary>
    /// 设置施肥
    /// </summary>
    /// <param name="manureCid">肥料cid</param>
    internal void SetManure(int manureCid)
    {
        if (SaveData.SeedData.ManureCid > 0)
        {
            Log.Error($"土地已经施肥了 不能再施肥了");
            return;
        }

        if (!GFEntryCore.DataTable.GetDataTable<DRHomeManure>().HasDataRow(manureCid))
        {
            Log.Error($"肥料配置表里没有找到cid为 {manureCid} 的肥料");
            return;
        }

        SaveData.SeedData.ManureCid = manureCid;
    }

    /// <summary>
    /// 设置土地肥沃度 如果之前有的话 会退回给系统
    /// </summary>
    /// <param name="soilFertile"></param>
    internal void SetSoilFertile(int soilFertile)
    {
        if (SaveData.Fertile == soilFertile)
        {
            return;
        }

        SaveData.Fertile = soilFertile;
        MessageCore.HomeOneSoilUsedFertileChanged.Invoke(SaveData.Id);
    }

    /// <summary>
    /// 获取下一次成熟的时间 秒 无效时返回-1
    /// </summary>
    /// <returns></returns>
    public float GetNextRipeTime()
    {
        if (SaveData.SoilStatus is not HomeDefine.eSoilStatus.SeedWet and not HomeDefine.eSoilStatus.GrowingWet)
        {
            return -1f;
        }

        if (SeedEveryGrowStageTime.ApproximatelyEquals(0))
        {
            return -1f;
        }

        int lostTimeMs = (int)(SoilStatusCore.GetNowTimestamp() - SaveData.StatusStartStamp);
        lostTimeMs = Mathf.Max(lostTimeMs, 0);
        float remain = SeedEveryGrowStageTime - (lostTimeMs * TimeUtil.MS2S);
        return Mathf.Max(remain, 0);
    }

    /// <summary>
    /// 设置当前熟练度 内部会保护范围
    /// </summary>
    public void SetCurProficiency(int newProficiency)
    {
        SaveData.SeedData.CurProficiency = Mathf.Clamp(newProficiency, 0, (int)GetAttribute(eAttributeType.RequirementProficiency));
    }

    /// <summary>
    /// 设置是否需要腐败
    /// </summary>
    /// <param name="v"></param>
    public void SetNeedPerish(bool v)
    {
        SaveData.SeedData.NeedPerish = v;
    }
}