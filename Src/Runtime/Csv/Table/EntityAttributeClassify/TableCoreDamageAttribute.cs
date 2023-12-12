/// <summary>
/// 核心伤害的某类行为的属性集合 比如砍树 采矿 打怪
/// </summary>
public class TableCoreDamageAttribute : TableSingleAttackAttribute
{
    /// <summary>
    /// 伤害加成
    /// </summary>
    public eAttributeType DmgBonus;
    /// <summary>
    /// 暴击率
    /// </summary>
    public eAttributeType CritRate;
    /// <summary>
    /// 暴击伤害
    /// </summary>
    public eAttributeType CritDmg;
    /// <summary>
    /// 防御力
    /// </summary>
    public eAttributeType Def;
}