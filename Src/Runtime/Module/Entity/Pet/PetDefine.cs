using GameMessageCore;


/// <summary>
/// 宠物能力
/// </summary>
public enum ePetAbility : int
{
    None = 0,  //无
    Battle = 1 << PetAbilityType.Battle,  //战斗
    Gather = 1 << PetAbilityType.Gather,  //采集
    Farming = 1 << PetAbilityType.Farming,  //农场
    Rescue = 1 << PetAbilityType.Rescue,  //救援
    Riding = 1 << PetAbilityType.Riding,  //骑乘
    SkillExtend = 1 << PetAbilityType.SkillExtend,  //技能扩展属性
}