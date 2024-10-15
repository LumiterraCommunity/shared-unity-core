using UnityEngine;

/// <summary>
/// 家园系统的定义
/// </summary>
public static class HomeDefine
{
    /// <summary>
    /// 家园进度值流逝速度 每秒流逝比例
    /// 为了性能 不要在进度数据组件中update读表
    /// </summary>
    public static float HomeProgressLostSpeed = 0;
    public static bool InitHomeProgressLostSpeed = false;

    public const int COLLECT_RESOURCE_DEATH_TIME = 10;//采集资源死亡时间 ms
    public const float PROGRESS_FULL_ANIM_TIME = 0.7f;//进度满了后的动画时间 需要等待 秒

    public static readonly Vector3 SOIL_SIZE = new(1.2f, 1.2f, 1.2f);  //土地格子大小
    public static readonly Vector3 EMPTY_SIZE = Vector3.one; //空地格子大小

    /// <summary>
    /// 单个格子操作时的向前偏移多少检查下面格子 就是挥插头往前插一点点的距离 老需求 现在暂时不需要了
    /// </summary>
    public const float SINGLE_GRID_OPERATE_OFFSET = 0f;
    /// <summary>
    /// 单个目标采集时的射出检测球体的半径
    /// </summary>
    public const float SEARCH_SINGLE_SPHERE_RADIUS = 0.7f;


    //TODO: pet test 需要解决这个开始自动时间小于宠物攻击间隔 但是回进度问题
    public const float HOME_PROGRESS_ACTION_BACK_PROTECT_TIME = 5000;//家园进度动作回退保护时间 ms 不能太小 因为需要考虑本地预表现回退时回包覆盖的情况

    public const int ACTION_MAX_PROGRESS_PROTECT = 100;//进度动作最大值的保护性值 防止异常报错 正常不会用到

    /// <summary>
    /// 需要消耗道具的动作集合
    /// </summary>
    public const eAction NEED_COST_ITEM_ACTION_MASK = eAction.Sowing | eAction.Manure | eAction.PutAnimalFood;
    /// <summary>
    /// 持续性进度动作集合 比如浇水
    /// </summary>
    public const eAction HOLD_PROGRESS_ACTION_MASK = eAction.Watering | eAction.Appease | eAction.Shearing | eAction.Milking;
    /// <summary>
    /// 分段进度动作集合 比如砍树
    /// </summary>
    public const eAction SEGMENT_PROGRESS_ACTION_MASK = eAction.Mowing | eAction.Mining | eAction.Cut;
    /// <summary>
    /// 支持进度的动作集合
    /// </summary>
    public const eAction PROGRESS_ACTION_MASK = HOLD_PROGRESS_ACTION_MASK | SEGMENT_PROGRESS_ACTION_MASK;
    /// <summary>
    /// 需要计算伤害的动作集合
    /// </summary>
    public const eAction NEED_CALCULATE_DAMAGE_ACTION_MASK = SEGMENT_PROGRESS_ACTION_MASK ^ (0);
    /// <summary>
    /// 采集资源的动作集合
    /// </summary>
    public const eAction COLLECT_RESOURCE_ACTION_MASK = eAction.Mowing | eAction.Cut | eAction.Mining;
    /// <summary>
    /// 走收获动画的那种特殊动作集合
    /// </summary>
    public const eAction HARVEST_ANIMAL_ACTION_MASK = eAction.Harvest | eAction.Pick;

    #region 宠物

    public const string ANIMAL_AI_NAME = "HomeAnimalAI";
    public const string PET_AI_NAME = "PetAI";

    /// <summary>
    /// 动物找吃的距离食盆的距离
    /// </summary>
    public const float ANIMAL_EAT_FOOD_DISTANCE = 2f;

    /// <summary>
    /// 畜牧动物死亡后能触发的对话的最大距离
    /// </summary>
    public const int ANIMAL_DEATH_DIALOG_TRIGGER_DISTANCE = 3;
    /// <summary>
    /// 动物死亡对话的对话名
    /// </summary>
    public const string ANIMAL_DEATH_DIALOG_CONVERSATION_NAME = "DeathDialog";
    /// <summary>
    /// 动物收获进度最大值的表现单位 可以是10000 也可以是1
    /// </summary>
    public const int ANIMAL_HARVEST_PROCESS_MAX_UNIT = 10000;
    /// <summary>
    /// 动物离主角多近时可以显示状态图标
    /// </summary>
    public const int ANIMAL_SHOW_STATUS_ICON_DISTANCE = 20;
    /// <summary>
    /// 动物离主角多近时可以显示头顶详细信息
    /// </summary>
    public const int ANIMAL_DETAIL_INFO_SHOW_DISTANCE = 3;

    /// <summary>
    /// 宠物采集资源时检测主人附近多少距离 米
    /// </summary>
    public const float PET_COLLECT_RESOURCE_CHECK_RADIUS = 15;


    #endregion

    /// <summary>
    /// 土地状态间的数据定义key
    /// </summary>
    public static class SoilStatusDataName
    {
        public const string IS_INIT_STATUS = "IS_INIT_STATUS";//标记是否是初始化状态
    }

    /// <summary>
    /// 土壤状态标记 需要和土地状态机每个状态对应
    /// </summary>
    public enum eSoilStatus
    {
        /// <summary>
        ///闲置空白
        /// </summary>
        Idle = 0,
        /// <summary>
        /// 松土完
        /// </summary>
        Loose = 1 << 0,
        /// <summary>
        /// 已播种干涸
        /// </summary>
        SeedThirsty = 1 << 1,
        /// <summary>
        /// 已播种已湿润
        /// </summary>
        SeedWet = 1 << 2,
        /// <summary>
        /// 生长已干涸
        /// </summary>
        GrowingThirsty = 1 << 3,
        /// <summary>
        /// 生长中已湿润
        /// </summary>
        GrowingWet = 1 << 4,
        /// <summary>
        /// 干枯
        /// </summary>
        Withered = 1 << 5,
        /// <summary>
        /// 成熟 (普通种子可以开始收获)
        /// </summary>
        Ripe = 1 << 6,
        /// <summary>
        /// 腐败的成熟了 不会有产出
        /// </summary>
        RipePerish = 1 << 7,
        /// <summary>
        /// 特殊功能状态 比如图腾等成熟后所在状态
        /// </summary>
        SpecialFunction = 1 << 8,
        /// <summary>
        /// 可收获状态
        /// </summary>
        Harvest = 1 << 9,
    }

    /// <summary>
    /// 家园系统的玩家动作
    /// </summary>
    public enum eAction
    {
        None = 0,
        /// <summary>
        /// 锄头锄地
        /// </summary>
        Hoeing = 1 << 1,
        /// <summary>
        /// 播种
        /// </summary>
        Sowing = 1 << 2,
        /// <summary>
        /// 浇水（有线形进度）
        /// </summary>
        Watering = 1 << 3,
        /// <summary>
        /// 施肥
        /// </summary>
        Manure = 1 << 4,
        /// <summary>
        /// 割草
        /// </summary>
        Mowing = 1 << 5,
        /// <summary>
        /// 斧头砍树（有进度）
        /// </summary>
        Cut = 1 << 6,
        /// <summary>
        /// 镐子挖矿（有进度）
        /// </summary>
        Mining = 1 << 7,
        /// <summary>
        /// 铲除植物
        /// </summary>
        Eradicate = 1 << 8,
        /// <summary>
        /// 放置动物食物
        /// </summary>
        PutAnimalFood = 1 << 9,
        /// <summary>
        /// 安抚动物
        /// </summary>
        Appease = 1 << 10,
        /// <summary>
        /// 剪毛
        /// </summary>
        Shearing = 1 << 11,
        /// <summary>
        /// 挤奶
        /// </summary>
        Milking = 1 << 12,
        /// <summary>
        /// 作物收获
        /// </summary>
        Harvest = 1 << 15,
        /// <summary>
        /// 捡东西
        /// </summary>
        Pick = 1 << 16,

        /// <summary>
        /// 攻击敌人 怪物 boss（这个给伤害计算分类用的 家园并不使用）
        /// </summary>
        AttackEnemy = 1 << 31,

    }

    /// <summary>
    /// 可采集资源类型 对应不同武器可操作不同类型
    /// </summary>
    public enum eResourceType
    {
        Unknown = 0,
        /// <summary>
        /// 土壤作物
        /// </summary>
        Soil = 1 << 0,
        /// <summary>
        /// 采集物
        /// </summary>
        HomeResource = 1 << 1,
        /// <summary>
        /// 动物食盆
        /// </summary>
        AnimalBowl = 1 << 2,
        /// <summary>
        /// 动物
        /// </summary>
        Animal = 1 << 3,
    }

    public enum eHomeResourcesAreaType : int
    {
        empty,    //空地
        farmland, //农田
        towerLevel, //爬塔关卡
    }

    /// <summary>
    /// 家园类型
    /// </summary>
    public enum eHomeType
    {
        Unknown = 0,
        /// <summary>
        /// 自己个人家园
        /// </summary>
        Personal = 1,
        /// <summary>
        /// 副本家园
        /// </summary>
        Instancing = 2,
    }

    /// <summary>
    /// 无效的种子实体id
    /// </summary>
    public static long INVALID_SEED_ENTITY_ID = 0;
    /// <summary>
    /// 副本去执行动作的特殊玩家ID
    /// </summary>
    public static long INSTANCING_PLAYER = -9001;
    /// <summary>
    /// 副本去执行动作的特殊实体ID
    /// </summary>
    public static long INSTANCING_ENTITY = -9009001;
    /// <summary>
    /// 副本去播种的特殊技能ID
    /// </summary>
    public static int INSTANCING_SOWING_SKILL = -9001;
    /// <summary>
    /// 副本去播种的特殊种子NFT ID
    /// </summary>
    public static string INSTANCING_SOWING_NFT_ID = "instance seed";
}