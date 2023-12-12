using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 实体上的输入数据 持续性的数据  瞬发性的命令通过事件及时广播出去不在这里
/// </summary>
public class EntityInputData : EntityBaseComponent
{
    /// <summary>
    /// 当前输入了方向性移动 空代表没有输入
    /// </summary>
    /// <value></value>
    public Vector3? InputMoveDirection { get; private set; }
    /// <summary>
    /// 当前输入了路径移动 不为空
    /// </summary>
    /// <value></value>
    public Queue<Vector3> InputMovePath { get; private set; } = new();


    /// <summary>
    /// 当前输入了鼠标位置 空代表没有输入
    /// </summary>
    /// <value></value>
    public Vector3? InputMouseDirection { get; private set; }

    /// <summary>
    /// 输入了鼠标位置 置空为没有鼠标位置
    /// </summary>
    /// <param name="moveDir"></param>
    public void SetInputMouseDirection(Vector3? dir)
    {
        InputMouseDirection = dir != null ? ((Vector3)dir).normalized : dir;
    }

    /// <summary>
    /// 输入了方向移动 置空为没有方向移动
    /// </summary>
    /// <param name="moveDir"></param>
    public void SetInputMoveDir(Vector3 moveDir)
    {
        InputMoveDirection = moveDir;

        //方向移动优先级更高 需要取消路径移动
        if (InputMovePath.Count > 0)
        {
            ClearInputMovePath(true);
        }
    }

    /// <summary>
    /// 清理输入方向移动
    /// </summary>
    public void ClearInputMoveDir()
    {
        InputMoveDirection = null;
    }

    /// <summary>
    /// 设置输入移动路径 空代表取消
    /// </summary>
    /// <param name="path"></param>
    public void SetInputMovePath(Vector3[] path, bool needEvent = true)
    {
        if (path == null || path.Length == 0)
        {
            Log.Error($"entity input move path error ,is null:{path == null}");
            return;
        }

        ClearInputMovePath(false);

        foreach (Vector3 pos in path)
        {
            InputMovePath.Enqueue(pos);
        }

        if (needEvent)
        {
            RefEntity.EntityEvent.InputMovePathChanged?.Invoke();
        }
    }

    /// <summary>
    /// 输入移动到某个点
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="needEvent"></param>
    public void SetInputMoveToPoint(Vector3 pos, bool needEvent = true)
    {
        ClearInputMovePath(false);

        InputMovePath.Enqueue(pos);

        if (needEvent)
        {
            RefEntity.EntityEvent.InputMovePathChanged?.Invoke();
        }
    }

    /// <summary>
    /// 清理输入移动路径包括移动到一个点
    /// </summary>
    public void ClearInputMovePath(bool needEvent)
    {
        InputMovePath.Clear();

        if (needEvent && RefEntity != null)
        {
            RefEntity.EntityEvent.InputMovePathChanged?.Invoke();
        }
    }

    /// <summary>
    /// 所有移动输入停止
    /// </summary>
    public void InputMoveStop()
    {
        ClearInputMoveDir();
        ClearInputMovePath(true);
    }

    /// <summary>
    /// 清理输入鼠标输入
    /// </summary>
    public void ClearInputMouse()
    {
        InputMouseDirection = null;
    }
}