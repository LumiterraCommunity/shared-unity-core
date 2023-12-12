using System;
using System.Collections.Generic;

/// <summary>
/// 队列大约需要多少帧完成 队列越长 每帧执行越多  不管多少都会尽量在某个时间内执行完 防止越来越多
/// </summary>
public class FrameTaskQueueFitQueueLength : FrameTaskQueueBase
{
    private readonly int _queueFinishFrameNum;
    private float _curDealSpeed = 1;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="taskType"></param>
    /// <param name="queueFinishFrameNum">队列大约需要多少帧完成</param>
    /// <returns></returns>
    public FrameTaskQueueFitQueueLength(int taskType, int queueFinishFrameNum) : base(taskType)
    {
        _queueFinishFrameNum = Math.Max(queueFinishFrameNum, 2);
    }

    protected override void OnAddTask(LinkedListNode<TaskDelegate> targetNode)
    {
        base.OnAddTask(targetNode);

        _curDealSpeed = LinkedQueue.Count / (float)_queueFinishFrameNum;
        if (_curDealSpeed < 1)
        {
            _curDealSpeed = 1;
        }
    }

    protected override void OnTick()
    {
        for (int i = 0; i < _curDealSpeed; i++)
        {
            if (LinkedQueue.Count <= 0)
            {
                break;
            }
            Execute();
        }
    }
}
