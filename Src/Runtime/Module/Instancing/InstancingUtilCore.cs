using UnityEngine;

/// <summary>
/// 副本相关工具
/// </summary>
public static class InstancingUtilCore
{
    /// <summary>
    /// 副本的等级（难度）转换为分数倍率
    /// </summary>
    /// <param name="difficulty"></param>
    /// <returns></returns>
    public static float InstancingLevelToScoreRate(int difficulty)
    {
        return Mathf.Pow(2, difficulty);
    }
}