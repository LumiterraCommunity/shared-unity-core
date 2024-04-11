using System.Collections.Generic;
using GameMessageCore;
/*
* @Author: mangit
 * @LastEditors: Please set LastEditors
* @Description: 表定义
* @Date: 2022-06-23 20:28:37
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Csv/TableDefine.cs
*/
public static class TableDefine
{
    /// <summary>
    /// 配置中的千分位配置变小数
    /// </summary>
    public static readonly float THOUSANDTH_2_FLOAT = 1 / 1000f;
    /// <summary>
    /// 配置中的百分位配置变小数
    /// </summary>
    public static readonly float PERCENTAGE_2_FLOAT = 1 / 100f;
    public static readonly int ITEMID_EXP_FARMING = 188;
    public static readonly int ITEMID_EXP_COMBAT = 189;
    public static readonly int ITEMID_EXP_GATHER = 190;
    public static readonly int ITEMID_BATTERY = 21;
    public static readonly int ITEMID_SEED = 22;
    public const int DATA_TABLE_START_ROW = 1;
    public const string DATA_TABLE_ENCODING = "UTF-8";

    public const int TEMPLATE_SKILL_ID = 14; // 模版技能ID
    public const int DAMAGE_EFFECT_ID = 15;  // 基础伤害效果ID

    public const int ENTITY_CID_NULL = -1; // 空实体配置ID
    public const int TOKEN_DECIMALS = 18; // 代币小数位数

    public const int DECIMALS_INT = 10; // 链上通用整数小数位数 

    public static Dictionary<TalentType, int> TalentType2ItemIdDic = new(){
        {TalentType.Farming, ITEMID_EXP_FARMING},
        {TalentType.Battle, ITEMID_EXP_COMBAT},
        {TalentType.Gather, ITEMID_EXP_GATHER},
    };
}

public enum eRoleID
{
    male = 1,
}

public enum eGameValueID
{
    animalMaxNum = 8,
    captureFavoriteItemRate = 9, // 捕获怪物喜欢的item倍率加成
    captureBaseDamage = 10,      // 捕获怪物基础伤害值
    hoeingMaxActionValue = 13,
    animalAppeaseMaxActionValue = 14,
    animalHarvestMaxActionValue = 15,
    homeActionLostSpeed = 16,
    soilFromLooseToIdleTime = 17,
    animalDeadTimeFromHunger = 18,
    animalHarvestActionValue = 19,
    animalAppeaseActionSpeed = 20,
    animalFavorabilityValueEveryHeart = 21,
    animalAddFavorabilityEveryAppease = 22,
    animalHeartShowMaxNum = 23,
    captureRopeMaxLen = 24,
    captureTrapTime = 25,
    SoilGrowTimeRate = 26,
    AnimalHarvestTimeRate = 27,
    TeamMemberMaxNum = 28,//最大队伍成员
    EnergyDropCostRate = 31,//精力掉落扣除系数 千分位
    EnergyRecoverSpeed = 32,//精力每分钟恢复量 千分位 和属性表中一致
    TeamDropRadius = 33, // 队伍掉落有效半径
    TeamExpRatio = 34, // 队伍经验分成比例
    TeammateHarvestGetDropRate = 35,
    EnterDungeonRatio = 36, // 传送进副本时队员离NPC或队长最大半径(单位m)
    InstancingDailyRewardCount = 37, // 副本每日奖励次数
    InstancingReadyTimeOut = 38, // 副本准备超时时间
    DailyTaskReceiveCD = 39, // 日常任务接取CD
    PVPDamageRate = 43,//PVP伤害系数
    PlayerCampChangeTime = 44, //玩家切换阵营时间
    HatchPetEggProtectTime = 45,//孵化宠物蛋保护时间，防止用户频繁取消孵化,减轻服务器压力
    PetCastSkillHungerCost = 46,//宠物释放技能饥饿度消耗
    PetProduceHungerRate = 47,//生产时的宠物饥饿度消耗倍率
    KillCostEnergy = 50,//击杀行为消耗精力 千分位
    PetAbilityDisplays = 52,//宠物特性对外名称显示
    offlineTimeout = 53, // 离线超时时间
    InstancingRewardsRateRange = 55, // 副本奖励倍率范围
}

// public static class GameValueID
// {
//     /// <summary>
//     /// 角色最大等级配置
//     /// </summary>
//     public const int ROLE_MAX_LV = 1;
//     /// <summary>
//     /// 角色插槽最大等级配置
//     /// </summary>
//     public const int SLOT_MAX_LV = 2;
//     /// <summary>
//     /// 技能最大等级配置
//     /// </summary>
//     public const int CRAFT_SKILL_MAX_LV = 3;
//     /// <summary>
//     /// 角色等级与插槽等级正负差距
//     /// </summary>
//     public const int MAX_LV_GAP_BETWEEN_SLOT_AND_ROLE = 4;
//     /// <summary>
//     /// 升级时不小于角色等级5级的插槽数量限制
//     /// </summary>
//     public const int COUNT_OF_VALID_SLOT_LV_TO_UPGRADE_ROLE = 5;
// }

/// <summary>
/// 语言表id
/// </summary>
public static class LanguageTableID
{
    public const int ACQUIRE_EXP_TIPS = 10060001;
    public const int ACQUIRE_LNCO_TIPS = 10060002;
    public const int ACQUIRE_SKILL_PROFICIENCY_TIPS = 10060003;
}

/// <summary>
/// 对应item表中的type字段
/// </summary>
public enum ePropItemType
{
    None = 0,
    Equipment = 1,
    Consumable = 3,
    Material = 4,
    Building = 5,
}

/// <summary>
/// 对应equipment表中的gearType
/// </summary>
public enum eEquipmentType
{
    HeadArmor = 1,
    ChestArmor,
    LegsArmor,
    FeetArmor,
    HandsArmor,
    Axe,
    PickAxe,
    Sword,
    Bow,
    Decorations,
    SingleGun,
    DoubleGun,
    Dagger,
    Spear,
}

//消耗品使用交互类型
public enum eFoodItemInteractType
{
    usePotion,//嗑药
    occupyLand,//占地
    equipItem,//装备道具
    releaseSkill,//释放技能
}

public enum eSkillEffectType : int
{
    EffectIdUnknown = 0,
    SENormalDamage = 1,
    SEPathMove = 2,
    SEBeHitPathMove = 3,
    SEInvincible = 4,
    SEEndure = 5,
    SELockEnemyPathMoveCore = 6,
    SEAttributeModifierCore = 7,
    SEStun = 8,
    SEDotDamage = 9,
    SECollisionTrigger = 10,
    SETriggerQuickCastSkill = 11,
    SESkillRangeTrigger = 12,
    SEBloodRage = 13,
    SEUnharmedAddAttr = 14,
    SESkillEffectModifier = 15,
    SECaptureRopeHit = 16,
    SECaptureDamage = 17,
    SELayerTrigger = 31,
    SEEntityReborn = 33,
    SEAddEntity = 34,
    SEAura = 35,
}

public enum eSEFuncType : int
{
    None = 0,
    SEFuncAddDamage = 1,
    SEFunApplySkillEffect = 2,
    SEFuncReset = 99,

}
/// <summary>
/// 技能效果作用类型
/// </summary>
public enum eSkillEffectApplyType : int
{
    Init = 0, //初始阶段
    Forward = 1, //前置阶段
    CastSelf = 2, //对自己释放
    CastEnemy = 3, //对敌人释放
}
/// <summary>
/// 技能效果修改类型
/// </summary>
public enum eSkillEffectModifierType : int
{
    Replace = 0, //替换
    Add = 1, //增加
    Remove = 2, //移除
}

public enum eSkillId : int
{
    Capture = 166, //捕获
    CaptureShoot = 168, //捕获-> 射怪
}