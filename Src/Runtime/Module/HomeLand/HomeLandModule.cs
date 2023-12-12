using System;
/// <summary>
/// 家园管理模块
/// </summary>
public class HomeLandModule : SceneModuleBase
{
    /// <summary>
    /// player 实体进入家园统一事件
    /// </summary>
    public static Action<EntityBase> OnPlayerEnterHomeLand = delegate { };
    /// <summary>
    /// player 实体离开家园统一事件
    /// </summary>
    public static Action<EntityBase> OnPlayerExitHomeLand = delegate { };
}