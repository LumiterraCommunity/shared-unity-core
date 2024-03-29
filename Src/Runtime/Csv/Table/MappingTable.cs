using System;
using System.Collections.Generic;
using GameFramework.DataTable;
/// <summary>
/// 关系映射表
/// </summary>
public class MappingTable
{
    private static MappingTable s_inst;
    public static MappingTable Inst
    {
        get
        {
            if (s_inst == null)
            {
                s_inst = new MappingTable();
            }
            return s_inst;
        }
    }

    private readonly DRMapping[] _rows;
    private readonly Dictionary<int, DRMapping> _idSearchDic = new();
    private readonly Dictionary<int, DRMapping> _itemIdSearchDic = new();

    public MappingTable()
    {
        IDataTable<DRMapping> dt = GFEntryCore.DataTable.GetDataTable<DRMapping>();
        _rows = dt.GetAllDataRows();
        for (int i = 0; i < _rows.Length; i++)
        {
            _idSearchDic.Add(_rows[i].Id, _rows[i]);
            _itemIdSearchDic.Add(_rows[i].ItemId, _rows[i]);
        }
    }

    /// <summary>
    /// 通过id获取行
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public DRMapping GetRowById(int id)
    {
        return _idSearchDic.GetValueOrDefault(id, null);
    }

    /// <summary>
    /// 通过itemId获取行
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public DRMapping GetRowByItemId(int itemId)
    {
        return _itemIdSearchDic.GetValueOrDefault(itemId, null);
    }

    /// <summary>
    /// 获取符合条件的行
    /// !!每次查询都会遍历所有行，有性能开销，如果需要频繁调用，请自己加查询字典
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public DRMapping GetRow(Func<DRMapping, bool> func)
    {
        for (int i = 0; i < _rows.Length; i++)
        {
            if (func(_rows[i]))
            {
                return _rows[i];
            }
        }
        return null;
    }
}