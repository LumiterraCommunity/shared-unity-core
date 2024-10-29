using System;
using GameMessageCore;
using UnityEngine;

/// <summary>
/// 单块土地上自身的事件
/// </summary>
public class SoilEvent : MonoBehaviour
{
    /// <summary>
    /// 使用保存数据初始化状态 T0:土地的保存数据
    /// </summary>
    public Action<SoilSaveData> MsgInitStatus;

    /// <summary>
    /// 执行某个动作 T0:动作类型 T1:执行动作的玩家id T2：执行动作的具体实体id T3：动作参数（比如播种的种子cid和是否有效等）自行判断解析异常
    /// </summary>
    public Action<HomeDefine.eAction, long, long, object> MsgExecuteAction;
    /// <summary>
    /// 被击中 T0:skillID
    /// </summary>
    public Action<int> OnBeHit;
    /// <summary>
    /// 开始收获特殊动画 T0:收获的玩家实体
    /// </summary>
    public Action<EntityBase> StartHarvestSpecialAnim;
    /// <summary>
    /// 当功能性种子成熟时 T0:功能性种子类型
    /// </summary>
    public Action<SeedFunctionType> OnFunctionSeedRipe;
    /// <summary>
    /// 当功能性种子实体被移除后
    /// </summary>
    public Action OnFunctionSeedEntityRemoved;
    /// <summary>
    /// 属性更新  T0:属性类型 T1:更新后到属性值
    /// </summary>
    public Action<eAttributeType, int> OnAttributeUpdated;

    #region  外部控制事件
    /// <summary>
    /// 尝试改变生长阶段 T0:offset生长阶段，+-都可以
    /// </summary>
    public Action<int> TryChangeGrowStage;
    /// <summary>
    /// 尝试改变浇水状态 T0:是否浇水状态，false反而会取消浇水
    /// </summary>
    public Action<bool> TryChangeWaterStatus;
    #endregion
}