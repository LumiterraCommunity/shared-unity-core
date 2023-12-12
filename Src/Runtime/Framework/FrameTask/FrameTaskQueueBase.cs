/// <summary>
/// 分帧任务队列基类
/// </summary>
public abstract class FrameTaskQueueBase : TaskQueue
{
    /// <summary>
    /// 任务类型 用于区分不同的任务队列
    /// </summary>
    /// <value></value>
    public int TaskType { get; private set; }

    public FrameTaskQueueBase(int taskType)
    {
        TaskType = taskType;
    }

    public void Tick()
    {
        if (LinkedQueue.Count <= 0)
        {
            return;
        }

        OnTick();
    }

    /// <summary>
    /// 子类实现具体每帧执行的逻辑 执行时队列一定不为空 不需要检查
    /// </summary>
    protected abstract void OnTick();
}