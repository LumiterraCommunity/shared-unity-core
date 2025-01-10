using UnityGameFramework.Runtime;


/// <summary>
/// 代码中的全局设置 设置其中值时需要谨慎
/// </summary>
public static class GlobalConfigCore
{
    /// <summary>
    /// 是否启用重力
    /// </summary>
    public static bool EnableGravity { get; private set; } = true;

    public static void SetGravityStatus(bool enable)
    {
        Log.Info($"SetGravityStatus: {enable}");
        EnableGravity = enable;
    }
}