/*
 * @Author: xiang huan
 * @Date: 2023-11-03 09:51:24
 * @Description: 玩家副本数据
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Instancing/Data/PlayerInstancingData.cs
 * 
 */
using System.Collections.Generic;
public class PlayerInstancingData
{
    public long PlayerId; //玩家ID
    public bool IsSettlement; //是否结算
    public bool IsDeductTicket; //是否扣除门票
    public HashSet<int> CompleteLevelList = new(); //已经完成的关卡
    public int Score;                              //副本分数
    public float TotemScore;                       //图腾分数
    protected GameMessageCore.InstancingPlayerData NetData = new();
    public void SetData(long playerId, int score = 0, float totemScore = 0)
    {
        PlayerId = playerId;
        Score = score;
        TotemScore = totemScore;
    }

    public void SetNetData(GameMessageCore.InstancingPlayerData data)
    {
        PlayerId = data.PlayerId;
        Score = data.Score;
        TotemScore = data.TotemScore;
    }
    public GameMessageCore.InstancingPlayerData GetNetData()
    {
        NetData.PlayerId = PlayerId;
        NetData.Score = Score;
        NetData.TotemScore = TotemScore;
        return NetData;
    }
}