using UnityEngine;
using static HomeDefine;

/// <summary>
/// 采集资源共用接口
/// </summary>
public interface ICollectResourceCore
{
    public ulong Id { get; }
    /// <summary>
    /// 资源类型 分为土地作物 矿石等
    /// </summary>
    /// <value></value>
    eResourceType ResourceType { get; }
    /// <summary>
    /// 资源逻辑上的root节点 别人有可能会从这里拿组件
    /// </summary>
    /// <value></value>
    GameObject LogicRoot { get; }
    /// <summary>
    /// 位置
    /// </summary>
    /// <value></value>
    Vector3 Position { get; }
    /// <summary>
    /// 等级
    /// </summary>
    /// <value></value>
    int Lv { get; }

    /// 检查是否支持当前复合动作 不支持也能选中 只是不会执行和发出去
    /// </summary>
    /// <param name="action">复合动作</param>
    /// <returns></returns>
    bool CheckSupportAction(eAction action);
    /// <summary>
    /// 获取当前支持的复合动作
    /// </summary>
    /// <value></value>
    eAction SupportAction { get; }
    /// <summary>
    /// 执行动作
    /// </summary>
    /// <param name="action"></param>
    /// <param name="toolCid">工具id 可能是种子 肥料 装备</param>
    /// <param name="skillId">技能id</param>
    /// <param name="playerId">操作的玩家id</param>
    /// <param name="actionData">动作数据 没有传null</param>
    void ExecuteAction(eAction action, int toolCid, int skillId, long playerId, object actionData);
    /// <summary>
    /// 执行了一次进度 最后一次进度也会调用 再去调用执行动作
    /// </summary>
    /// <param name="targetCurAction"></param>
    /// <param name="skillId">技能id</param>
    /// <param name="deltaProgress">进度变化值</param>
    /// <param name="isCrit">是否暴击</param>
    /// <param name="isPreEffect">是否是预表现效果</param>
    /// <param name="playerId">操作的玩家id</param>
    void ExecuteProgress(eAction targetCurAction, int skillId, int deltaProgress, bool isCrit, bool isPreEffect, long playerId);
}