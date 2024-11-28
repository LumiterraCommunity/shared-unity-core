using System.Collections.Generic;
using GameMessageCore;
/// <summary>
/// 共享库的专精相关定义
/// </summary>
public static class TalentDefineCore
{
    /// <summary>
    /// 天赋类型对应的攻击目标
    /// </summary>
    public static readonly Dictionary<eTalentType, EntityType> Talent2AttackTarget = new()
    {
        {eTalentType.general, EntityType.Monster},
        {eTalentType.farming, EntityType.HomePlant},
        {eTalentType.battle, EntityType.Monster},
        {eTalentType.gather, EntityType.Resource},
    };
}