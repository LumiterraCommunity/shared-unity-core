/// <summary>
/// 宠物能力
/// </summary>
public enum ePetAbility : int
{
    None = 0,  //无
    Battle = 1 << 0,  //战斗
    Gather = 1 << 1,  //采集
    Farming = 1 << 2,  //农场
    Rescue = 1 << 3,  //救援
    Riding = 1 << 4,  //骑乘
    SkillExtend = 1 << 5,  //技能扩展属性
}