public static class DRDropExtensions
{
    /// <summary>
    /// 是否需要玩家分享掉落
    /// </summary>
    /// <param name="drop"></param>
    /// <returns></returns>
    public static bool IsPlayerShareDrop(this DRDrop drop)
    {
        return drop.MaxSharer > 0;
    }
}