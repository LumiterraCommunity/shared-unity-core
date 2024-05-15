using GameFramework.Fsm;
using UnityGameFramework.Runtime;
/// <summary>
/// 方向移动的浮空状态
/// </summary>
public class FloatInAirDirectionMoveStatusCore : FloatInAirStatusCore
{
    private DirectionMove _directionMove;
    private EntityInputData _inputData;
    private bool _isDirectionMoving;//是否正在输入移动

    protected override void OnEnter(IFsm<EntityStatusCtrl> fsm)
    {
        base.OnEnter(fsm);

        _inputData = StatusCtrl.GetComponent<EntityInputData>();
        if (!StatusCtrl.TryGetComponent(out _directionMove))
        {
            Log.Error($"FloatInAirDirectionMoveStatusCore enter,not find PathMove,name={StatusCtrl.gameObject.name}");
        }

        CheckInputMoveStatus();
    }

    protected override void OnLeave(IFsm<EntityStatusCtrl> fsm, bool isShutdown)
    {
        StopInputMove();

        base.OnLeave(fsm, isShutdown);
    }

    protected override void OnUpdate(IFsm<EntityStatusCtrl> fsm, float elapseSeconds, float realElapseSeconds)
    {
        CheckInputMoveStatus();

        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);//这里会切状态leave 放后面执行
    }

    private void CheckInputMoveStatus()
    {
        if (!MoveDefine.ENABLE_MOVE_IN_AIR)
        {
            return;
        }

        if (_inputData.InputMoveDirection != null && !_isDirectionMoving)
        {
            StartInputMove();
        }
        else if (_inputData.InputMoveDirection == null && _isDirectionMoving)
        {
            StopInputMove();
        }
    }

    private void StartInputMove()
    {
        if (_isDirectionMoving)
        {
            return;
        }

        _isDirectionMoving = true;
        _directionMove.SetMoveSpeed(StatusCtrl.RefEntity.MoveData.Speed);
        _directionMove.StartMove();
    }

    private void StopInputMove()
    {
        if (!_isDirectionMoving)
        {
            return;
        }

        _isDirectionMoving = false;
        _directionMove.StopMove();
    }

    protected override void AddEvent(EntityEvent entityEvent)
    {

        entityEvent.EntityMoveDataSpeedUpdate += OnMoveSpeedUpdate;
    }

    protected override void RemoveEvent(EntityEvent entityEvent)
    {
        entityEvent.EntityMoveDataSpeedUpdate -= OnMoveSpeedUpdate;
    }
    private void OnMoveSpeedUpdate()
    {
        if (_isDirectionMoving)
        {
            _directionMove.SetMoveSpeed(StatusCtrl.RefEntity.MoveData.Speed);
        }
    }
}