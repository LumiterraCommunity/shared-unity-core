/// <summary>
/// 区域信息
/// </summary>
public class SceneAreaInfo
{
    /// <summary>
    /// 区域类型标识，用于区域判断，存在多个相同区域
    /// </summary>
    public eSceneArea Area;
    /// <summary>
    /// 区域优先级，用于区域重叠时的优先级判断
    /// </summary>
    public eSceneAreaPriority Priority;
    /// <summary>
    /// 区域唯一标识
    /// 区域所在的gameObject的hashCode 
    /// </summary>
    public int AreaID;

    public SceneAreaInfo(eSceneArea area, eSceneAreaPriority priority, int areaID)
    {
        Area = area;
        Priority = priority;
        AreaID = areaID;
    }
}