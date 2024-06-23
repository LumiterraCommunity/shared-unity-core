/// <summary>
/// 移动相关的定义
/// </summary>
public static class MoveDefine
{
    /// <summary>
    /// 移动最大请求间隔时间 s
    /// </summary>
    public const float MOVE_MAX_SYNC_INTERVAL_TIME = 1f;//TODO:0.5效果最好 目前为了解决服务器网络性能暂时改成1s
    public const float MOVE_MAX_SYNC_INTERVAL_TIME_INSTANCE = 0.2f;//副本走直线
    /// <summary>
    /// 方向偏移多少度就需要同步
    /// </summary>
    public const float MOVE_SYNC_DIRECTION_OFFSET_ANGLE = 20f;//TODO:10度效果最好 不会怎么闪 30度如果一直转圈容易闪 不过在服务器没有解决性能问题前这样可以保证服务器更安全 不一直转圈也还好
    public const float MOVE_SYNC_DIRECTION_OFFSET_ANGLE_INSTANCE = 5f;//副本偏移阈值
    /// <summary>
    /// 允许的网络延迟时间 s
    /// </summary>
    public const float MOVE_ALLOW_NETWORK_DELAY_TIME = 0.2f;
    /// <summary>
    /// 空中允许的网络延迟时间 s
    /// </summary>
    public const float AIR_MOVE_ALLOW_NETWORK_DELAY_TIME = 0.5f;
    /// <summary>
    /// 当放技能时 平移到达技能输入位置的时间 秒 目前用在客户端放技能 服务器和其他玩家能平移位置过去对齐
    /// </summary>
    public const float ARRIVED_INPUT_POS_TIME_ON_SKILL = 0.5f;

    /// <summary>
    /// 移动的步高 也就是能移动的直线高度
    /// </summary>
    public const float MOVE_STEP_HEIGHT = 0.3f;
    /// <summary>
    /// 能移动的布高占身高比例
    /// </summary>
    public const float MOVE_STEP_HEIGHT_RATIO = 0.15f;
    /// <summary>
    /// 最大爬坡度数
    /// </summary>
    public const float MOVE_SLOPE_LIMIT = 45f;
    /// <summary>
    /// 是否启用空中移动特性
    /// </summary>
    public const bool ENABLE_MOVE_IN_AIR = true;//暂时不开 开了后移动同步有点问题 还要时间解决 目前将空中摩擦力加上后体验还行
}