using System;
using System.Collections.Generic;
using GameFramework;
using UnityGameFramework.Runtime;

/// <summary>
/// 任务队列 保持进入顺序执行  核心是 push 和 execute  可以删除其中任务
/// </summary>
public class TaskQueue
{
    protected LinkedList<TaskDelegate> LinkedQueue = new();

    public int Length => LinkedQueue.Count;

    protected virtual void OnAddTask(LinkedListNode<TaskDelegate> targetNode)
    {
        // 子类选择处理 添加任务后执行
    }

    protected virtual void OnRemoveTask(LinkedListNode<TaskDelegate> targetNode)
    {
        // 子类选择处理 删除任务前执行
    }

    public void Clear()
    {
        foreach (TaskDelegate task in LinkedQueue)
        {
            ReferencePool.Release(task);
        }

        LinkedQueue.Clear();
    }

    /// <summary>
    /// 向队列推入一个任务
    /// </summary>
    /// <param name="cb"></param>
    /// <param name="parm">没有参数就传null</param>
    /// <returns></returns>
    public LinkedListNode<TaskDelegate> Push(Action<object> cb, object parm)
    {
        TaskDelegate task = ReferencePool.Acquire<TaskDelegate>();
        task.SetData(cb, parm);
        return Push(task);
    }

    public LinkedListNode<TaskDelegate> Push(Action cb)
    {
        TaskDelegate task = ReferencePool.Acquire<TaskDelegate>();
        task.SetData(cb);
        return Push(task);
    }

    private LinkedListNode<TaskDelegate> Push(TaskDelegate task)
    {
        LinkedListNode<TaskDelegate> addNode = LinkedQueue.AddLast(task);
        OnAddTask(addNode);
        return addNode;
    }

    /// <summary>
    /// 执行最早的一个任务并删除
    /// </summary>
    public void Execute()
    {
        if (LinkedQueue.Count == 0)
        {
            return;
        }

        TaskDelegate task = LinkedQueue.First.Value;
        task.Execute();
        RemoveTask(LinkedQueue.First);
    }

    /// <summary>
    /// 直接删除某个任务
    /// </summary>
    /// <param name="taskNode"></param>
    public void RemoveTask(LinkedListNode<TaskDelegate> taskNode)
    {
        if (taskNode == null)
        {
            Log.Error("TaskQueue removeTask target is null");
            return;
        }

        if (taskNode.List == null)
        {
            return; // 已经被删除过了，比如在完成时又调用removeTask就会触发，或者重复执行
        }

        OnRemoveTask(taskNode);

        LinkedQueue.Remove(taskNode);

        ReferencePool.Release(taskNode.Value);
    }
}