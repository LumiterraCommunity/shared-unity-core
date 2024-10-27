/// <summary>
/// 属于伤害减免的技能效果
/// </summary>
internal interface ISEDamageReduction
{
    /// <summary>
    /// 减免伤害 返回减免后的伤害值
    /// </summary>
    /// <param name="fromEntity">来源实体 不为null</param>
    /// <param name="deltaInt">传入的是负数 代表伤害</param>
    /// <returns></returns>
    int ReductionDamage(EntityBase fromEntity, int deltaInt);
}