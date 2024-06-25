/// <summary>
/// 跟随宠
/// </summary>
public class PetFollowCore : EntityBaseComponent
{
    /// <summary>
    /// 宠物基础数据
    /// </summary>
    /// <value></value>
    public PetDataCore PetData { get; private set; }
    /// <summary>
    /// 跟随宠数据
    /// </summary>
    /// <value></value>
    public PetFollowDataCore FollowData { get; private set; }

    protected virtual void Start()
    {
        PetData = RefEntity.GetComponent<PetDataCore>();
        FollowData = RefEntity.GetComponent<PetFollowDataCore>();
    }
}