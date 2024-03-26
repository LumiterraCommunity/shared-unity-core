/*
 * @Author: xiang huan
 * @Date: 2022-07-19 10:51:41
 * @Description: 战斗公共定义
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/BattleDefine.cs
 * 
 */

using System.Collections.Generic;
using GameMessageCore;

public static class BattleDefine
{
    /// <summary>
    /// 检测技能命中最大碰撞体数量 为了GC
    /// </summary>
    public const int SKILL_CHECK_HIT_MAX_COLLIDER_NUM = 50;

    /// <summary>
    /// 地形伤害实体ID 包括了毒圈 掉落 溺水等 为了给统一的实体伤害事件处理
    /// </summary>
    public const long SCENE_DAMAGE_ENTITY_ID = -999;
    /// <summary>
    /// 家园饥饿伤害实体ID
    /// </summary>
    public const long HUNGER_DAMAGE_ENTITY_ID = -899;

    public enum eSkillShapeId : int
    {
        IdUnknown = 0,
        SkillShapeBox = 1,
        SkillShapeSphere = 2,
        SkillShapeCapsule = 3,
        SkillShapeFan = 4,
    }
    public enum eBattleState
    {
        Invincible,  //无敌效果（攻击全miss）
        Endure,     //霸体效果（释放技能不可打断）  
        Stun,           //眩晕——不响应任何操作
        Root,           //缠绕——又称定身——目标不响应移动请求，但是可以执行某些操作，如施放某些技能
        Silence,        //沉默——目标禁止施放技能
        Invisible,      //隐身——不可被其他人看见
    }

    // public static readonly Dictionary<EntityProfileField, string> ProfileFieldDict = new()
    //     {
    //         {EntityProfileField.Lv, "Lv"},
    //         {EntityProfileField.Exp, "Exp"},
    //         {EntityProfileField.Att, "Att"},
    //         {EntityProfileField.AttSpeed, "AttSpeed"},
    //         {EntityProfileField.Def, "Def"},
    //         {EntityProfileField.HpLimit, "HpLimit"},
    //         {EntityProfileField.CritRate, "CritRate"},
    //         {EntityProfileField.CritDamage, "CritDmg"},
    //         {EntityProfileField.HitRate, "HitRate"},
    //         {EntityProfileField.MissRate, "MissRate"},
    //         {EntityProfileField.MoveSpeed, "MoveSpeed"},
    //         {EntityProfileField.PushDmg, "PushDmg"},
    //         {EntityProfileField.PushDist, "PushDist"},
    //         {EntityProfileField.HpCurrent, "HpCurrent"},
    //         {EntityProfileField.HpRecovery, "HpRecovery"},
    //     };

    public const int SKILL_USE_TAG = (int)eSkillType.General | (int)eSkillType.Channel | (int)eSkillType.Toggle;
    public const int SKILL_ID_NULL = -1;
    public const int QUICK_USE_SKILL_TIME_SCALE = 2;
    public const int USE_SKILL_TIME_SCALE = 1;
    public static eBattleState[] BATTLE_STATE_CANNOT_MOVE_LIST = { eBattleState.Stun, eBattleState.Root };
    public static eBattleState[] BATTLE_STATE_CANNOT_SKILL_LIST = { eBattleState.Stun, eBattleState.Silence };
    public const long ENTITY_ID_UNKNOWN = -1;  // 未知实体ID
    public const int ENTITY_HATRED_MAX_RANGE = 99;  // 仇恨最大范围
    public const float MAX_PARABOLA_FLYER_HEIGHT = 2.5f;  // 抛物线子弹最大高度

    public const int PLAYER_DEATH_STATUS_TIME = 60000;  // 玩家死亡状态时间
    public const int PEACE_AREA_ID = 1;  // 和平区域ID

    public static readonly Dictionary<eEntityCampType, HashSet<eEntityCampType>> EntityCampFriend = new()
    {
        {eEntityCampType.Monster, new HashSet<eEntityCampType> {eEntityCampType.Monster}},
        {eEntityCampType.Player, new HashSet<eEntityCampType> {eEntityCampType.Player}},
        {eEntityCampType.PlayerPVP, new HashSet<eEntityCampType> {}},
    };

    public static readonly Dictionary<eEntityCampType, HashSet<eEntityCampType>> EntityCampEnemy = new()
    {
        {eEntityCampType.Monster, new HashSet<eEntityCampType> {eEntityCampType.Player, eEntityCampType.PlayerPVP}},
        {eEntityCampType.Player, new HashSet<eEntityCampType> {eEntityCampType.Monster, eEntityCampType.PlayerPVP}},
        {eEntityCampType.PlayerPVP, new HashSet<eEntityCampType> {eEntityCampType.Monster, eEntityCampType.Player, eEntityCampType.PlayerPVP}},
    };

    public static readonly HashSet<eEntityCampType> PlayerCampList = new()
    {
        eEntityCampType.Player,
        eEntityCampType.PlayerPVP,
    };

}
public enum eEntityCDType : int
{
    Skill,  //技能
    Extend, //扩展
}
public enum eEntityExtendCDType : int
{
    Revive = 0,  //复活
}

public enum eSkillType : int
{
    Passive = 1 << 1,  //被动
    General = 1 << 2,  //主动
    Channel = 1 << 3, //主动持续释放，预留
    Toggle = 1 << 4,  //开关，预留
}
public enum eSkillTargetFlag : int
{
    NotTarget = 1 << 1,  //无需目标
    Target = 1 << 2,  //需要目标
    Pos = 1 << 3, //需要位置
}


public enum eSkillTargetType : int
{
    Enemy = 1 << 1,  //敌方目标
    Friend = 1 << 2,  //友方目标
}

public enum eEntityCampType : int
{
    Monster = 1,  //怪物阵营
    Player = 2,   //玩家阵营
    PlayerPVP = 3,  //玩家PVP阵营
}

public enum eBattleAreaType : int
{
    Peace = 1,    //和平区域，无法攻击
    Danger = 2,   //危险区域，敌对阵营可以攻击
    Chaos = 3,    //混乱区域，可以攻击任何阵营
}

//持续伤害效果类型
public enum eDotDamageType : int
{
    Normal = 1,  //普通
    Fixed = 2, //血量固定伤害
    Percent = 3,  //血量百分比伤害
    MaxPercent = 4,  //血量最大百分比伤害
}

public enum eSearchTargetType : int
{
    RangeTarget = 1,    //范围内目标
    RangeRandomPos = 2,  //范围内随机位置
    CaptureTarget = 3,  //捕获目标
}