/*
 * @Author: xiang huan
 * @Date: 2022-12-02 13:54:41
 * @Description: 区域定义
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Define/SceneAreaDefine.cs
 * 
 */


/// <summary>
/// 场景区域定义
/// </summary>
public enum eSceneArea : int
{
    None,
    World = 10001,  //大世界
    // World2 = 10003,  //大世界
    Home = 10002,   //家园
    DungeonSceneLv1_3 = 20000,  //1-3级副本
    DungeonSceneLv4_6 = 20001,  //4-6级副本
    DungeonSceneLv7_9 = 20002,  //7-9级副本
    PVPDungeonSceneLv1_3 = 21001,  //1-3级PVEP副本
    PVPDungeonSceneLv4_6 = 21002,  //4-6级PVEP副本
    PVPDungeonSceneLv7_9 = 21003,  //7-9级PVEP副本
    PVPDungeonSceneSoloLv1_3 = 22001,  //1-3级PVEP副本
    PVPDungeonSceneSoloLv4_6 = 22002,  //4-6级PVEP副本
    PVPDungeonSceneSoloLv7_9 = 22003,  //7-9级PVEP副本
    PVPDungeonSceneLv1_3_Slight = 23001,  //1-3级PVEP副本
    PVPDungeonSceneLv4_6_Slight = 23002,  //4-6级PVEP副本
    PVPDungeonSceneLv7_9_Slight = 23003,  //7-9级PVEP副本
    DailyDungeon = 30001,  //每日副本
    PVPTrainingDungeon = 30002,  //每日PVEP副本
    GatherDungeon1_3 = 40001,  //采集副本
    GatherDungeon4_6 = 40002,  //采集副本
    GatherDungeon7_9 = 40003,  //采集副本
    FarmingDungeon1_3 = 50001,  //农牧副本
    FarmingDungeon4_6 = 50002,  //农牧副本
    FarmingDungeon7_9 = 50003,  //农牧副本
    TowerDungeon_Combat = 24001,  //农牧副本
    TowerDungeon_Gather = 44001,  //农牧副本
    TowerDungeon_Farming = 54001,  //农牧副本
}

/// <summary>
/// 区域优先级
/// 用于区域重叠时的优先级判断
/// 枚举值越大，优先级越高
/// </summary>
public enum eSceneAreaPriority
{
    none,
    low,
    middle,
    high,
}

/// <summary>
/// 区域改变类型
/// </summary>
public enum eAreaChangedType
{
    enter,
    exit,
}

public enum eSceneType
{
    Unknown = 0,//未知
    World = 1,//大世界
    Home = 2,//家园
    Instancing = 3,//副本
}