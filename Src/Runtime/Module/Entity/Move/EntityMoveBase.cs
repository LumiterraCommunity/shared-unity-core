using UnityGameFramework.Runtime;

/// <summary>
/// 移动组件基础
/// </summary>
public class EntityMoveBase : EntityBaseComponent
{
    /// <summary>
    /// 移动速度 m/s 不为0 默认1的初始速度 美术场景预览可以直接设置
    /// </summary>
    /// <value></value>
    public float MoveSpeed = 1;

    /// <summary>
    /// 正在移动中
    /// </summary>
    /// <value></value>
    public bool IsMoving { get; protected set; }

    private void OnDisable()
    {
        if (IsMoving)
        {
            StopMove();
        }
    }

    /// <summary>
    /// 设置移动速度 m/s 不能为0
    /// </summary>
    /// <param name="speed"></param>
    public void SetMoveSpeed(float speed)
    {
        if (speed <= 0)
        {
            Log.Error($"path move set speed <0 ={speed}");
            return;
        }

        if (speed.ApproximatelyEquals(0))
        {
            Log.Error($"path move set speed approximately equals 0 ={speed}");
            return;
        }

        MoveSpeed = speed;
    }

    /// <summary>
    /// 开始移动 标志的正式开始移动
    /// </summary>
    public virtual void StartMove()
    {
        IsMoving = true;
        enabled = true;
    }

    /// <summary>
    /// 停止移动 和自动结束是不一样的
    /// </summary>
    public virtual void StopMove()
    {
        IsMoving = false;
        enabled = false;
    }

    /// <summary>
    /// 移动完成 子类调用的  代表移动主动结束的情况
    /// </summary>
    protected virtual void FinishMove()
    {
        IsMoving = false;
        enabled = false;
    }
}