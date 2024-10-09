/// <summary>
/// 世界图腾工具类
/// </summary>
public static class WorldTotemUtilCore
{
    /// <summary>
    /// 判断某个场景是否可以放置图腾
    /// </summary>
    /// <param name="curArea"></param>
    /// <returns></returns>
    public static bool JudgeSceneCanPlaceTotem(eSceneArea curArea)
    {
        return curArea == eSceneArea.World;
    }
}