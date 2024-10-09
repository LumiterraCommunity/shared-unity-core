using GameMessageCore;

/// <summary>
/// 世界图腾数据
/// </summary>
public class WorldTotemDataCore : EntityBaseComponent
{
    /// <summary>
    /// main服的基础数据
    /// </summary>
    public WorldTotemEntityInfo BaseData { get; private set; }
    /// <summary>
    /// 当前图腾配置 异常为null
    /// </summary>
    public DRTotem DRTotem { get; private set; }

    protected void Start()
    {
        GFEntryCore.GetModule<WorldTotemEntityMgrCore>().AddWorldTotem(this);
    }

    protected void OnDestroy()
    {
        GFEntryCore.GetModule<WorldTotemEntityMgrCore>().RemoveWorldTotem(this);
    }

    /// <summary>
    /// 设置基础信息
    /// </summary>
    /// <param name="totemInfo"></param>
    /// <param name="isInit">是初始化不会广播更新消息</param>
    public void SetBaseInfo(WorldTotemEntityInfo totemInfo, bool isInit)
    {
        BaseData = totemInfo;
        DRTotem = TableUtil.GetConfig<DRTotem>(BaseData.Cid);

        if (!isInit)
        {
            RefEntity.EntityEvent.WorldTotemBaseDataUpdate?.Invoke();
        }
    }
}