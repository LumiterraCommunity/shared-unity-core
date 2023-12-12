using UnityEngine;

/// <summary>
/// 依靠CharacterController控制角色直线运动
/// </summary>
public sealed class CharacterDirectionMove : DirectionMove
{
    private CharacterMoveCtrl _controller;
    private MoveModifier _moveModifier;


    protected override void Start()
    {
        base.Start();
        _controller = GetComponent<CharacterMoveCtrl>();
    }

    public override void StartMove()
    {
        base.StartMove();
        AddMoveModifier();
    }

    private void Update()
    {
        if (_controller == null || _moveModifier == null)
        {
            return;
        }
        if (InputData.InputMoveDirection != null && MoveSpeed > 0)
        {
            Vector3 moveDir = InputData.InputMoveDirection.Value;
            moveDir.Normalize();
            transform.forward = moveDir;
            _controller.UpdateMove(_moveModifier, MoveSpeed * moveDir, Vector3.zero);
        }
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

        Vector3 speed = Vector3.zero;
        if (InputData.InputMoveDirection != null && MoveSpeed > 0)
        {
            Vector3 moveDir = InputData.InputMoveDirection.Value;
            moveDir.Normalize();
            speed = MoveSpeed * moveDir;
        }
        _moveModifier = _controller.AddMove(speed, Vector3.zero);
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