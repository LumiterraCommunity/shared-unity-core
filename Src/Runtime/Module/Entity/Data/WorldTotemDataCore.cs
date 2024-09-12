using GameMessageCore;

/// <summary>
/// 世界图腾数据
/// </summary>
public class WorldTotemDataCore : EntityBaseComponent
{
    /// <summary>
    /// main服的基础数据
    /// </summary>
    public WorldTotemInfo BaseData { get; private set; }
    /// <summary>
    /// 当前图腾配置 异常为null
    /// </summary>
    public DRTotem DRTotem { get; private set; }

    protected void Start()
    {
        RefEntity.EntityEvent.InitToScene += InitToScene;
        RefEntity.EntityEvent.UnInitFromScene += UnInitFromScene;
    }

    protected void OnDestroy()
    {
        RefEntity.EntityEvent.InitToScene -= InitToScene;
        RefEntity.EntityEvent.UnInitFromScene -= UnInitFromScene;
    }

    private void InitToScene(EntityBase entity)
    {
        GFEntryCore.GetModule<WorldTotemEntityMgrCore>().AddWorldTotem(RefEntity);
    }

    private void UnInitFromScene(EntityBase entity)
    {
        GFEntryCore.GetModule<WorldTotemEntityMgrCore>().RemoveWorldTotem(RefEntity);
    }

    /// <summary>
    /// 设置基础信息
    /// </summary>
    /// <param name="totemInfo"></param>
    public void SetBaseInfo(WorldTotemInfo totemInfo)
    {
        BaseData = totemInfo;
        DRTotem = TableUtil.GetConfig<DRTotem>(BaseData.Cid);
    }
}