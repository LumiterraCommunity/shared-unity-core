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

    /// 检查是否支持当前单一动作执行 只关注动作本身 不支持也能选中 只是不会执行和发出去
    /// </summary>
    /// <param name="action">单一动作</param>
    /// <returns></returns>
    bool CheckSupportAction(eAction action);
    /// <summary>
    /// 检查某个玩家是否能够执行单一动作 关注玩家的权限 也会检查动作本身
    /// </summary>
    /// <param name="playerId">准备执行的玩家id 不是实体id</param>
    /// <param name="action">单一动作</param>
    /// <returns></returns>
    bool CheckPlayerAction(long playerId, eAction action);
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
    /// <param name="playerId">操作的玩家id 可能是宠物的主人</param>
    /// <param name="entityId">操作的具体实体id 可能是宠物</param>
    /// <param name="skillId">技能id</param>/*  */
    /// <param name="actionData">动作数据 没有传null</param>
    void ExecuteAction(eAction action, int toolCid, long playerId, long entityId, int skillId, object actionData);
    /// <summary>
    /// 执行了一次进度 最后一次进度也会调用 再去调用执行动作
    /// </summary>
    /// <param name="targetCurAction"></param>
    /// <param name="triggerEntityId">触发者id</param>
    /// <param name="skillId">技能id</param>
    /// <param name="deltaProgress">进度变化值 正数 如果是hold动作会给0</param>
    /// <param name="isCrit">是否暴击</param>
    /// <param name="isPreEffect">是否是预表现效果</param>
    void ExecuteProgress(eAction targetCurAction, long triggerEntityId, int skillId, int deltaProgress, bool isCrit, bool isPreEffect);
    /// <summary>
    /// 获取对应家园动作的等级 其实是专精等级的概念 错误返回0
    /// </summary>
    /// <param name="action">具体单一动作</param>
    /// <returns></returns>
    int GetActionLevel(eAction action);
}