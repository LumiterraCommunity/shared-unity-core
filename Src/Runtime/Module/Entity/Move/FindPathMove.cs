using System;
using UnityEngine;

/// <summary>
/// 寻路移动基类 业务层直接获取基类操作 需要外部自己设置PathMove组件和其中速度参数 寻路组件不重复设置 子类可能是navmesh寻路 也可能是A星等别的方式
/// </summary>
public abstract class FindPathMove : EntityBaseComponent
{
    [Tooltip("勾上后只会输入程序需要的移动数据，不会自动执行移动")]
    public bool OnlyInputMoveData = false;

    # region 仅仅输入移动数据方式
    private EntityInputData _inputData;//只有在OnlyInputMoveData==true下才有效 此时不会真的移动 只会输入到该组件类 由状态机驱动移动
    private EntityEvent _entityEvent;
    #endregion

    # region 能够执行移动方式
    private PathMove _pathMove;//只有在OnlyInputMoveData==false下 代表需要自动移动时才直接控制这个组件移动
    private bool _addedPathMove = false;
    #endregion

    protected Action<FindPathMove> MoveArrivedCB;

    /// <summary>
    /// 当期目标点 停止移动后会置空
    /// </summary>
    /// <value></value>
    protected Vector3? Destination { get; private set; }

    /// <summary>
    /// 寻路移动中
    /// </summary>
    public bool IsMoving => Destination != null;

    protected virtual void Start()
    {
        if (OnlyInputMoveData)
        {
            _inputData = GetComponent<EntityInputData>();
            _entityEvent = GetComponent<EntityEvent>();
            _entityEvent.OnEntityPathMoveArrived += OnMoveArrived;
        }
        else
        {
            if (!TryGetComponent(out _pathMove))
            {
                _pathMove = gameObject.AddComponent<PathMove>();
                _addedPathMove = true;
            }
        }
    }

    protected virtual void OnDestroy()
    {
        StopMove();

        if (OnlyInputMoveData)
        {
            _inputData = null;
            _entityEvent.OnEntityPathMoveArrived -= OnMoveArrived;
            _entityEvent = null;
        }
        else
        {
            if (_addedPathMove)
            {
                Destroy(_pathMove);
            }
            _pathMove = null;
        }
    }

    /// <summary>
    /// 移动到某个点 可能会寻路失败 返回false
    /// </summary>
    /// <param name="destination">目的地 如果目的地不能走路 有可能最终走到附近一点点可以走路的地方</param>
    /// <param name="moveArrivedCB">移动到终点后回调 对仅仅是寻路输入移动数据的情况下无效 需要自行监听事件</param>
    /// <returns></returns>
    public bool MoveToPosition(Vector3 destination, Action<FindPathMove> moveArrivedCB = null)
    {
        StopMove(true);

        Destination = destination;
        MoveArrivedCB = moveArrivedCB;

        Vector3[] path = FindPath(destination);

        if (path == null || path.Length == 0)
        {
            return false;
        }

        if (OnlyInputMoveData)
        {
            _inputData.SetInputMovePath(path);
        }
        else
        {
            _pathMove.MovePath(path, (target) => OnMoveArrived());
        }

        return true;
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public void StopMove(bool fromNextMove = false)
    {
        if (OnlyInputMoveData)
        {
            _inputData.ClearInputMovePath(!fromNextMove);
        }
        else
        {
            _pathMove.StopMove();
        }

        Destination = null;
        MoveArrivedCB = null;
    }

    private void OnMoveArrived()
    {
        //移动到达终点
        MoveArrivedCB?.Invoke(this);

        Destination = null;
        MoveArrivedCB = null;
    }

    /// <summary>
    /// 子类实现找路逻辑
    /// </summary>
    /// <param name="destination">寻路目的地</param>
    /// <returns>找不到路径或者出错直接返回null即可</returns>
    protected abstract Vector3[] FindPath(Vector3 destination);
}