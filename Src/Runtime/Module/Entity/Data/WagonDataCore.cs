using System.Collections.Generic;
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
    // public List<long> TeamMembers { private set; get; }//护送者ID
    public EscortWagonFailReason FailReason { get; private set; } = EscortWagonFailReason.Unknown;//Unknown表示不失败
    public bool Arrived { get; private set; }//是否到达目

    public void Init(long teamId, int cid)
    {
        EscortTeamId = teamId;
        Cid = cid;
        Cfg = GFEntryCore.DataTable.GetDataTable<DREscortWagon>().GetDataRow(cid);

        AcceptTime = TimeUtil.GetServerTimeStamp();
        ExpireTime = AcceptTime + Cfg.MaxExistenceTime * TimeUtil.S2MSInt;
    }

    public void SetFailReason(EscortWagonFailReason reason)
    {
        FailReason = reason;
    }

    public void SetArrived()
    {
        Arrived = true;
    }
}