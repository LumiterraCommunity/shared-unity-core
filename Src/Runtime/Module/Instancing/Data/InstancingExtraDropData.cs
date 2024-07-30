using System;
using System.Collections.Generic;
using GameMessageCore;
using UnityGameFramework.Runtime;

/// <summary>
/// 副本额外掉落数据
/// </summary>
public class InstancingExtraDropData
{
    //额外掉落限制数量 key:额外掉落cid
    private readonly Dictionary<int, ItemCountInfo> _extraDropCountInfo = new();

    public bool NeedExtraDrop => _extraDropCountInfo.Count > 0;

    public void InitData(IEnumerable<ItemBaseInfo> items)
    {
        string log = "";
        foreach (ItemBaseInfo item in items)
        {
            _extraDropCountInfo.Add(item.Cid, new ItemCountInfo
            {
                Cid = item.Cid,
                MaxNum = item.Num,
                DropNum = 0
            });
            log += $"[cid = {item.Cid},num = {item.Num}] ";
        }
        Log.Info($"InstancingExtraDropData InitData: {log}");
    }

    /// <summary>
    /// 尝试额外掉落xx多少个 返回实际掉落数量
    /// </summary>
    /// <param name="cid"></param>
    /// <param name="num">想要掉落的数量</param>
    /// <returns></returns>
    public int TryExtraDrop(int cid, int num)
    {
        if (!_extraDropCountInfo.TryGetValue(cid, out ItemCountInfo countInfo))
        {
            Log.Error($"TryExtraDrop Error: cid not find = {cid},configNum = {_extraDropCountInfo.Count}");
            return 0;
        }

        int poolRemain = countInfo.GetRemainDropNum();
        if (poolRemain <= 0)
        {
            return 0;
        }

        int dropNum = Math.Min(num, poolRemain);
        countInfo.AddDropNum(dropNum);
        return dropNum;
    }

    private class ItemCountInfo
    {
        public int Cid;
        /// <summary>
        /// 最大掉落数量
        /// </summary>
        public int MaxNum;
        /// <summary>
        /// 已经掉落的数量
        /// </summary>
        public int DropNum;

        internal void AddDropNum(int dropNum)
        {
            DropNum += dropNum;

            if (DropNum > MaxNum)
            {
                Log.Error($"ItemCountInfo AddDropNum Error: DropNum > MaxNum,DropNum = {DropNum},MaxNum = {MaxNum}");
            }
        }

        internal int GetRemainDropNum()
        {
            return MaxNum - DropNum;
        }
    }
}