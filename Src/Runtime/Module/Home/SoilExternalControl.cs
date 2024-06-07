using UnityEngine;

/// <summary>
/// 土壤上用于外部控制一些状态逻辑的辅助组件 内部控制是通过执行动作正常流转的 其他为外部控制 比如事件
/// </summary>
public class SoilExternalControl : MonoBehaviour
{
    public SoilData _soilData { get; private set; }

    private void Start()
    {
        _soilData = GetComponent<SoilData>();
    }

    /// <summary>
    /// 改变种子生长阶段数据 如果合理 内部会设置好数据 返回新的生长阶段 异常等情况返回false
    /// 外部不要直接调用这里 设置只会改数据 不会变更状态
    /// </summary>
    /// <param name="offsetStage">想要改变的量 内部会保护</param>
    /// <param name="newStage">新的最终生长阶段</param>
    /// <returns></returns>
    internal bool ChangeGrowStage(int offsetStage, out int newStage)
    {
        newStage = _soilData.SaveData.SeedData.GrowingStage + offsetStage;
        newStage = Mathf.Clamp(newStage, 0, _soilData.SeedGrowStageNum - 1);
        int realOffset = newStage - _soilData.SaveData.SeedData.GrowingStage;
        if (realOffset == 0)//没变化
        {
            return false;
        }

        int offsetProficiency = CalculateUnitProficiency() * realOffset;

        _soilData.SetCurProficiency(_soilData.SaveData.SeedData.CurProficiency + offsetProficiency);
        _soilData.SetGrowStage(newStage);
        return true;
    }

    /// <summary>
    /// 改变浇水数据 这里不会判断是否能浇水
    /// </summary>
    /// <param name="isWatering">浇水或者取消浇水</param>
    internal void ChangeWaterData(bool isWatering)
    {
        int offset = isWatering ? 1 : -1;
        _soilData.SetCurProficiency(_soilData.SaveData.SeedData.CurProficiency + offset * CalculateUnitProficiency());
    }

    /// <summary>
    /// 计算出每个阶段需要的熟练度
    /// </summary>
    /// <returns></returns>
    private int CalculateUnitProficiency()
    {
        float allNeedProficiency = _soilData.GetAttribute(eAttributeType.RequirementProficiency);
        return Mathf.CeilToInt(allNeedProficiency / _soilData.SeedGrowStageNum);
    }

    /// <summary>
    /// 判断外部是否能浇水 这里只处理外部控制的额外条件判断 基础能否浇水不在这里判断 这里也不管
    /// eg: 为什么有这个需求 因为计算腐败需要用到玩家属性 所以最后一次浇水只能由玩家来浇水 产品暂时这样定的
    /// </summary>
    /// <returns></returns>
    internal bool JudgeExternalCanWatering()
    {
        if (_soilData.SaveData.SeedData.SeedCid <= 0)
        {
            return false;
        }

        //最后一个阶段只能玩家人工浇水 因为涉及到计算腐败 需要玩家属性
        if (_soilData.SaveData.SeedData.GrowingStage >= _soilData.SeedGrowStageNum - 1)
        {
            return false;
        }

        return true;
    }
}