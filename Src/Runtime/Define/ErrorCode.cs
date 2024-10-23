/** 
 * @Author XQ
 * @Date 2022-08-05 12:54:47
 * @FilePath /Assets/Plugins/SharedCore/Src/Runtime/Define/ErrorCode.cs
 */
/// <summary>
/// 统一errorCode 首位代表常规错误码还是s2s 前3位模块 后3位模块内 预留头2位将来用
/// </summary>
public enum eErrorCode : int
{
    success = 0,

    systemError = 0000001, //系统内部错误

    #region  常规错误码 主要是S2C 但是有些通用
    selfMoveFail = 0001001,
    pickDropNotExist = 0002001,//拾取物品不存在
    pickDropNotYours = 0002002,//拾取物品不是自己的
    pickDropOverdue = 0002003,//拾取物品过期
    pickDropOverRange = 0002004,//拾取物品超出范围
    pickDropPickerIllegal = 0002005,//拾取者非法
    inputSkillFail = 0003001,
    inputSkillStatusFail = 0003002,
    inputSkillCDFail = 0003003,
    inputSkillUseFail = 0003004, //技能使用失败
    inputSkillUseCostItemFail = 0003005, //技能使用扣除道具失败

    playerReviveAlive = 0004001, //玩家还存活
    playerReviveCD = 0004002, //玩家复活CD中
    entityCDDataCoreNull = 0004003, //玩家角色数据为空
    playerRevivePointNull = 0004004, //玩家附近复活点为空
    playerReviveInBattle = 0004005,  //玩家在战斗中

    homeCollectResourceNotExist = 0005001, //家园采集资源不存在
    homeActionNotSupport = 0005002, //家园操作不支持
    homeItemCostError = 0005003, //家园道具扣除异常
    animalCaptureReqError = 0006001, //捕捉动物请求异常
    animalCaptureInvalid = 0006002, //捕捉动物无效
    lvNotMatch = 0006003, //等级不匹配
    memberTooFar = 0006004, //队员太远
    ticketsNotEnough = 0006005, //门票不足


    sceneStartFailed = 0010000, // 场景服务启动失败
    changeSceneTimeOut = 0010001, // 切换场景超时
    playerEntityNotFound = 0010002, //玩家实体未找到
    canNotEnterScene = 0010003, // 没有权限进入场景
    changeSceneEventNotFound = 0010004, // 切场景事件丢失
    repeatedApplyChangeScene = 0010005, // 重复申请切场景
    takeTicketFailed = 0010006, // 门票扣除失败
    readyChangeSceneTimeout = 0010007, // 准备切服状态已经超时

    InstancingLevelCompleteFailure = 0007001, //运行关卡失败
    InstancingLevelCompleteNotLeader = 0007002, //不是队长
    InstancingLevelCompleteNotSuccess = 0007003, //上一关卡未成功
    InstancingNotComplete = 0007004, //副本未完成
    InstancingNotCompleteMatch = 0007005, //副本未完成匹配
    changePlayerCampFailed = 0008001, //切换阵营失败
    playerReviveNoLifeCount = 0008002, //玩家副本复活次数不足



    #region 家园
    NotInHome = 0009001, //不在家园中
    #endregion

    #region  图腾
    TotemNotFind = 0010001, //图腾未找到
    TotemHavePrizeLp = 0010002,//图腾有还有投资数据
    TotemOwnerInvalid = 0010003,//图腾所有者无效
    TotemSysErr = 0010004,//图腾系统错误
    TotemNotHaveReward = 0010005,//图腾没有收益
    #endregion

    #region  世界图腾
    worldTotemOverlap = 0011001,//世界图腾重叠
    worldTotemTerrainInvalid = 0011002,//世界图腾地形不允许
    worldTotemAreaInvalid = 0011003,//世界图腾区域不允许
    worldTotemSceneInvalid = 0011004,//世界图腾场景不允许
    worldTotemNotFound = 0011005,//世界图腾未找到
    worldTotemDataNotFound = 0011006,//世界图腾数据未找到
    worldTotemNotActive = 0011007,//世界图腾未激活
    #endregion

    #endregion

    #region 宠物
    petSystemErr = 1001001, //宠物系统错误
    petRecallOnCD = 1001002, //宠物召回CD中
    petNotFound = 1001003, //宠物未找到
    #endregion

    #region  S2S错误码
    notFindUserData = 1001404,

    #endregion

    #region  任务
    npcNotFound = 1002001, //npc未找到
    npcTooFar = 1002002, //npc太远
    #endregion

    #region  押镖
    escortWagonSystemErr = 1003001, //押镖系统错误
    notTeamLeader = 1003002, //不是队长
    wagonNotFound = 1003003, //镖车未找到
    teamNotInWagonTask = 1003004, //队伍不在押镖任务中
    wagonConfigNotFound = 1003005, //镖车配置未找到
    wagonPathNotFound = 1003006, //镖车路径未找到
    teamNotFound = 1003007, //队伍未找到
    #endregion
}