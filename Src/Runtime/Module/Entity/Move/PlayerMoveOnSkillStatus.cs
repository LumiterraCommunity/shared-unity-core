using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 玩家在技能状态下的强行平移组件 用于在一直释放技能时的保持和客户端位置同步
/// 添加了这个组件时 就会自动在技能释放生命周期内平移到指定地点 没放技能的时候是走过去 如果被眩晕等打断技能状态就停止平移
/// </summary>
public class PlayerMoveOnSkillStatus : EntityBaseComponent
{
    private bool _isMoving;//是否正在移动
    private DistanceMove _distanceMove;

    private void Start()
    {
        if (!TryGetComponent(out _distanceMove))
        {
            Log.Error("PlayerMoveOnSkillStatus Start Error: No DistanceMove Component");
        }

        RefEntity.EntityEvent.OnSkillStatusStart += OnSkillStatusStart;
        RefEntity.EntityEvent.OnSkillStatusEnd += OnSkillStatusEnd;
    }

    private void OnDestroy()
    {
        RefEntity.EntityEvent.OnSkillStatusStart -= OnSkillStatusStart;
        RefEntity.EntityEvent.OnSkillStatusEnd -= OnSkillStatusEnd;

        StopMove();
    }

    private void OnSkillStatusStart(InputSkillReleaseData data)
    {
        if (_distanceMove == null)
        {
            return;
        }

        StopMove();

        Vector3 offset = data.Pos - RefEntity.Position;
        if (offset.ApproximatelyEquals(Vector3.zero))
        {
            return;
        }

        //空中不要移动 否则可能重力会重复移动
        if (!RefEntity.MoveData.IsGrounded)
        {
            return;
        }

        float speed = offset.magnitude / MoveDefine.ARRIVED_INPUT_POS_TIME_ON_SKILL;
        speed = Mathf.Min(speed, RefEntity.MoveData.Speed);//最大不能超过移动速度 防止作弊

        _distanceMove.MoveTo(offset, offset.magnitude, speed, null);//固定方向移动一段距离
        _distanceMove.StartMove();

        _isMoving = true;
    }

    private void OnSkillStatusEnd(DRSkill skill, bool isBreak)
    {
        StopMove();
    }

    private void StopMove()
    {
        if (_distanceMove == null)
        {
            return;
        }

        if (!_isMoving)
        {
            return;
        }

        _isMoving = false;
        _distanceMove.StopMove();
    }
}