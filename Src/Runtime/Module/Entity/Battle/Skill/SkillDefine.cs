public class SkillDefine
{
}

public enum eSkillFlyerType : int
{
    Follow = 1,       //锁定子弹
    Rope = 2,         //绳子子弹
    FollowRope = 3,   //锁定绳子子弹
    Quick = 4,        //快速子弹
    Parabola = 5,     //抛物线子弹
}

/// <summary>
/// 技能组类型
/// </summary>
public enum eSkillGroupType : int
{
    Base = 1,       // 基础
    Equipment = 2,  // 装备
    Jump = 3,       // 跳跃
    Rescue = 4,     // 救援
    Harvest = 5,    // 收获
    Talent = 6,     // 天赋技能
    InHandItem = 7,     // 手持道具技能组
    Extend = 8,     // 扩展技能组
    Pet = 9,        // 宠物技能组
    Item = 10,           //道具技能组
    Action = 11,         //动作技能组
    HomeSkill = 12,      //家园技能组
    Common = 13,         //通用技能组
}

public enum eSEStatusType
{
    Runtime,
    Static, //静态效果，不会进行刷新。节省性能开销
    StaticUpdate, //静态效果，但是需要刷新
}