/// <summary>
/// 每帧固定执行一定数量的任务
/// </summary>
public class FrameTaskQueueFixNum : FrameTaskQueueBase
{
    private readonly int _onceNum = 1;

    public FrameTaskQueueFixNum(int taskType, int onceNum) : base(taskType)
    {
        _onceNum = onceNum;
        if (_onceNum <= 0)
        {
            _onceNum = 1;
        }
    }

    protected override void OnTick()
    {
        for (int i = 0; i < _onceNum; i++)
        {
            Execute();
        }
    }
}
