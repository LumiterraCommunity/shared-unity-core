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
        _soilData.SetNeedPerish(false);//TODO: home 暂定不腐败
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

        if (isWatering)
        {
            _soilData.SetNeedPerish(false);//TODO: home 暂定不腐败
        }
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
}