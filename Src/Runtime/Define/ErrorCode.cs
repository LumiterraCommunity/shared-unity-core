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

    #endregion

    #region  S2S错误码
    notFindUserData = 1001404,

    #endregion
}