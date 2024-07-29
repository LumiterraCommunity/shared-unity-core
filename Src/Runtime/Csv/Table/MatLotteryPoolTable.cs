using System.Collections.Generic;
using GameFramework.DataTable;

public class MatLotteryPoolTable
{
    private readonly DRMatLotteryPool[] _rows;
    private static MatLotteryPoolTable s_inst;
    public static MatLotteryPoolTable Inst
    {
        get
        {
            if (s_inst == null)
            {
                s_inst = new MatLotteryPoolTable();
            }
            return s_inst;
        }
    }

    private Dictionary<int, DRMatLotteryPool> _dic { get; set; } = new(); // key: 奖票CId , value: 奖票配置数据 
    public MatLotteryPoolTable()
    {
        IDataTable<DRMatLotteryPool> dt = GFEntryCore.DataTable.GetDataTable<DRMatLotteryPool>();
        _rows = dt.GetAllDataRows();
        foreach (DRMatLotteryPool row in _rows)
        {
            _dic.Add(row.LotteryItem, row);
        }
    }

    /// <summary>
    /// 通过奖票CId获取奖票配置数据 
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public DRMatLotteryPool GetRowByItemID(int itemID)
    {
        if (_dic.TryGetValue(itemID, out DRMatLotteryPool row))
        {
            return row;
        }
        return null;
    }
}