using System;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 按距离朝某个方向移动 能不能移动过去 取决于是否有碰撞体等其他子类实现 但是会按照速度消耗掉所有移动距离位置 不会立马停止移动
/// </summary>
public abstract class DistanceMove : EntityMoveBase
{
    /// <summary>
    /// 位置更新事件 T0：最新位置
    /// </summary>
    public event Action<Vector3> OnPosUpdatedEvent;

    private float _remainDistance = -1;//剩下未移动的距离
    protected Vector3 DirectionUnit;//移动方向单位
    private Action _arrivedCB;

    public void MoveTo(Vector3 dir, float distance, float speed, Action arrivedCB = null)
    {
        if (dir == Vector3.zero)
        {
            Log.Error($"distance move to dir is zero");
            return;
        }

        if (distance <= 0)
        {
            Log.Error($"distance move set distance <0 ={distance}");
            return;
        }
        SetMoveSpeed(speed);
        _arrivedCB = arrivedCB;
        _remainDistance = distance;
        DirectionUnit = dir.normalized;
        StartMove();
    }

    protected override void FinishMove()
    {
        base.FinishMove();

        _arrivedCB?.Invoke();
    }

    public override void StopMove()
    {
        _arrivedCB = null;
        _remainDistance = -1;

        base.StopMove();
    }

    /// <summary>
    /// 走一步 需要在合适时机触发
    /// </summary>
    /// <param name="tickDelay"></param>
    protected void TickMove(float tickDelay)
    {
        if (_remainDistance <= 0)
        {
            return;
        }

        float stepDistance = MoveSpeed * tickDelay;

        bool isArrived = false;
        if (_remainDistance <= stepDistance)
        {
            stepDistance = _remainDistance;
            _remainDistance = -1;
            isArrived = true;
        }
        else
        {
            _remainDistance -= stepDistance;
        }
        OnPosUpdatedEvent?.Invoke(transform.position);

        if (isArrived)
        {
            FinishMove();
        }
    }
}