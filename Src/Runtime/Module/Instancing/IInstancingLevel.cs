/*
 * @Author: xiang huan
 * @Date: 2023-09-26 17:06:34
 * @Description: 副本关卡接口
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Instancing/IInstancingLevel.cs
 * 
 */

/// <summary>
/// 副本关卡接口
/// </summary>
public interface IInstancingLevel
{
    /// <summary>
    /// 运行关卡
    /// </summary>
    bool RunLevel();
    /// <summary>
    /// 完成关卡
    /// </summary>
    bool CompleteLevel(bool isSuccess, bool isReward = true);
    /// <summary>
    /// 重置关卡
    /// </summary>
    bool ResetLevel();
}

