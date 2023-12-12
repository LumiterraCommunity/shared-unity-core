using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 分帧任务管理
/// </summary>
public abstract class FrameTaskManagerCore : MonoBehaviour
{
    private readonly ListMap<int, FrameTaskQueueBase> _queueMap = new();

    private void Awake()
    {
        InitQueue();
    }

    private void Update()
    {
        for (int i = 0; i < _queueMap.Count; i++)
        {
            _queueMap[i].Tick();
        }
    }

    /// <summary>
    /// 添加一个带参数分帧任务
    /// </summary>
    /// <param name="frameTaskType">添加之前需要在InitQueue添加过该类型队列</param>
    /// <param name="cb"></param>
    /// <param name="parm"></param>
    /// <returns></returns>
    public LinkedListNode<TaskDelegate> AddTask(int frameTaskType, Action<object> cb, object parm)
    {
        if (!_queueMap.ContainsKey(frameTaskType))
        {
            Log.Error($"frame task mgr add have parm task not find type={frameTaskType}");
            return null;
        }

        FrameTaskQueueBase queue = _queueMap.GetFromKey(frameTaskType);
        return queue.Push(cb, parm);
    }

    /// <summary>
    /// 添加一个不带参数分帧任务
    /// </summary>
    /// <param name="frameTaskType">添加之前需要在InitQueue添加过该类型队列</param>
    /// <param name="cb"></param>
    /// <returns></returns>
    public LinkedListNode<TaskDelegate> AddTask(int frameTaskType, Action cb)
    {
        if (!_queueMap.ContainsKey(frameTaskType))
        {
            Log.Error($"frame task mgr add no parm task not find type={frameTaskType}");
            return null;
        }

        FrameTaskQueueBase queue = _queueMap.GetFromKey(frameTaskType);
        return queue.Push(cb);
    }

    /// <summary>
    /// 移除一个分帧任务 移除前需要业务层保存好该任务添加时的的node
    /// </summary>
    /// <param name="frameTaskType"></param>
    /// <param name="taskNode"></param>
    public void RemoveTask(int frameTaskType, LinkedListNode<TaskDelegate> taskNode)
    {
        if (!_queueMap.ContainsKey(frameTaskType))
        {
            Log.Error($"frame task mgr remove task not find type={frameTaskType}");
            return;
        }

        FrameTaskQueueBase queue = _queueMap.GetFromKey(frameTaskType);
        queue.RemoveTask(taskNode);
    }

    /// <summary>
    /// 清理一个分帧任务队列的所有任务
    /// </summary>
    /// <param name="frameTaskType"></param>
    public void ClearQueueAllTask(int frameTaskType)
    {
        if (!_queueMap.ContainsKey(frameTaskType))
        {
            Log.Error($"frame task mgr remove queue all task not find type={frameTaskType}");
            return;
        }

        FrameTaskQueueBase queue = _queueMap.GetFromKey(frameTaskType);
        queue.Clear();
    }

    /// <summary>
    /// 添加一个分帧任务队列 重复会报错 一般在InitQueue中添加
    /// </summary>
    /// <param name="queue"></param>
    public void AddQueue(FrameTaskQueueBase queue)
    {
        if (queue == null)
        {
            Log.Error($"frame task mgr add queue is null");
            return;
        }

        int frameTaskType = queue.TaskType;
        if (_queueMap.ContainsKey(frameTaskType))
        {
            Log.Error($"frame task mgr add queue is repeat type={frameTaskType}");
            return;
        }

        _ = _queueMap.Add(frameTaskType, queue);
    }

    public void RemoveQueue(int frameTaskType)
    {
        if (!_queueMap.ContainsKey(frameTaskType))
        {
            Log.Error($"frame task mgr remove not exist queue type={frameTaskType}");
            return;
        }

        _ = _queueMap.Remove(frameTaskType);
    }

    /// <summary>
    /// 子类实现 用来初始化队列
    /// </summary>
    protected abstract void InitQueue();
}