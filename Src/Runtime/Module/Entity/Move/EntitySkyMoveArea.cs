using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 用于判定玩家在跳跳菇和风场等空中移动区域 目前为了解决移动校验空中强拉体验
/// </summary>
public class EntitySkyMoveArea : EntityBaseComponent
{
    private const float VALID_SKY_AREA_RADIUS = 20;//判定是否可以使用客户端位置的有效的空中范围半径
    private const float VALID_SKY_AREA_HEIGHT = 50;//判定是否可以使用客户端位置的有效的空中范围高度

    /// <summary>
    /// 是否还在空中移动区域状态 这个状态不是一定在某个空中力场中 只要进入力场浮空到下次着地都是这个状态
    /// </summary>
    /// <value></value>
    public bool IsInSkyMoveAreaStatus { get; private set; }

    private EntityMoveData _moveData;
    private readonly HashSet<object> _skyAreaSet = new();//我所在的具体的空中场景力场中 包括 整个风场 和 跳跳菇的触发阶段
    private Vector3 _lastRoleSkyAreaPos;

    private void Start()
    {
        _moveData = GetComponent<EntityMoveData>();
    }

    private void LateUpdate()
    {
        //有空中力场需要进入状态 为什么没用CharacterMoveCtrl.MoveSpeed的Y来判定 是担心后面有些弹跳技能也会被误判断成空中力场来作弊 还是业务侧主动来添加力场更保险
        //即时后面新的力场没有添加进来也只是可能容易强拉不会有作弊严重后果
        if (_skyAreaSet.Count > 0 && !IsInSkyMoveAreaStatus)
        {
            EnterSkyMoveAreaStatus();
        }
        else if (_skyAreaSet.Count == 0 && _moveData.IsGrounded && IsInSkyMoveAreaStatus)//没有力场又着地了就判定空中区域无效了
        {
            ExitSkyMoveAreaStatus();
        }

        if (_skyAreaSet.Count > 0 && IsInSkyMoveAreaStatus)
        {
            UpdateLastRoleSkyAreaPos();
        }
    }

    private void UpdateLastRoleSkyAreaPos()
    {
        _lastRoleSkyAreaPos = RefEntity.Position;
    }

    private void ExitSkyMoveAreaStatus()
    {
        if (!IsInSkyMoveAreaStatus)
        {
            Log.Error("ExitSkyMoveAreaStatus error, already exit sky move area status");
            return;
        }

        IsInSkyMoveAreaStatus = false;
    }

    private void EnterSkyMoveAreaStatus()
    {
        if (IsInSkyMoveAreaStatus)
        {
            Log.Error("EnterSkyMoveAreaStatus error, already in sky move area status");
            return;
        }

        IsInSkyMoveAreaStatus = true;
    }


    /// <summary>
    /// 添加一个空中力场区域 此时说明角色收到了空中力场改变了速度 参数是具体的力场区域对象
    /// </summary>
    internal void AddSkyArea(object area)
    {
        if (!_skyAreaSet.Add(area))
        {
            Log.Error($"AddSkyArea error, area already exist,type:{area.GetType()}");
        }
    }

    /// <summary>
    /// 移除一个空中力场区域 参数是具体的力场区域对象
    /// </summary>
    internal void RemoveSkyArea(object area)
    {
        if (!_skyAreaSet.Remove(area))
        {
            Log.Error($"RemoveSkyArea error, area not exist,type:{area.GetType()}");
        }
    }

    /// <summary>
    /// 检查目标位置是否在最后一个浮空区域 并不一定要在力场中 只要在最后记录的位置一定范围内都算有效范围 往往是圆柱范围判定
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool CheckInLastSkyArea(Vector3 position)
    {
        if (!IsInSkyMoveAreaStatus)
        {
            return false;
        }

        Vector3 offset = position - _lastRoleSkyAreaPos;
        if (offset.OnlyXZ().magnitude > VALID_SKY_AREA_RADIUS)
        {
            return false;
        }

        if (Mathf.Abs(offset.y) > VALID_SKY_AREA_HEIGHT)
        {
            return false;
        }

        return true;
    }
}