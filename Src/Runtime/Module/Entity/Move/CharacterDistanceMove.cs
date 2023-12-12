using UnityEngine;

/// <summary>
/// 使用角色控制器的距离移动
/// </summary>
public sealed class CharacterDistanceMove : DistanceMove
{
    private CharacterMoveCtrl _controller;
    private MoveModifier _moveModifier;

    private void Start()
    {
        _controller = GetComponent<CharacterMoveCtrl>();
    }

    private void Update()
    {
        if (_controller == null)
        {
            return;
        }

        TickMove(Time.deltaTime);
    }

    public override void StartMove()
    {
        base.StartMove();
        AddMoveModifier();
    }

    public override void StopMove()
    {

        RemoveMoveModifier();
        base.StopMove();
    }

    protected override void FinishMove()
    {
        RemoveMoveModifier();
        base.FinishMove();
    }

    /// <summary>
    /// 添加移动速度修改器
    /// </summary>
    private void AddMoveModifier()
    {
        RemoveMoveModifier();
        _moveModifier = _controller.AddMove(MoveSpeed * DirectionUnit, Vector3.zero);
    }

    /// <summary>
    /// 移除移动速度修改器
    /// </summary>
    private void RemoveMoveModifier()
    {
        if (_moveModifier != null)
        {
            _controller.RemoveMove(_moveModifier);
            _moveModifier = null;
        }
    }
}