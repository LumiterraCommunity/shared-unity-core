using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;
/// <summary>
/// 路径移动的浮空状态
/// </summary>
public class FloatInAirPathMoveStatusCore : FloatInAirStatusCore
{
    protected EntityInputData InputData { get; private set; }
    private CharacterMoveCtrl _controller;
    private MoveModifier _moveModifier;

    protected override void OnEnter(IFsm<EntityStatusCtrl> fsm)
    {
        base.OnEnter(fsm);

        if (MoveDefine.ENABLE_MOVE_IN_AIR)
        {
            InputData = StatusCtrl.GetComponent<EntityInputData>();

            if (!StatusCtrl.TryGetComponent(out _controller))
            {
                Log.Error($"FloatInAirPathMoveStatusCore enter,not find _controller,name={StatusCtrl.gameObject.name}");
            }

            ApplyMove();
        }
    }

    protected override void OnLeave(IFsm<EntityStatusCtrl> fsm, bool isShutdown)
    {
        if (MoveDefine.ENABLE_MOVE_IN_AIR)
        {
            if (InputData != null)
            {
                InputData.ClearInputMovePath(false);
                InputData = null;
            }
            RemoveMoveModifier();
            _controller = null;
        }

        base.OnLeave(fsm, isShutdown);
    }

    protected override void OnUpdate(IFsm<EntityStatusCtrl> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        if (MoveDefine.ENABLE_MOVE_IN_AIR)
        {
            ApplyMove();
        }
    }

    private void ApplyMove()
    {
        if (_controller == null)
        {
            return;
        }

        Vector3 moveSpeed = Vector3.zero;
        if (InputData.InputMovePath.Count > 0)
        {
            Vector3 nextPos = InputData.InputMovePath.Peek();
            Vector3 offset = nextPos - StatusCtrl.RefEntity.Position;

            //改变朝向
            StatusCtrl.RefEntity.SetForward(new Vector3(offset.x, 0, offset.z));//在空中只设置水平方向移动 不直接走到路径点

            moveSpeed = new Vector3(offset.x, 0, offset.z).normalized * StatusCtrl.RefEntity.MoveData.Speed;
        }
        if (_moveModifier != null)
        {
            _controller.UpdateMove(_moveModifier, moveSpeed, Vector3.zero);
        }
        else
        {
            AddMoveModifier(moveSpeed);
        }

    }


    /// <summary>
    /// 添加移动速度修改器
    /// </summary>
    private void AddMoveModifier(Vector3 speed)
    {
        RemoveMoveModifier();
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