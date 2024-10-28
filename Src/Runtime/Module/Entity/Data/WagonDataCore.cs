using GameMessageCore;


/// <summary>
/// 镖车实体核心数据
/// </summary>
public class WagonDataCore : EntityBaseComponent
{
    public int Cid { private set; get; }//镖配置ID
    public DREscortWagon Cfg { private set; get; }//镖配置ID
    public long AcceptTime { private set; get; }//接镖时间
    public long ExpireTime { private set; get; }//过期时间
    public long EscortTeamId { private set; get; }//护送队伍ID
    public long LeaderId { private set; get; }//队长ID
    public EscortWagonStatus Status { get; private set; } = EscortWagonStatus.Accept;//镖车初始状态

    public void Init(long teamId, long leaderId, int cid)
    {
        EscortTeamId = teamId;
        LeaderId = leaderId;
        Cid = cid;
        Cfg = GFEntryCore.DataTable.GetDataTable<DREscortWagon>().GetDataRow(cid);

        AcceptTime = TimeUtil.GetServerTimeStamp();
        ExpireTime = AcceptTime + Cfg.MaxExistenceTime * TimeUtil.S2MSInt;
    }

    public void Init(Wagon wagonNetData)
    {
        Init(wagonNetData.EscortTeamId, wagonNetData.LeaderId, wagonNetData.Cid);
    }

    public void SetStatus(EscortWagonStatus reason)
    {
        Status = reason;
    }
}