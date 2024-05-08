using UnityEngine;

/// <summary>
/// 依靠CharacterController控制角色直线运动
/// </summary>
public sealed class CharacterDirectionMove : DirectionMove
{
    private CharacterMoveCtrl _controller;
    /// <summary>
    /// 当前移动速度修改器 为空时说明没有修改移动
    /// </summary>
    /// <value></value>
    public MoveModifier MoveModifier { get; private set; }

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
        if (_controller == null || MoveModifier == null)
        {
            return;
        }
        if (InputData.InputMoveDirection != null && MoveSpeed > 0)
        {
            Vector3 moveDir = InputData.InputMoveDirection.Value;
            moveDir.Normalize();
            transform.forward = moveDir;
            _controller.UpdateMove(MoveModifier, MoveSpeed * moveDir, Vector3.zero);
        }
        else
        {
            _controller.UpdateMove(MoveModifier, Vector3.zero, Vector3.zero);
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
        MoveModifier = _controller.AddMove(speed, Vector3.zero);
    }

    /// <summary>
    /// 移除移动速度修改器
    /// </summary>
    private void RemoveMoveModifier()
    {
        if (MoveModifier != null)
        {
            _controller.RemoveMove(MoveModifier);
            MoveModifier = null;
        }
    }
}